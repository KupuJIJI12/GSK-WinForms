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
        public double Radius { get; set; } = 50;
        public double Angle { get; set; } = 0;
        public Point Center { get; set; }

        public Star(Pen pen, Graphics graphics, int peaksCount) : base(pen, graphics)
        {
            _peaksCount = peaksCount;
        }

        public override void Draw(List<Point> points)
        {
            Center = points[0];
            var pointsF = GetNeededPoints(Center).ToArray();

            Points.AddRange(pointsF);
            
            Graphics.DrawLines(Pen, pointsF);
        }

        public override void Rotate(double angle)
        {
            Angle += angle;
            var pointsF = GetNeededPoints(Center).ToArray();

            Points = new List<PointF>(pointsF);

            Graphics.DrawLines(Pen, pointsF);
        }

        public override void ChangeScale(double radius)
        {
            Radius += radius;
            var pointsF = GetNeededPoints(Center).ToArray();

            Points = new List<PointF>(pointsF);

            Graphics.DrawLines(Pen, pointsF);;
        }

        public override void ReDraw()
        {
            Graphics.DrawLines(Pen, Points.ToArray());
        }

        private IEnumerable<PointF> GetNeededPoints(Point center)
        { 
            var r = Radius / 2.0;
            var angle = Angle;
            var x0 = center.X;
            var y0 = center.Y;
            var points = new PointF[2 * _peaksCount + 1];
            var da = Math.PI / _peaksCount;

            for (var i = 0; i < 2 * _peaksCount + 1; i++)
            {
                var length = i % 2 == 0 ? Radius : r;
                points[i] = new PointF((float)(x0 + length * Math.Cos(angle)), (float)(y0 + length * Math.Sin(angle)));
                angle += da;
            }

            return points;
        }
    }
}