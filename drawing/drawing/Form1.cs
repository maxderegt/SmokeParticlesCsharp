﻿using System;
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
        private List<particle> particles { get; set; } = new List<particle>();
        private List<Keys> pressed = new List<Keys>();
        private bool spawn = false;
        private Player player;
        private SolidBrush brush;

        public Form1()
        {
            InitializeComponent();
            System.Timers.Timer timer = new System.Timers.Timer(1000/30);
            timer.Elapsed += Timer_Tick;
            timer.Start();
            particles.Capacity = 10000;
            player = new Player(10,10);
            Color clr = Color.Red;
            brush = new SolidBrush(clr);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (spawn)
            {
                lock (particles)
                    for (int i = 0; i < 1; i++)
                        particles.Add(new particle((int)player.x,(int)player.y));
            }
            lock (particles)
                foreach (particle part in particles) part.update();
            lock (player)
            {
                foreach (Keys key in pressed)
                {
                    switch (key)
                    {
                        case Keys.W:
                            player.y--;
                            break;
                        case Keys.A:
                            player.x--;
                            break;
                        case Keys.S:
                            player.y++;
                            break;
                        case Keys.D:
                            player.x++;
                            break;
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Anti-aliasing
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //Anti-aliasing for text
            //e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.DrawString(pressed.Count+" number of particle's " + particles.Count, new Font("Arial", 10), Brushes.Black, 10,
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
            lock(player)
                e.Graphics.FillEllipse(brush,(int)(player.x-player.size/2),(int)(player.y-player.size/2),player.size,player.size);

            Invalidate();
            Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (spawn) spawn = false;
            else spawn = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(!pressed.Contains(e.KeyCode))
            pressed.Add(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(pressed.Contains(e.KeyCode))
            pressed.Remove(e.KeyCode);
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
        public double x { get; set; }
        public double y { get; set; }
        public int a { get; set; }
        private double xd;
        private double yd;
        public SolidBrush brush { get; set; }

        private Color color;

        public particle(int x, int y)
        {
           
            this.x = x;
            this.y = y;
            this.a = 255;
            Random rdm = new Random();
            xd = rdm.Next(0, 200) - 100;
            yd = rdm.Next(0, 200) - 100;

            color = Color.FromArgb(a, rdm.Next(0, 255), rdm.Next(0, 255), rdm.Next(0, 255));
            brush = new SolidBrush(color);
        }

        public void update()
        {
            x += xd/500;
            y += yd/500;
            a -= 5;
            if (a >= 0)
            brush = new SolidBrush(Color.FromArgb(a,color.R,color.G,color.B));
        }

    }


}
