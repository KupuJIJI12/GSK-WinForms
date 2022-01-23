using System;
using System.Collections.Generic;
using System.Drawing;
using WinFormsApp1.Abstractions;

namespace WinFormsApp1.Shapes
{
    public class Figure2 : Shape
    {
        public double Radius { get; set; } = 50;

        public Figure2(Pen pen, Graphics graphics, double radius) : base(pen, graphics)
        {
            Radius = radius;
        }

        public override void Draw(List<Point> points)
        {
            Points.Clear();
            
            Points.Add(new PointF(points[0].X, points[0].Y));
            
            var newPoint = new PointF(Points[0].X - Convert.ToInt32(Radius), Points[0].Y);
            Points.Add(newPoint);
            newPoint = new PointF(Points[Points.Count - 1].X, Points[Points.Count - 1].Y + (float) (Radius));
            Points.Add(newPoint);
            newPoint = new PointF(Points[Points.Count - 1].X - (float) (Radius / 2.0), Points[Points.Count - 1].Y + (float) (Radius / 2.0));
            Points.Add(newPoint);
            newPoint = new PointF(Points[Points.Count - 1].X + (float) (Radius * 2.0), Points[Points.Count - 1].Y);
            Points.Add(newPoint);
            newPoint = new PointF(Points[Points.Count - 1].X - (float) (Radius / 2.0), Points[Points.Count - 1].Y - (float) (Radius/ 2.0));
            Points.Add(newPoint);
                
            Points.Add(Points[0]);
            
            Graphics.DrawLines(Pen, Points.ToArray());
        }

        public override void ReDraw()
        {
            Graphics.DrawLines(Pen, Points.ToArray());
        }

        public override void ChangeScale(double radius)
        {
            Radius += radius;

            Draw(new List<Point>(){new((int) Points[0].X,(int) Points[0].Y)});
        }

        public override void Rotate(double angle)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                var x = (float) (Points[i].X * Math.Cos(angle)) - (float) (Points[i].Y * Math.Sin(angle));
                var y = (float) (Points[i].X * Math.Sin(angle)) + (float) (Points[i].Y * Math.Cos(angle));

                Points[i] = new PointF(x, y);
            }

            Graphics.DrawLines(Pen, Points.ToArray());
        }
    }
}