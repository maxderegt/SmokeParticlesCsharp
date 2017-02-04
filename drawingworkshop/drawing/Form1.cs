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
        private bool _spawn;

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
            // Spawn particles, if the button/switch has been pressed
            if (_spawn)
            {
                lock (Particles)
                    for (var i = 0; i < 1; i++)
                        Particles.Add(new Particle(Width/2, Height/2,10));
            }
            // Update all the particles
            lock (Particles)
                foreach (var part in Particles) part.Update();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // draws the text with the amount of particle's currently visible
            e.Graphics.DrawString("number of particle's " + Particles.Count, new Font("Arial", 10), Brushes.Black, 10,
                10);

            // if the particles list has any particles
            lock (Particles)
            {
                if (Particles.Any())
                {
                    // Draw every particle
                   foreach (var part in Particles)
                    {
                        if (part.A > 0)
                        {
                            e.Graphics.FillEllipse(part.Brush, (float) part.X, (float) part.Y, part.Size, part.Size);
                        }
                    }
                    
                }
            }

            // Update/Refresh the form
            Invalidate();
            Update();
        }

        // Button/key handlers
        private void button1_Click(object sender, EventArgs e)
        {
            _spawn = !_spawn;
        }
    }
   
    public class Particle
    {
        // The particles coordinates
        public double X { get; set; }
        public double Y { get; set; }
        // size of the particle
        public int Size;
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
        /// <param name="size">The size of the particle</param>
        public Particle(int x, int y, int size)
        {
            X = x;
            Y = y;
            A = 255;
            Size = size;
            var rdm = new Random();

            _xd = rdm.Next(100, 500) / 500f + 0.1;
            _yd = rdm.Next(300, 400) / 400f;

            _color = Color.FromArgb(A, rdm.Next(0, 255), rdm.Next(0, 255), rdm.Next(0, 255));
            Brush = new SolidBrush(_color);
        }

        /// <summary>
        /// Update the particle with a new delta x and y.
        /// Also give the particle a new alpha.
        /// </summary>
        public void Update()
        {
            A -= 1;
            if (A >= 0)
            {
                Brush = new SolidBrush(Color.FromArgb(A,_color.R,_color.G,_color.B));
            }
        }

    }


}
