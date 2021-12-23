using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WinFormsApp1.Abstractions;

namespace WinFormsApp1.Shapes
{
    public sealed class BezierCurve : Shape
    {
        public BezierCurve(Pen pen, Graphics graphics) : base(pen, graphics)
        { }

        public override void Draw(List<Point> points)
        {
            var neededPoints = GetCalculatedPoints(points).ToArray();

            Graphics.DrawLines(Pen, neededPoints);
        }

        public override void ReDraw()
        {
            
        }

        private static IEnumerable<PointF> GetCalculatedPoints(IReadOnlyList<Point> points)
        {
            const double dt = 0.04;

            var neededPoints = new List<PointF>();
            var pointsCount = points.Count - 1;
            var nFact = Factorial(pointsCount);
            var t = dt;
            var xPred = points[0].X;
            var yPred = points[0].Y;

            while (t < 1 + dt / 2)
            {
                var xt = 0;
                var yt = 0;
                var i = 0;
                while (i <= pointsCount)
                {
                    var j = Math.Pow(t, i) * Math.Pow(1 - t, pointsCount - i) * (nFact / (Factorial(i) * Factorial(pointsCount - i)));
                    
                    xt += (int) Math.Round(points[i].X * j);
                    yt += (int) Math.Round(points[i].Y * j);
                    
                    i++;
                }

                neededPoints.AddRange(new []{new PointF(xPred, yPred), new PointF(xt, yt)});
                //Graphics.DrawLine(Pen, new PointF(xPred, yPred), new PointF(xt, yt));
                
                t += dt;
                xPred = xt;
                yPred = yt;
            }
            
            return neededPoints; 
        }
    }
}