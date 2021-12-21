using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1.Shapes
{
    public class BezierCurve : Shape
    {
        public BezierCurve(Pen pen, Graphics graphics) : base(pen, graphics)
        { }

        public override void DrawShape(List<Point> points)
        {
            var pointsCount = points.Count - 1;
            var nFact = Factorial(pointsCount);
            const double dt = 0.01;
            var t = dt;
            int xPred = points[0].X, yPred = points[0].Y;
            
            while (t < 1 + dt / 2)
            {
                int xt = 0, yt = 0;
                var i = 0;
                while (i <= pointsCount)
                {
                    var j = Math.Pow(t, i) * Math.Pow(1 - t, pointsCount - i) * (nFact / (Factorial(i) * Factorial(pointsCount - i)));
                    xt += (int) Math.Round(points[i].X * j);
                    yt += (int) Math.Round(points[i].Y * j);
                    
                    i++;
                }

                Graphics.DrawLine(Pen, new PointF(xPred, yPred), new PointF(xt, yt));
                
                t += dt;
                xPred = xt;
                yPred = yt;
            }
        }
    }
}