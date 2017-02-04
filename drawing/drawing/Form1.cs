using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace drawing
{
    public partial class Form1 : Form
    {
        // List containing every particle on screen
        private List<particle> particles { get; set; } = new List<particle>();
        private bool spawn = false;

        public Form1()
        {
            // Initialize the form
            InitializeComponent();

            // Start the main loop timer
            System.Timers.Timer timer = new System.Timers.Timer(1000/30);
            timer.Elapsed += Timer_Tick;
            timer.Start();

            // set the maximum particle count
            particles.Capacity = 10000;
        }

        /// <summary>
        /// Timer which serves as the main loop for the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {

            if (spawn)
            {
                lock (particles)
                    for (int i = 0; i < 10; i++)
                        particles.Add(new particle(this.Width/2,this.Height/2));
            }
            lock (particles)
                foreach (particle part in particles) part.Update();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Anti-aliasing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawString("number of particle's " + particles.Count, new Font("Arial", 10), Brushes.Black, 10,10);

            if (particles.Any())
            {
                lock (particles)
                    foreach (particle part in particles)
                    {
                        if (part.A > 0)
                        {
                            e.Graphics.FillEllipse(part.Brush, (float) part.X, (float) part.Y, 4, 4);
                        }
                    }
                lock (particles)
                    particles.RemoveAll(part => part.A < 0);
            }
            Invalidate();
            Update();
        }

        // Button/key handlers
        private void button1_Click(object sender, EventArgs e)
        {
            spawn = !spawn;
        }
    }
   
    public class particle
    {
        // The particles coordinates
        public double X { get; set; }
        public double Y { get; set; }
        // Alpha value
        public int A { get; set; }
        // Change in direction 
        private readonly double _xd;
        private readonly double _yd;
        // Color of the particle
        public SolidBrush Brush { get; set; }
        private Color _color;

        /// <summary>
        /// Constructor for the particle class
        /// </summary>
        /// <param name="x">The starting position on the x-axis</param>
        /// <param name="y">The starting position on the y-axis</param>
        public particle(int x, int y)
        {
            X = x;
            Y = y;
            A = 255;
            Random rdm = new Random();
            _xd = rdm.Next(50, 150);
            _yd = rdm.Next(0, 400);

            _color = Color.FromArgb(A, rdm.Next(0, 255), rdm.Next(0, 255), rdm.Next(0, 255));
            Brush = new SolidBrush(_color);
        }


        public void Update()
        {
            X += _xd/500;
            Y -= _yd/400;
            A -= 5;
            if (A >= 0)
            Brush = new SolidBrush(Color.FromArgb(A,_color.R,_color.G,_color.B));
        }

    }


}
