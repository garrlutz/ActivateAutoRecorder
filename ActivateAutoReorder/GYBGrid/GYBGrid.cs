using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corel = Corel.Interop.VGCore;

namespace ActivateAutoReorder.GYBGrid
{
    public class GYBGrid : corel.ShapeRangeClass
    {
        private double _marginTop;
        private double _marginBottom;
        private double _marginLeft;
        private double _marginRight;

        public double MarginTop
        {
            get { return _marginTop; }
            set { _marginTop = value; }
        }


        public double MarginBottom
        {
            get { return _marginBottom; }
            set { _marginBottom = value; }
        }


        public double MarginLeft
        {
            get { return _marginLeft; }
            set { _marginLeft = value; }
        }


        public double MarginRight
        {
            get { return _marginRight; }
            set { _marginRight = value; }
        }






        public GYBGrid()
        {
            this.SetMargin(1, 1, 1, 1);
        }

        private void SetMargin(double left, double top, double right, double bottom)
        {
            _marginLeft = left;
            _marginTop = top;
            _marginRight = right;
            _marginBottom = bottom;
        }
    }
}
