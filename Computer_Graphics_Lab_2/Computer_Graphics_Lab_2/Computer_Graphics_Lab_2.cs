using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Tomogram_visualizer
{
    public partial class Computer_Graphics_Lab_2 : Form
    {
        Bin bin = new Bin();
        View view = new View();
        bool loaded = false;
        int currentLayer = 0;
        //---
        bool needReload = false;

        public Computer_Graphics_Lab_2()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) 
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl1.Width, glControl1.Height);
                trackBar1.Maximum = Bin.Z - 1;
                loaded = true;
                glControl1.Invalidate();
            }
        }

        [Obsolete]
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (radioButton1.Checked)
                    view.DrawQuads(currentLayer);
                else
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }



        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        int FrameCount = 0;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        void displayFPS()
        {
            if (DateTime.Now > NextFPSUpdate) 
            {
                this.Text = String.Format("CT Visualizer (fps = {0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            view.Min = trackBar2.Value;
            needReload = true;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            view.Win = trackBar3.Value;
            needReload = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
