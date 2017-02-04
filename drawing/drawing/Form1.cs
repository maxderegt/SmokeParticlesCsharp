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
        private List<Particle> Particles { get; set; } = new List<Particle>();
        private bool _spawn = false;

        public Form1()
        {
            // Initialize the form
            InitializeComponent();

            // Start the main loop timer
            System.Timers.Timer timer = new System.Timers.Timer(1000f/30f);
            timer.Elapsed += Timer_Tick;
            timer.Start();

            // set the maximum particle count
            Particles.Capacity = 10000;
        }

        /// <summary>
        /// Timer which serves as the main loop for the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {

            if (_spawn)
            {
                lock (Particles)
                    for (var i = 0; i < 1; i++)
                        Particles.Add(new Particle(this.Width/2,this.Height/2));
            }
            lock (Particles)
                foreach (var part in Particles) part.Update();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Anti-aliasing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            e.Graphics.DrawString("number of particle's " + Particles.Count, new Font("Arial", 10), Brushes.Black, 10,
                10);

            if (Particles.Any())
            {
                lock (Particles)
                    foreach (var part in Particles)
                    {
                        if (part.A > 0)
                        {
                            e.Graphics.FillEllipse(part.Brush, (float) part.X, (float) part.Y, 4, 4);
                        }
                    }
                lock (Particles)
                    Particles.RemoveAll(part => part.A < 0);
            }
            Invalidate();
            Update();
        }

        // Button/key handlers
        private void button1_Click(object sender, EventArgs e)
        {
            if (_spawn) _spawn = false;
            else _spawn = true;
        }
    }
   
    public class Particle
    {
        // The particles coordinates
        public double X { get; set; }
        public double Y { get; set; }
        // Alpha value
        public int A { get; set; }
        // Change in direction 
        private double _xd;
        private double _yd;
        // Color of the particle
        public SolidBrush Brush { get; set; }
        private Color _color;

        /// <summary>
        /// Constructor for the particle class
        /// </summary>
        /// <param name="x">The starting position on the x-axis</param>
        /// <param name="y">The starting position on the y-axis</param>
        public Particle(int x, int y)
        {
            X = x;
            Y = y;
            A = 255;
            var rdm = new Random();

            _xd = rdm.Next(0, 100);
            _yd = rdm.Next(0, 100);

            _color = Color.FromArgb(A, rdm.Next(0, 255), rdm.Next(0, 255), rdm.Next(0, 255));
            Brush = new SolidBrush(_color);
        }


        public void Update()
        {
            X += _xd/300;
            Y -= _yd/100;
            A -= 5;
            if (A >= 0)
            Brush = new SolidBrush(Color.FromArgb(A,_color.R,_color.G,_color.B));
        }

    }


}
