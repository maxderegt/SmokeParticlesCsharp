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
<<<<<<< HEAD

            // Initialize the player(particle origin)
            player = new Player(10,10);

            Color clr = Color.Red;
            brush = new SolidBrush(clr);
=======
>>>>>>> 34828f31929b78bdaf136e0a4907e977b19d5883
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
                    for (int i = 0; i < 1; i++)
                        particles.Add(new particle(this.Width/2,this.Height/2));
            }
            lock (particles)
                foreach (particle part in particles) part.update();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Anti-aliasing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
<<<<<<< HEAD

            e.Graphics.DrawString(pressed.Count+" number of particle's " + particles.Count, new Font("Arial", 10), Brushes.Black, 10,
=======
            e.Graphics.DrawString("number of particle's " + particles.Count, new Font("Arial", 10), Brushes.Black, 10,
>>>>>>> 34828f31929b78bdaf136e0a4907e977b19d5883
                10);

            if (particles.Count() > 0)
            {
                lock (particles)
                    foreach (particle part in particles)
                    {
                        if (part.a > 0)
                        {
                            e.Graphics.FillEllipse(part.brush, (float) part.x, (float) part.y, 4, 4);
                        }
                    }
                lock (particles)
                    particles.RemoveAll(part => part.a < 0);
            }
            Invalidate();
            Update();
        }

        // Button/key handlers
        private void button1_Click(object sender, EventArgs e)
        {
            if (spawn) spawn = false;
            else spawn = true;
        }
    }

    public class Player
    {
        public double x { get; set; } = 0;
        public double y { get; set; } = 0;
        public int size { get; set; } = 16;

        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

   
    public class particle
    {
        // The particles coordinates
        public double x { get; set; }
        public double y { get; set; }
        // Alpha value
        public int a { get; set; }
        // Change in direction 
        private double xd;
        private double yd;
        // Color of the particle
        public SolidBrush brush { get; set; }
        private Color color;

        /// <summary>
        /// Constructor for the particle class
        /// </summary>
        /// <param name="x">The starting position on the x-axis</param>
        /// <param name="y">The starting position on the y-axis</param>
        public particle(int x, int y)
        {
           
            this.x = x;
            this.y = y;
            this.a = 255;
            Random rdm = new Random();
            xd = rdm.Next(0, 100);
            yd = rdm.Next(0, 100);

            color = Color.FromArgb(a, rdm.Next(0, 255), rdm.Next(0, 255), rdm.Next(0, 255));
            brush = new SolidBrush(color);
        }


        public void update()
        {
            x += xd/300;
            y -= yd/100;
            a -= 5;
            if (a >= 0)
            brush = new SolidBrush(Color.FromArgb(a,color.R,color.G,color.B));
        }

    }


}
