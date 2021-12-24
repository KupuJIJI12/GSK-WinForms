﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Shapes;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly Graphics _graphics;
        private List<Point> _points;
        private readonly Shape _bezierCurve;
        private Shape _shape;
        private string _selectedShape;

        public Form1()
        {
            InitializeComponent();
            _graphics = pictureBox1.CreateGraphics();
            
            _points = new List<Point>();
            _bezierCurve = new BezierCurve(new Pen(Color.Black), _graphics);
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _graphics.Clear(Color.White);
                _points.Clear();

                return;
            }

            if (_selectedShape == "Звезда" || _selectedShape == "Ромб" || _selectedShape == "Стрелка1")
            {
                _shape.DrawShape(new List<Point> { new(e.X, e.Y) });

                return;
            }

            _points.Add(new Point(e.X, e.Y));
            _graphics.DrawEllipse(Pens.Crimson, e.X - 2, e.Y - 2, 5, 5);

            if (e.Button == MouseButtons.Right) // Конец ввода
            {
                var countPoints = _points.Count - 1;
                
                _graphics.DrawLine(new Pen(Color.Magenta, 1), _points[countPoints - 1], _points[countPoints]);
                _bezierCurve.DrawShape(_points);
                
                _points.Clear();
            }
            else
            {
                var countPoints = _points.Count - 1;
                // Прямая - при двух точках
                if(countPoints == 1) _graphics.DrawLine(new Pen(Color.Magenta, 1), _points[countPoints - 1], _points[countPoints]);
                //Кривая Безье при более двух.
                if (countPoints > 1)
                {
                    _graphics.DrawLine(new Pen(Color.Magenta, 1), _points[countPoints - 1], _points[countPoints]);
                }
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            _selectedShape = comboBox.Text;
            if (comboBox.Text == "Звезда")
            {
                int count;
                using var starForm = new ZvezdaWindow();
                //С этим надо быть поаккуратней))0)
                (starForm.Controls[0] as Button).MouseDown += (_, _) =>
                {
                    //а с этим вдвойне -_-
                    count = int.Parse((starForm.Controls[2] as NumericUpDown).Text);
                    _shape = new Star(new Pen(Color.Chocolate), _graphics, count);
                    
                    starForm.Close();
                };
                
                starForm.ShowDialog();
            }

            if (comboBox.Text == "Ромб") _shape = new Romb(new Pen(Color.Chocolate), _graphics);

            if (comboBox.Text == "Стрелка1") _shape = new Arrow1(new Pen(Color.Chocolate), _graphics);
        }
    }
}