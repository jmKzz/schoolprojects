using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace koosteteht
{
    class OmaCanvas : Canvas
    {

        public double width { get; set; }
        public double height { get; set; }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Pen p = new Pen(Brushes.Black, 10);
            dc.DrawEllipse(Brushes.Transparent, p,
                new System.Windows.Point(430, 360),
                width / 2, height / 2);

        }
    }
}
