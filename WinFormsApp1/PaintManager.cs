using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.Abstractions;
using WinFormsApp1.Shapes;

namespace WinFormsApp1
{
    public class PaintManager
    {
        private readonly Graphics _graphics;
        private readonly List<Point> _points;
        private readonly List<Shape> _shapes;
        
        private Shape _selectedShape;
        private int _peaksCount = 5;
        public string SelectedShape { get; set; } = "Звезда";
        public string SelectedOperation { get; set; } = "Поворот";

        public PaintManager(Graphics graphics)
        {
            _graphics = graphics;
            
            _points = new List<Point>();
            _shapes = new List<Shape>();
        }

        private bool SelectShape(Point point)
        {
            if (_selectedShape is not null && !_shapes.Contains(_selectedShape))
            {
                _shapes.Add(_selectedShape);
            }
                    
            foreach (var value in _shapes)
            {
                var xs = value.Points.Select(p => p.X).ToArray();
                var ys = value.Points.Select(p => p.Y).ToArray();
                var minX = xs.Min();
                var maxX = xs.Max();
                var minY = ys.Min();
                var maxY = ys.Max();

                if (minX <= point.X && maxX >= point.X && minY <= point.Y && maxY >= point.Y)
                {
                    if (_selectedShape == value) return true;
                    
                    if (_selectedShape is not null)
                    {
                        if(!_selectedShape.IsPainted)
                            _selectedShape.Pen = Pens.Chocolate;
                    }
                    
                    if(!value.IsPainted)
                        value.Pen = Pens.DodgerBlue;
                    
                    _selectedShape = value;

                    ReDrawExitingShapes();

                    return true;
                }
            }

            return false;
        }

        private void ReDrawExitingShapes()
        {
            _graphics.Clear(Color.White);
            _points.Clear();
            foreach (var shape in _shapes)
            {
                shape.ReDraw();
                if (shape.IsPainted)
                {
                    PaintOverShape(shape.Pen.Brush, shape);
                }
            }
        }
        
        private static void Sort(IList<PointF> a)
        {
            var i = 1;
            while (i < a.Count)
            {
                if (i == 0 || a[i - 1].X <= a[i].X)
                {
                    i++;
                }
                else
                {
                    (a[i], a[i - 1]) = (a[i - 1], a[i]);
                    i--;
                }
            }
        }

        private static bool LineIntersected(float yI, float yK, float yLine)
        {
            return (yI < yLine && yK >= yLine) || (yI >= yLine && yK < yLine);
        }

        public void Clear()
        {
            _graphics.Clear(Color.White);
            _shapes.Clear();
            _points.Clear();
            _selectedShape = null;
        }

        public void DrawShape(MouseEventArgs e)
        {
            if (SelectedShape == "Звезда")
            {
                if (_shapes.Count != 0)
                {
                    var b = SelectShape(new Point(e.X, e.Y));

                    if (b)
                    {
                        return;
                    }
                }

                var star = new Star(new Pen(Color.Chocolate), _graphics, _peaksCount);
                    
                _shapes.Add(star);
                star.Draw(new List<Point> {new(e.X, e.Y)});

                _points.Clear();
            }
            else if (SelectedShape == "Безье")
            {
                _points.Add(new Point(e.X, e.Y));
                _graphics.DrawEllipse(Pens.Crimson, e.X - 2, e.Y - 2, 5, 5);
                
                if (e.Button == MouseButtons.Right) // Конец ввода
                {
                    var countPoints = _points.Count - 1;
                    var bezierCurve = new BezierCurve(Pens.Black, _graphics);
                
                    _graphics.DrawLine(new Pen(Color.Magenta, 1), _points[countPoints - 1], _points[countPoints]);
                    bezierCurve.Draw(_points);

                    _points.Clear();
                }
                else
                {
                    var countPoints = _points.Count - 1;
                    if (countPoints > 0)
                    {
                        _graphics.DrawLine(new Pen(Color.Magenta, 1), _points[countPoints - 1], _points[countPoints]);
                    }
                }
            }
            else if (SelectedShape == "Фигура2")
            {
                _points.Add(new Point(e.X, e.Y));
                
                if (_shapes.Count != 0)
                {
                    var b = SelectShape(new Point(e.X, e.Y));

                    if (b)
                    {
                        return;
                    }
                }
                
                var figure = new Figure2(Pens.Green, _graphics, 50);

                figure.Draw(_points);
                
                _shapes.Add(figure);
                _points.Clear();
            }
        }

        public void ExecuteShapeOperation(MouseEventArgs e)
        {
            if (_selectedShape is null) return;

            switch (SelectedOperation)
            {
                case "Поворот":
                {
                    var angle = e.Delta / 40.0;

                    _shapes.Remove(_selectedShape);
                    ReDrawExitingShapes();
                    _selectedShape.Rotate(angle);

                    break;
                }
                case "Масштаб":
                {
                    var radius = e.Delta > 0 ? 10.0 : -10.0;

                    _shapes.Remove(_selectedShape);
                    ReDrawExitingShapes();
                    _selectedShape.ChangeScale(radius);

                    break;
                }
                case "Зеркало":
                {
                    _selectedShape.Points = _selectedShape.Points
                        .Select(p =>
                        {
                            var diff = e.X - p.X;
                            p.X = e.X + diff;
                            diff = e.Y - p.Y;
                            p.Y = e.Y + diff;

                            return p;
                        })
                        .ToList();

                    ReDrawExitingShapes();

                    break;
                }
            }
            
            _shapes.Add(_selectedShape);
            if (_selectedShape.IsPainted)
            {
                PaintOverShape(_selectedShape.Pen.Brush, _selectedShape);
            }
        }
        
        public void UseFigureCustomizer()
        {
            int count;
            var starForm = new ZvezdaWindow();
            
            //Применить
            (starForm.Controls[0] as Button).MouseDown += (_, _) =>
            {
                count = int.Parse((starForm.Controls[2] as NumericUpDown).Text);
                _peaksCount = count;
                    
                starForm.Close();
                starForm.Dispose();
            };
                
            starForm.ShowDialog();
        }

        public void PaintOverShape(Brush brush, Shape shape = default)
        {
            shape ??= _selectedShape;
            shape.IsPainted = true;
            shape.Pen = new Pen(brush);
            
            var points = shape.Points;
            var ys = points
                .Select(p => p.Y)
                .ToArray();
            
            var pointsCount = points.Count;
            var yMin = ys.Min();
            var yMax = ys.Max();

            for (var y = yMin; y < yMax; y++)
            {
                var xb = new List<PointF>();
                for (var i = 0; i < pointsCount; i++)
                {
                    var k = i < points.Count - 1 ? i + 1 : 0;
                    if (LineIntersected(points[i].Y, points[k].Y, y))
                    {
                        xb.Add(new PointF
                        {
                            X = ((points[k].X - points[i].X) *
                                    y + (points[i].X * points[k].Y - points[k].X * points[i].Y))
                                / (points[k].Y - points[i].Y),
                            Y = y
                        });
                    }
                }

                Sort(xb);
                for (var i = 0; i < xb.Count; i += 2)
                {
                    _graphics.DrawLine(new Pen(brush), xb[i], xb[i + 1]);
                }

                xb.Clear();
                
            }
        }

        /*public void MoveShape(int x, int y)
        {
            if (_selectedShape is not null)
            {
                _shapes.Remove(_selectedShape);
                ReDrawExitingShapes();
                _selectedShape.Draw(new List<Point>{new(x, y)});
            }
        }*/
    }
}