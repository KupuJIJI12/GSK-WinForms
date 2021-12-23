using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1.Abstractions
{
    public abstract class Shape
    {
        public  Pen Pen { get; set; }
        protected readonly Graphics Graphics;
        public List<PointF> Points { get; set; }

        protected Shape(Pen pen, Graphics graphics)
        {
            Graphics = graphics;
            Pen = pen;
            Points = new List<PointF>();
        }

        public abstract void Draw(List<Point> points);
        public abstract void ReDraw();

        public virtual void Rotate(double angle){}
        
        protected static double Factorial(int a)
        {
            switch (a)
            {
                case 0:
                case 1:
                    return 1;
                case 2:
                    return 2;
                default:
                    return a * Factorial(a - 1);
            }
        }
    }
}