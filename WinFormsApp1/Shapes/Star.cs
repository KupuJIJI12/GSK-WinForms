using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WinFormsApp1.Abstractions;

namespace WinFormsApp1.Shapes
{
    public class Star : Shape
    {
        private readonly int _peaksCount;
        private int _radius;
        private double _angle;

        public Star(Pen pen, Graphics graphics, int peaksCount, double angle = 0, int radius = 50) : base(pen, graphics)
        {
            _peaksCount = peaksCount;
            _radius = radius;
            _angle = angle;
        }

        public override void DrawShape(List<Point> points)
        {
            var pointsF = GetNeededPoints(points[0]).ToArray();
 
            Graphics.DrawLines(Pen, pointsF);
        }

        private IEnumerable<PointF> GetNeededPoints(Point center)
        { 
            var r = _radius / 2.0; 
            var x0 = center.X;
            var y0 = center.Y;
            var points = new PointF[2 * _peaksCount + 1];
            var da = Math.PI / _peaksCount;

            for (var i = 0; i < 2 * _peaksCount + 1; i++)
            {
                var length = i % 2 == 0 ? _radius : r;
                points[i] = new PointF((float)(x0 + length * Math.Cos(_angle)), (float)(y0 + length * Math.Sin(_angle)));
                _angle += da;
            }

            return points;
        }
    }
}