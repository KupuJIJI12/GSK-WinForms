using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1
{
    public interface IShape
    {
        void DrawShape(List<Point> points, int pointsCount);
    }
}