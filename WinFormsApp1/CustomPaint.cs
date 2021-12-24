using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly PaintManager _paintManager;
        
        public Form1()
        {
            InitializeComponent();
            
            var graphics = pictureBox1.CreateGraphics();
            _paintManager = new PaintManager(graphics);
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _paintManager.Clear();
                
                return;
            }

            _paintManager.DrawShape(e);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            _paintManager.SelectedShape = comboBox.Text;
            
            if (comboBox.Text == "Звезда")
            {
                _paintManager.UseFigureCustomizer();
            }
        }

        private void MouseEventHandler(object sender, MouseEventArgs e)
        {
            _paintManager.ExecuteShapeOperation(e);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            _paintManager.SelectedOperation = comboBox.Text;
        }
    }
}