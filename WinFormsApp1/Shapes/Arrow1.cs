using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Shapes
{
    public class Arrow1 : Shape
    {
        public Arrow1(Pen pen, Graphics graphics) : base(pen, graphics)
        {
        }

        public override void DrawShape(List<Point> points)
        {
            Pen pen = new Pen(Color.Chocolate, 8);
            pen.StartCap = LineCap.NoAnchor;
            pen.EndCap = LineCap.ArrowAnchor;
            Graphics.DrawLine(pen, points[0].X, points[0].Y, points[0].X+90, points[0].Y);
        }
    }
}
