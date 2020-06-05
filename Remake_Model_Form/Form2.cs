using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenGLNamespace;

namespace Remake_Model_Form
{
    public partial class Form2 : Form
    {
        public static Form2 form2;
        public Form3 fom3 ;
        public Form2()
        {
            InitializeComponent();
            form2 =this;
            form2.Text = "Проект" + " " + Form1.form.simulation.s + " " + "Панель инструментов" + " " + Form1.form.simulation.line1;
            fom3 = new Form3();
            Form1.form.simulation.grid_with_force();
            setControlsEnabled(tabControl1.TabPages[1].Controls, false);
            setControlsEnabled(tabControl1.TabPages[2].Controls, false);

            //fom3 = new Form3();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setType1(comboBox1.SelectedIndex + 1);
            if (comboBox1.SelectedIndex == 0)
            {
                //tabControl1.TabPages[0].;
                tabControl1.SelectedIndex = 0;
                setControlsEnabled(tabControl1.TabPages[0].Controls, true);
                
                setControlsEnabled(tabControl1.TabPages[1].Controls, false);
                setControlsEnabled(tabControl1.TabPages[2].Controls, false);
            }
            else
            {
                tabControl1.SelectedIndex = 1;
                setControlsEnabled(tabControl1.TabPages[0].Controls, false);

                setControlsEnabled(tabControl1.TabPages[1].Controls, true);
                setControlsEnabled(tabControl1.TabPages[2].Controls, true);
            }
            //setControlsEnabled(tabControl1.TabPages[1].Controls, false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setShowZones(checkBox1.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setShowBounds(checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
                if (checkBox3.Checked)
            {
                Form1.form.simulation.startAnimationXY();
            }
            else
            {
                Form1.form.simulation.endAnimationXY();
                //simulation.startAnimationXY(-1);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setShowForce(checkBox4.Checked);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Form1.form.simulation.setForceScale(trackBar1.Value / 100.0);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton1.Checked)
            {

                Form1.form.simulation.getColorStress(0);
            }*/
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton2.Checked)
            {
                Form1.form.simulation.getColorStress(1);
            }*/
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton3.Checked)
            {
                Form1.form.simulation.getColorStress(2);
            }*/
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton4.Checked)
            {
                Form1.form.simulation.getColorStress(3);
            }*/
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton5.Checked)
            {
                Form1.form.simulation.getColorStress(4);
            }*/
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton6.Checked)
            {
                Form1.form.simulation.getColorStress(5);
            }*/
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            /*if (radioButton7.Checked)
            {
                Form1.form.simulation.getColorStress(6);
            }*/
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setShowYellow(checkBox6.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setShowBlink(checkBox5.Checked);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox7.Checked)
            {
                //fom3.Invalidate()
                //
                fom3.Show();
            }
            else
            {
                fom3.Hide();
            }

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(1);
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(2);
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(3);
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(4);
            }
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(5);
            }
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(6);
            }
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(7);
            }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton15.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(8);
            }
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton16.Checked == true)
            {
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(9);
            }
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
        
            if (radioButton17.Checked==true)
            {
           
            //radioButton17.Checked = true;

            Form1.form.simulation.getColorStress(-1);
            Form1.form.simulation.setType2(10);
            radioButton8.Checked = false;
            radioButton9.Checked = false;
            radioButton10.Checked = false;
            radioButton11.Checked = false;
            radioButton12.Checked = false;
            radioButton13.Checked = false;
            radioButton14.Checked = false;
            radioButton15.Checked = false;
            radioButton16.Checked = false;
            }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton18.Checked == true)
            {
                

                Form1.form.simulation.getColorStress(-1);
                Form1.form.simulation.setType2(11);
                radioButton8.Checked = false;
                radioButton9.Checked = false;
                radioButton10.Checked = false;
                radioButton11.Checked = false;
                radioButton12.Checked = false;
                radioButton13.Checked = false;
                radioButton14.Checked = false;
                radioButton15.Checked = false;
                radioButton16.Checked = false;
            }
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.type_stress = "cpc";
            Form1.form.simulation.make_stress_for_form(Form1.form.type_stress, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.type_stress = "cm";
            Form1.form.simulation.make_stress_for_form(Form1.form.type_stress, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.type_stress = "ke";
            Form1.form.simulation.make_stress_for_form(Form1.form.type_stress, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setDrawNumberOfNode(checkBox8.Checked);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setDrawNumberOfElement(checkBox9.Checked);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            Form1.form.simulation.setDrawNumberOfProperty(checkBox10.Checked);
        }

        private void setControlsEnabled(Control.ControlCollection controlCollection, bool enable)
        {
            foreach (Control currControl in controlCollection)
            {
                currControl.Enabled = enable;
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.form.button2.Enabled = true;
            fom3.Hide();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            //checkBox7.Enabled = true;
           // checkBox7.Enabled = false;
           
            if (checkBox11.Checked == true)
            {
                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
                checkBox16.Checked = false;
                checkBox17.Checked = false;

                Form1.form.simulation.getColorStress(0);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
           
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            //Form1.form.simulation.getColorStress(1);
            //if (checkBox11.Checked == true)
               // checkBox11.Checked = false;
           

            if (checkBox12.Checked == true)
            {
                checkBox11.Checked = false;
                //checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
                checkBox16.Checked = false;
                checkBox17.Checked = false;
                Form1.form.simulation.getColorStress(1);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            

            if (checkBox13.Checked == true)
            {
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                //checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
                checkBox16.Checked = false;
                checkBox17.Checked = false;
                Form1.form.simulation.getColorStress(2);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            

            if (checkBox14.Checked == true)
            {
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
                //checkBox14.Checked = false;
                checkBox15.Checked = false;
                checkBox16.Checked = false;
                checkBox17.Checked = false;
                Form1.form.simulation.getColorStress(3);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox15.Checked == true)
            {
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                //checkBox15.Checked = false;
                checkBox16.Checked = false;
                checkBox17.Checked = false;

                Form1.form.simulation.getColorStress(4);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            

            if (checkBox16.Checked == true)
            {
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
                //checkBox16.Checked = false;
                checkBox17.Checked = false;
                Form1.form.simulation.getColorStress(5);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            
            //checkBox17.Checked = false;

            if (checkBox17.Checked == true)
            {
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
                checkBox16.Checked = false;
                Form1.form.simulation.getColorStress(6);
                checkBox7.Enabled = true;
            }
            else
            {
                Form1.form.simulation.getColorStress(-1);
                checkBox7.Enabled = false;
                if (checkBox11.Checked == false && checkBox12.Checked == false && checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false)
                checkBox7.Checked = false;
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (textBox17.Text == "")
            {
                Form1.form.simulation.setZeroZone(0);
                return;
            }

            try
            {
                Double r = Convert.ToDouble(textBox17.Text);
                if ((r >= 0) && (r <= 100))
                {
                    Form1.form.simulation.setZeroZone(r);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Form1.form.setAnimationSpeed(trackBar2.Value);
        }

        [STAThread]
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(() => {
                    if (colorDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Form1.form.simulation.BackgroundColor = colorDialog1.Color;
                    }
                    // /OpenFileDialog dlg = new OpenFileDialog();
                    // The following would not return the dialog if the current
                    // thread is not STA
                    // var result = dlg.ShowDialog();
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.Out.WriteLine(ex.Message);
                Console.Out.WriteLine(ex.StackTrace);
            }
            //MessageBox.Show("1212", "124321", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // colorDialog1.Color = Form1.form.simulation.BackgroundColor;
            // colorDialog1.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            // Form1.form.simulation.BackgroundColor = colorDialog1.Color;
            //}
        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //Form1.form.simulation.setShowZones(checkBox1.Checked);
        //}
    }
}
