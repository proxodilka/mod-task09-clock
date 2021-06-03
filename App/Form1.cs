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

namespace clock
{
    public partial class Form1 : Form
    {
        int clockRadius = 200, secondHandLength = 180, minuteHandLength = 130, hourHandLength = 90;
        public Form1()
        {
            InitializeComponent();
            Paint += Draw;
            var timer = new Timer { Interval=1000 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graph.Clear(Color.White);

            int offset = 10;
            Point center = new Point(Width / 2 - offset, Height / 2 - offset);

            graph.TranslateTransform(center.X, center.Y);
            float hz = Math.Min(Width, Height) - 10 * offset;
            graph.ScaleTransform(
                hz / (2 * clockRadius), hz / (2 * clockRadius)
            );
            
            center = new Point(0, 0);

            Rectangle clockRecrangle = new Rectangle(
                Point.Subtract(center, new Size(clockRadius, clockRadius)),
                new Size(clockRadius * 2, clockRadius * 2)
            );
            graph.DrawEllipse(new Pen(Color.Black, 2), clockRecrangle);

            GraphicsState defaultGraphState = graph.Save();
            for (int i=0; i<12; i++)
            {
                int lineLength = 30;
                graph.RotateTransform(15);
                graph.DrawLine(
                    new Pen(Color.Black, 2),
                    new Point(0, -clockRadius + lineLength / 3),
                    new Point(0, -clockRadius)
               );
               graph.RotateTransform(15);
               graph.DrawLine(
                   new Pen(Color.Black, 2),
                   new Point(0, -clockRadius + lineLength),
                   new Point(0, -clockRadius)
               );
            }
            graph.Restore(defaultGraphState);

            var timeStamp = DateTime.Now;
            float secondHandAngle = timeStamp.Second * 6;
            float minuteHandAngle = (timeStamp.Minute + timeStamp.Second / 60f) * 6;
            float hourHandAngle = (timeStamp.Hour + timeStamp.Minute / 60f) * 30;

            defaultGraphState = graph.Save();

            graph.RotateTransform(secondHandAngle);
            graph.DrawLine(new Pen(Color.Black, 2), center, Point.Add(center, new Size(0, -secondHandLength)));
            graph.Restore(defaultGraphState);
            defaultGraphState = graph.Save();

            graph.RotateTransform(minuteHandAngle);
            graph.DrawLine(new Pen(Color.Black, 2), center, Point.Add(center, new Size(0, -minuteHandLength)));
            graph.Restore(defaultGraphState);

            defaultGraphState = graph.Save();
            graph.RotateTransform(hourHandAngle);
            graph.DrawLine(new Pen(Color.Black, 2), center, Point.Add(center, new Size(0, -hourHandLength)));
            graph.Restore(defaultGraphState);

            graph.DrawString(
                timeStamp.ToLongTimeString(),
                new Font("Arial", 20f),
                Brushes.Black,
                Point.Add(center, new Size(0, offset))
            );
        }
    }
}
