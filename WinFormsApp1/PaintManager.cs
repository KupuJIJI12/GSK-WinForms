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
                    if(_selectedShape is not null)
                        _selectedShape.Pen = Pens.Chocolate;
                            
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
            }
        }

        public void Clear()
        {
            _graphics.Clear(Color.White);
            _shapes.Clear();
            _points.Clear();
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
        }

        public void ExecuteShapeOperation(MouseEventArgs e)
        {
            switch (SelectedOperation)
            {
                case "Поворот":
                {
                    if (_selectedShape is Star star)
                    {
                        var angle = e.Delta / 40.0;

                        _shapes.Remove(_selectedShape);
                        ReDrawExitingShapes();
                        star.Rotate(angle);
                    }
                    break;
                }
                case "Масштаб":
                {
                    if (_selectedShape is Star star)
                    {
                        var radius = e.Delta > 0 ? 10.0 : -10.0;

                        _shapes.Remove(_selectedShape);
                        ReDrawExitingShapes();
                        star.ChangeScale(radius);
                    }
                    break;
                }
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
    }
}