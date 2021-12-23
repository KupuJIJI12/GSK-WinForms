using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.Abstractions;
using WinFormsApp1.Shapes;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly Graphics _graphics;
        private readonly List<Point> _points;
        private readonly List<Shape> _shapes;
        
        private int _peaksCount = 5;
        private Shape _star;
        private string _comboBoxSelectedShape = "Звезда";
        private string _comboBoxSelectedOperation = "Поворот";
        private Shape _selectedShape; 

        public Form1()
        {
            InitializeComponent();
            _graphics = pictureBox1.CreateGraphics();
            
            _points = new List<Point>();
            _shapes = new List<Shape>();
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _graphics.Clear(Color.White);
                _shapes.Clear();
                _points.Clear();

                return;
            }

            if (_comboBoxSelectedShape == "Звезда")
            {
                if (_shapes.Count != 0)
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

                        if (minX <= e.X && maxX >= e.X && minY <= e.Y && maxY >= e.Y)
                        {
                            if(_selectedShape is not null)
                                _selectedShape.Pen = Pens.Chocolate;
                            
                            value.Pen = Pens.DodgerBlue;
                            _selectedShape = value;
                            
                            ReDrawExitingShapes();
                            
                            return;
                        }
                    }
                }

                var star = new Star(new Pen(Color.Chocolate), _graphics, _peaksCount);
                    
                _shapes.Add(star);
                star.Draw(new List<Point> {new(e.X, e.Y)});

                _points.Clear();
            }
            else if (_comboBoxSelectedShape == "Безье")
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

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            _comboBoxSelectedShape = comboBox.Text;
            if (comboBox.Text == "Звезда")
            {
                int count;
                using var starForm = new ZvezdaWindow();
                //Применить
                (starForm.Controls[0] as Button).MouseDown += (_, _) =>
                {
                    count = int.Parse((starForm.Controls[2] as NumericUpDown).Text);
                    _peaksCount = count;
                    
                    starForm.Close();
                };
                
                starForm.ShowDialog();
            }
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

        private void MouseEventHandler(object sender, MouseEventArgs e)
        {
            switch (_comboBoxSelectedOperation)
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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            _comboBoxSelectedOperation = comboBox.Text;
        }
    }
}