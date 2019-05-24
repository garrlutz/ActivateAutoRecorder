using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using corel = Corel.Interop.VGCore;
using System.Windows.Media.Animation;

namespace ActivateAutoReorder
{
    /// <summary>
    /// This Addon will allow the user to toggle cell
    /// reordering on and off.
    /// </summary>
    public partial class ControlUI : UserControl
    {
        private corel.Application corelApp;
        private corel.Document _currentDocument;
        private corel.Shape _initalShape;
        private Styles.StylesController stylesController;

        private bool _toggled = false;
        private Thickness Left = new Thickness(-8, 0, 0, 0);
        private Thickness Right = new Thickness(0, 0, -8, 0);
        SolidColorBrush OnBrush = new SolidColorBrush(Color.FromRgb(0, 230, 118));
        SolidColorBrush OffBrush = new SolidColorBrush(Color.FromRgb(238, 238, 238));
        public bool Toggled { get => _toggled; set => _toggled = value; }

        private Storyboard ToggleOnStoryboard = new Storyboard();
        private Storyboard ToggleOffStoryboard = new Storyboard();



        public ControlUI(object app)
        {
            InitializeComponent();
            try
            {
                this.corelApp = app as corel.Application;
                stylesController = new Styles.StylesController(this.Resources, this.corelApp);
            }
            catch
            {
                global::System.Windows.MessageBox.Show("VGCore Error");
            }

            //Initialize Default Toggle State
            Toggled = false;
            ToggleCircle.Margin = Left;
            ToggleIndicator.Fill = OffBrush;

            this.RegisterName("ToggleBrush", OffBrush);

            //Initialize Animations
            ThicknessAnimation toggleOnAnimation = new ThicknessAnimation();
            toggleOnAnimation.From = Left;
            toggleOnAnimation.To = Right;
            toggleOnAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            toggleOnAnimation.AutoReverse = false;

            ThicknessAnimation toggleOffAnimation = new ThicknessAnimation();
            toggleOffAnimation.From = Right;
            toggleOffAnimation.To = Left;
            toggleOffAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            toggleOffAnimation.AutoReverse = false;

            ColorAnimation toggleOffColorAnimation = new ColorAnimation();
            toggleOffColorAnimation.From = OnBrush.Color;
            toggleOffColorAnimation.To = OffBrush.Color;
            toggleOffColorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            toggleOffColorAnimation.AutoReverse = false;

            ColorAnimation toggleOnColorAnimation = new ColorAnimation();
            toggleOnColorAnimation.From = OffBrush.Color;
            toggleOnColorAnimation.To = OnBrush.Color;
            toggleOnColorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            toggleOnColorAnimation.AutoReverse = false;


            ToggleOnStoryboard.Children.Add(toggleOnAnimation);
            ToggleOnStoryboard.Children.Add(toggleOnColorAnimation);
            Storyboard.SetTarget(toggleOnAnimation, ToggleCircle);
            Storyboard.SetTargetName(toggleOnColorAnimation, "ToggleBrush");
            Storyboard.SetTargetProperty(toggleOnAnimation, new PropertyPath(MarginProperty));
            Storyboard.SetTargetProperty(toggleOnColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            ToggleOffStoryboard.Children.Add(toggleOffAnimation);
            ToggleOffStoryboard.Children.Add(toggleOffColorAnimation);
            Storyboard.SetTarget(toggleOffAnimation, ToggleCircle);
            Storyboard.SetTargetName(toggleOffColorAnimation, "ToggleBrush");
            Storyboard.SetTargetProperty(toggleOffAnimation, new PropertyPath(MarginProperty));
            Storyboard.SetTargetProperty(toggleOffColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            btn_Command.Click += ToggleAutoReorder;
            corelApp.DocumentOpen += CorelApp_DocumentOpen;
            corelApp.DocumentNew += CorelApp_DocumentNew;
            
            
        }

        private void CorelApp_DocumentNew(corel.Document Doc, bool FromTemplate, string Template, bool IncludeGraphics)
        {
            throw new NotImplementedException();
        }

        private void ToggleAutoReorder(object sender, RoutedEventArgs e)
        {

            if (!Toggled)
            {
                Toggled = true;
                ToggleOnStoryboard.Begin(this);
                

            }
            else
            {
                Toggled = false;
                ToggleOffStoryboard.Begin(this);

            }
        }

        private void CorelApp_DocumentOpen(corel.Document Doc, string FileName)
        {
            corelApp.MsgShow("some shit is happening 1");
            _currentDocument = Doc;
            _currentDocument.ShapeMove += _currentDocument_ShapeMove;
            
        }

        private void _currentDocument_ShapeMove(corel.Shape currentShape)
        {
            corelApp.MsgShow("x: " + currentShape.CenterX + "and y:" + currentShape.CenterY);
            corelApp.MsgShow("some shit is happening 2");
        }


        
        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            stylesController.LoadThemeFromPreference();
        }

    }
}
