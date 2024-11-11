using IzMusics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IzMusic
{
    public partial class IzMusic : Form
    {

        public IzMusic()
        {
            InitializeComponent();
        }

        private void roundButton1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush linGrBrush = new LinearGradientBrush(new Point(10, 100), new Point(200, 10), Color.FromArgb(255, 0, 0, 255), Color.FromArgb(255, 255, 0, 0));
            Pen pen = new Pen(linGrBrush);
            e.Graphics.FillRectangle(linGrBrush, 0, 0, 400, 42);
            g.DrawString("Войти", new Font("Segoe UI", 12, FontStyle.Bold), System.Drawing.Brushes.White, new Point(15, 10));
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.ForeColor = Color.White;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.ForeColor = Color.Gray;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.ForeColor = Color.White;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Gray;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.ForeColor = Color.White;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.ForeColor = Color.Gray;
        }
    }
}
