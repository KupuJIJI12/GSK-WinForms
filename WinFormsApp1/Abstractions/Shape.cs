using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1.Abstractions
{
    public abstract class Shape
    {
        protected readonly Pen Pen;
        protected readonly Graphics Graphics;

        protected Shape(Pen pen, Graphics graphics)
        {
            Graphics = graphics;
            Pen = pen;
        }

        public abstract void DrawShape(List<Point> points);
        
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