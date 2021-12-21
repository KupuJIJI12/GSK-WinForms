using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinFormsApp1.Shapes
{
    public class Star : Shape
    {
        private readonly int _sidesCount;

        public Star(Pen pen, Graphics graphics, int sidesCount) : base(pen, graphics)
        {
            _sidesCount = sidesCount;
        }

        public override void DrawShape(List<Point> points)
        {
            var pointsF = GetNeededPoints(points[0]).ToArray();
 
            Graphics.DrawLines(Pen, pointsF);
        }

        private IEnumerable<PointF> GetNeededPoints(Point center)
        { 
            double R = 25, r = 50;   // радиусы
            double alpha = 0;        // поворот
            double x0 = center.X, y0 = center.Y; // центр
 
            var points = new PointF[2 * _sidesCount + 1];
            double a = alpha, da = Math.PI / _sidesCount, l;
            for (int k = 0; k < 2 * _sidesCount + 1; k++)
            {
                l = k % 2 == 0 ? r : R;
                points[k] = new PointF((float)(x0 + l * Math.Cos(a)), (float)(y0 + l * Math.Sin(a)));
                a += da;
            }

            return points;
        }
    }
}