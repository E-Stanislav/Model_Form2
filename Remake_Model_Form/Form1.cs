using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _2d_graphics_d;
// для работы с библиотекой FreeGLUT
// для работы с элементом управления SimpleOpenGLControl
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Drawing2D;
using Remake_Model_Form;
using OpenTK.Graphics;
//using degreework;

namespace OpenGLNamespace
{
    public partial class Form1 : Form
    {
        public Simulation simulation;
        public static Form1 form;
        public string type_stress = "cpc";
        Size oldSize;


        public Form1()
        {
            InitializeComponent();
            //MdiParent = parent;
            form = this;
            //AnT.InitializeContexts();
            //simulation = new Simulation(glControl1);
            //oldSize = glControl1.Size;
            // timer1.Interval = 11;
            // timer1.Enabled = true;
            //simulation.setType1(1);
            //simulation.grid_with_force();
        }


        /*public void setPictureBox()
        {
            LinearGradientBrush GBrush = new LinearGradientBrush(new Point(0, 0),
        new Point( 0, pictureBox1.Height), Color.Red, Color.Blue);
            Rectangle rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            // Fill with gradient 
            System.Drawing.Graphics e = pictureBox1.CreateGraphics();
            e.FillRectangle(GBrush, rect);
        }*/

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            simulation.setType1(3);
            simulation.setType2(1);
            glControl1.Invalidate();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
            //simulation.setType1(1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender)
        {
           
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void AnT_KeyPress(object sender)
        {
            //MessageBox.Show(e.KeyChar.ToString());

            //simulation.keyPressed(e);

            /*if (e.KeyChar.ToString() == "w")
            {
                simulation.addScale();
            }
            if (e.KeyChar.ToString() == "s")
            {
                simulation.subScale();
            }*/
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("dsd");
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            simulation.onMouseDown(e);
            glControl1.Invalidate();

        }

        private void AnT_MouseUp(object sender, MouseEventArgs e)
        {
            simulation.onMouseUp();
            glControl1.Invalidate();
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            simulation.onMouseMove(e.X, e.Y);
            glControl1.Invalidate();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
         
        }

        

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
         
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //simulation.setType1(comboBox1.SelectedIndex+1);
        }

        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {
            simulation.keyPressed(e);
            glControl1.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setDrawNumberOfElement(checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setDrawNumberOfNode(checkBox3.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setShowForce(checkBox5.Checked);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setShowBounds(checkBox4.Checked);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setShowBlink(checkBox6.Checked);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

            //groupBox3.Left = Width - 328;
            //AnT.Width = Width - 463;
            glControl1.Width = Width - 50;
            glControl1.Height = Height - 100;
            //AnT.Width = Width;
            //AnT.Height = Height;
            if (simulation.type1 == 2)
            {
                simulation.init2d();
            }
            else
            {
                simulation.init3d();
            }
            glControl1.Invalidate();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setShowZones(checkBox7.Checked);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //simulation.setForceScale(trackBar1.Value/100.0);
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setDrawNumberOfProperty(checkBox8.Checked);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            //simulation.setShowYellow(checkBox9.Checked);
        }

        private void AnT_Resize(object sender, EventArgs e)
        {
            
            simulation.cam.addLen(glControl1.Size.Width-oldSize.Width);
            simulation.cam.addLen(glControl1.Size.Height - oldSize.Height);
            oldSize = glControl1.Size;
            glControl1.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //setPictureBox();

        }

        private void AnT_Paint(object sender, PaintEventArgs e)
        {
            //setPictureBox();
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
           // type_stress = "cpc";
            //simulation.make_stress_for_form(type_stress, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
            //simulation.cm_m_stress_for_form(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
           // type_stress = "cm";
            //simulation.make_stress_for_form(type_stress, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
            //simulation.cps_m_stress_for_form(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));

        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            //type_stress = "ke";
            //simulation.make_stress_for_form(type_stress, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
            //simulation.ke_m_stress_for_form(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Bitmap scr = CopyColorBuffer();
            //Image scr = AnT.BackgroundImage;

            //pictureBox1.Image = scr;
            SaveFileDialog dlg = new SaveFileDialog();
            //if (MainForm.PrScFilepath!="")
            //{
                //dlg.InitialDirectory = MainForm.PrScFilepath;
            //}
            dlg.Filter = "*.png|*.png|*.jpg; *.jpeg|*.jpg;*.jpeg|*.bmp|*.bmp|Все файлы|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //MainForm.PrScFilepath = dlg.InitialDirectory;
                scr.Save(dlg.FileName, ImageFormat.Jpeg);
            }
        }

        public Bitmap CopyColorBuffer()
        {
            glControl1.Invalidate();
            glControl1.Update();
            glControl1.Refresh();
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();
            int w = glControl1.ClientSize.Width;
            int h = glControl1.ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(glControl1.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, w, h, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form2 newform = new Form2();
            button2.Enabled = false;
            newform.Show();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            if (simulation == null)
            {
                simulation = new Simulation(glControl1);
                oldSize = glControl1.Size;
                timer1.Enabled = true;
                simulation.setType1(1);
            }
            simulation.Update();
            simulation.Draw();
            glControl1.Invalidate();
            loaded = true;//<--------------------------------------
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            // Clears the control using the background color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // Swaps front frame with back frame
            simulation.Update();
            simulation.Draw();
            glControl1.SwapBuffers();
            glControl1.Invalidate();
            setText(new StringBuilder()
                .Append(simulation.cam.mPos.x)
                .Append(" ")
                .Append(simulation.cam.mPos.y)
                .Append(" ")
                .Append(simulation.cam.mPos.z)
                .Append(" ")
                .Append(simulation.cam.len)
                .ToString());
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (simulation != null)
                simulation.onMouseUp();
            glControl1.Invalidate();
        }

        private void glControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (simulation != null)
                simulation.onMouseMove(e.X, e.Y);
            glControl1.Invalidate();
        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (simulation != null)
                simulation.onMouseDown(e);
            glControl1.Invalidate();
        }

        public void setText(String text)
        {
            label1.Text = text;
        }

        public void setAnimationSpeed(int speed)
        {
            simulation.updateAnimation(speed);
        }


    }
}
