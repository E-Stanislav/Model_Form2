using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenGLNamespace;

namespace Remake_Model_Form
{
    
    public partial class Form3 : Form
    {
        public static Form3 form3;

        public Form3()
        {
            InitializeComponent();
            form3 = this;
            form3.Text = "Проект" + " " + Form1.form.simulation.s + " " + "Шкала" + " " + Form1.form.simulation.line1;
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show("");
            setPictureBox();
            this.Invalidate();
            //pictureBox1.Invalidate();
            //pictureBox1.Refresh();
            form3.Update();
            //this.Refresh();
        }

        public void setPictureBox()
        {
            LinearGradientBrush GBrush = new LinearGradientBrush(new Point(0, 0),
        new Point(0, pictureBox1.Height), Color.Red, Color.Blue);
            Rectangle rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            // Fill with gradient 
            System.Drawing.Graphics e = pictureBox1.CreateGraphics();
            e.FillRectangle(GBrush, rect);
          
            //pictureBox1.Refresh();
            //pictureBox1.Invalidate();
        }

        private void Form3_VisibleChanged(object sender, EventArgs e)
        {
            //setPictureBox();
           // this.Invalidate();
            //pictureBox1.Invalidate();
            //this.Refresh();

            // 
            //pictureBox1.Refresh();
            //this.Update();
            //setPictureBox();
           //this.Refresh();
            

            //
            //pictureBox1.Invalidate();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            //F
            //Form2.form2.checkBox7.Checked = false;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
           // this.Hide();
            Form2.form2.checkBox7.Checked = false;
        }

        private void Form3_EnabledChanged(object sender, EventArgs e)
        {
            //pictureBox1.Invalidate();
            //setPictureBox();
            //form3.Update();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //setPictureBox();
            //this.Update();
            
        }

        private void Form3_Activated(object sender, EventArgs e)
        {
            //pictureBox1.Invalidate();
            //setPictureBox();
            //form3.Update();
            //pictureBox1.Refresh();
        }

        private void Form3_Enter(object sender, EventArgs e)
        {
            //setPictureBox();
            //form3.Update();
           // pictureBox1.Refresh();
        }

        private void Form3_LocationChanged(object sender, EventArgs e)
        {
           //pictureBox1.Refresh();

        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            //setPictureBox();
            //pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
