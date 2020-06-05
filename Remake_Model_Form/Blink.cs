using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using _2d_graphics_d;

namespace OpenGLNamespace
{
    class Blink
    {
        int counter;
        int time;
        Data data;
        public Blink(Data data)
        {
            counter = 0;
            time = 50;
            this.data = data;
        }

        public void getColor(int elem, ref Color c1, ref Color c2, ref Color  c3)
        {

            if (Form1.form.simulation.colorType != -1)
            {
                Double q = Data.mass_of_material[data.temp_elem.all_elements[elem].material - 1].SG;
                if (data.temp_elem.all_elements[elem].stress[Form1.form.simulation.colorType] > q)
                {
                    if (counter < time)
                    {
                        c1 = Color.Yellow;
                        c2 = Color.Yellow;
                        c3 = Color.Yellow;
                    }
                }
            }
            else
            {
                foreach (Int64 e in data.el_destroy)
                {
                    if (elem == e)
                    {
                        if (counter < time)
                        {
                            c1 = Color.Yellow;
                            c2 = Color.Yellow;
                            c3 = Color.Yellow;
                        }
                        return;
                    }
                }
            }
        }
        public void Update()
        {
            counter++;
            if (counter > 2 * time)
            {
                counter = 0;
            }
        }
    }
}
