using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace _2d_graphics_d
{
    //храним материалы
    public struct materials
    {
        public Double E;
        public Double MU;
        public Double SG;
        public Double TS;//толщина пластины
        public Double free4;
        public Double free5;
        public Double free6;
    }

    //хранит всю информацию о пластине
    public class Data
    {
        public static string file_path;
        public static double xxx;
        public Nodes temp_nodes = new Nodes();
        public Elements temp_elem = new Elements();

        //
        //Здесь хранится тип материала для каждого треугольника
        public List<int> lii = new List<int>();
        public List<Int64> el_destroy = new List<Int64>();
        public Double s;

        public Double colorStressMax;
        public Double colorStressMin;

        //public  materials mater;
        public static materials[] mass_of_material = new materials[3];
        public int MaxX;
        public int MaxY;
        public List<node> node_with_fofce = new List<node>();

        //
        public int k; //число зон
        public double[,] point = new double[20, 16]; //точки, из которых состоят зоны, как треугольники.
        //первое измерение массива - число зон, оно соответствует к; остальные пусты. Второе измерение массива - 16, здесь хранятся
        //координаты для 8 точек, из которых состоит массив, т.е. 1 и 2й элемент координаты первой точки, 3,4 - координаты 2й точки

        public double[] mass = new double[50];

        public void openfile_res1()
        {
            double x;
            double y;
            byte z;
            Int32 i;
            node nn;
            Int32 num = 0;


            temp_nodes.count_of_nodes = 0;
            FileInfo f = new FileInfo(file_path + @"\RESULT1.BIN");

            using (BinaryReader readdata = new BinaryReader(f.OpenRead()))
            {
                while (readdata.PeekChar() != -1)
                {
                    x = readdata.ReadDouble();
                    if (x == -1)
                        break;
                    else
                    {
                        y = readdata.ReadDouble();
                        ++temp_nodes.count_of_nodes;
                        node n = new node();
                        n.number = temp_nodes.count_of_nodes;
                        n.x = x;
                        n.y = y;
                        n.forceX = 0;
                        n.forceY = 0;
                        n.movX = 0;
                        n.movY = 0;
                        n.TypeBound = 0;
                        n.r_number = 0;

                        temp_nodes.all_nodes.Add(n);
                        //Data.rb.Text += x;
                        //Data.rb.Text += "\n";
                        //Data.rb.Text += y;
                        //Data.rb.Text += "\n";
                    }

                }

                while (readdata.PeekChar() != -1)
                {

                    x = readdata.ReadDouble();
                    if (x == -1)
                        break;
                    else
                    {
                        //Data.rb.Text += x;
                        //Data.rb.Text += "\n";
                        y = readdata.ReadDouble();
                        i = temp_nodes.findnode(x, y);
                        nn = temp_nodes.getnode(i);
                        //Data.rb.Text += y;
                        //Data.rb.Text += "\n";
                        z = readdata.ReadByte();
                        nn.TypeBound = z;
                        temp_nodes.all_nodes[i] = nn;
                        //Nodes.all_nodes[i].TypeBound = z;
                        //Data.rb.Text += z;
                        //Data.rb.Text += "\n";
                    }
                }



                //Data.rb.Text += "zak";
                //Data.rb.Text += "\n";
                while (readdata.PeekChar() != -1)
                {
                    x = readdata.ReadDouble();
                    if (x == -1)
                        break;
                    else
                    {
                        //Data.rb.Text += x;
                        //Data.rb.Text += "\n";
                        y = readdata.ReadDouble();
                        //Data.rb.Text += y;
                        //Data.rb.Text += "\n";
                        i = temp_nodes.findnode(x, y);
                        nn = temp_nodes.getnode(i);
                        temp_nodes.numbers_f.Add(nn.r_number);
                        x = readdata.ReadDouble();
                        //Data.rb.Text += x;
                        //Data.rb.Text += "\n";
                        nn.forceX = x;
                        y = readdata.ReadDouble();
                        nn.forceY = y;
                        temp_nodes.all_nodes[i] = nn;
                        //Data.rb.Text += y;
                        //Data.rb.Text += "\n";
                    }
                }




                //Data.rb.Text += "zak";
                //Data.rb.Text += "\n";

                while (readdata.PeekChar() != -1)
                {
                    x = readdata.ReadDouble();
                    if (x == -1)
                        break;
                    else
                    {
                        ++num;
                        y = readdata.ReadDouble();
                        i = temp_nodes.findnode(x, y);
                        nn = temp_nodes.getnode(i);
                        nn.r_number = num;
                        temp_nodes.all_nodes[i] = nn;
                        //Data.rb.Text += x;
                        //Data.rb.Text += "\n";
                    }

                }



                //Data.rb.Text += "zak";
                //Data.rb.Text += "\n";
                num = 0;
                while (readdata.PeekChar() != -1)
                {

                    x = readdata.ReadDouble();
                    if (x == -1)
                        break;
                    else
                    {
                        ++num;
                        y = readdata.ReadDouble();
                        i = temp_nodes.find_r_node(num);
                        nn = temp_nodes.getnode(i);
                        nn.movX = x;
                        nn.movY = y;
                        temp_nodes.all_nodes[i] = nn;
                        //Data.rb.Text += x;
                        //Data.rb.Text += "\n";
                    }

                }

            }
            Console.Write(111);

        }



        public void openfile_res2()
        {
            Int32 x;
            Double x1;


            FileInfo f = new FileInfo(file_path + @"\RESULT2.BIN");


            using (BinaryReader readdata = new BinaryReader(f.OpenRead()))
            {
                bool a = true;
                //while (readdata.PeekChar() != -1)
                while (a == true)
                {
                    x = readdata.ReadInt16();
                    /*Data.rb.Text += "\n";
                    Data.rb.Text += "1 point";
                    Data.rb.Text += "\n";*/
                    if (x == -1)
                        break;
                    else
                    {
                        element el;
                        ++temp_elem.count_of_elements;
                        el.number = temp_elem.count_of_elements;

                        el.node1 = x;


                        x = readdata.ReadInt16();
                        el.node2 = x;

                        x = readdata.ReadInt16();
                        el.node3 = x;

                        el.material = 0;
                        el.stress = new Double[7];
                        for (Int32 i = 0; i != 7; ++i)
                        {
                            el.stress[i] = 0;
                        }
                        temp_elem.all_elements.Add(el);

                    }

                }


                /*Data.rb.Text += "\n";
                Data.rb.Text += "1 point";
                Data.rb.Text += "\n";*/

                Int32 n = 0;
                element el1;
                while (readdata.PeekChar() != -1)
                {
                    x1 = readdata.ReadDouble();
                    if (x1 == -1)
                        break;
                    else
                    {
                        el1 = temp_elem.get_element(n);
                        el1.stress[0] = x1;
                        x1 = readdata.ReadDouble();
                        el1.stress[1] = x1;
                        x1 = readdata.ReadDouble();
                        el1.stress[2] = x1;
                        x1 = readdata.ReadDouble();
                        el1.stress[3] = x1;
                        x1 = readdata.ReadDouble();
                        el1.stress[4] = x1;
                        x1 = readdata.ReadDouble();
                        el1.stress[5] = x1;
                        x1 = readdata.ReadDouble();
                        el1.stress[6] = x1;
                        temp_elem.all_elements[n] = el1;
                        ++n;

                    }
                }

                n = 0;
                while (readdata.PeekChar() != -1)
                {

                    x1 = readdata.ReadDouble();
                    if (x1 == -1)
                        break;
                    else
                    {
                        mass_of_material[n].E = x1;

                        /*Data.rb.Text += x1;
                        Data.rb.Text += "\n";*/
                        x1 = readdata.ReadDouble();
                        mass_of_material[n].MU = x1;
                        //Data.rb.Text += x1;
                        //Data.rb.Text += "\n";
                        x1 = readdata.ReadDouble();
                        mass_of_material[n].SG = x1;
                        //Data.rb.Text += x1;
                        //Data.rb.Text += "\n";
                        x1 = readdata.ReadDouble();
                        mass_of_material[n].free4 = x1;
                        //Data.rb.Text += x1;
                        //Data.rb.Text += "\n";
                        x1 = readdata.ReadDouble();
                        mass_of_material[n].free5 = x1;
                        //Data.rb.Text += x1;
                        //Data.rb.Text += "\n";
                        x1 = readdata.ReadDouble();
                        mass_of_material[n].free6 = x1;
                        //Data.rb.Text += x1;
                        //Data.rb.Text += "\n";
                        x1 = readdata.ReadDouble();
                        mass_of_material[n].TS = x1;

                        //Data.rb.Text += x1;
                        //Data.rb.Text += "\n";
                        //Data.rb.Text += "\n";
                        //Data.rb.Text += "\n";

                        //Data.rb.Text += n;
                        //Data.rb.Text += "\n";
                        ++n;
                    }

                }

                //double x2;

                //while (readdata.PeekChar() != -1)
                //{
                n = 0;

                for (int i = 0; i < temp_elem.count_of_elements; ++i)
                {
                    //n = 0;
                    x = readdata.ReadInt16();
                    //lii.Add(x);
                    //temp_elem.all_elements[i].material = x;
                    //if (x == -1)
                    //break;
                    //else
                    //{
                    el1 = temp_elem.get_element(n);
                    el1.material = x;
                    temp_elem.all_elements[n] = el1;

                    ++n;
                }
                //}
                //}


                //zone
                //int k;
                //double x_x;
                n = 0;
                //while (readdata.PeekChar() != -1)
                //{
                //int ii = 0;
                x = readdata.ReadInt16();
                x = readdata.ReadInt16();
                k = x;
                x = readdata.ReadInt16();
                for (int i = 0; i < k; ++i)
                {
                    for (int j = 0; j < 16; ++j)
                    {
                        x1 = readdata.ReadDouble();
                        point[i, j] = x1;

                    }
                }
                //mass[ii] = x;
                //++ii;

                //if (x == -1)
                //  break;

                //}


                /*while (readdata.PeekChar() != -1)
                {
                    x1 = readdata.ReadDouble();
                    if (x1 == -1)
                        x = readdata.ReadInt16();
                        break;
                
                }*/
                //x = readdata.ReadInt16();
                //zones.k = x;
                /*while (readdata.PeekChar() != -1)
                {
                    

                    x1 = readdata.ReadDouble();
                   
                  
                    if (x1 == -1)
                        break;

                }
                while (readdata.PeekChar() != -1)
                {
                   // int i = 0;
                    //mass[i] = x1;
                    //++i;
                    x = readdata.ReadInt16();
                    if (x == -1)
                        break;
                    //zones.k = x;

                }

                while (readdata.PeekChar() != -1)
                {

                    x = readdata.ReadInt16();
                    //zones.k = x;
                    if (x == -1)
                       
                        break;
                    //zones.k = x;

                }*/
                //x = readdata.ReadInt16();

                //zones.k = x;
                //



            } //using


        }



        public void min_max_size_img()
        {
            double m_x = 0;
            double m_y = 0;
            foreach (node n in temp_nodes.all_nodes)
            {
                if (n.x > m_x)
                    m_x = n.x;
                if (n.y > m_y)
                    m_y = n.y;
            }
            MaxX = (int)m_x;
            MaxY = (int)m_y;
        }


        public void find_nodes_with_force()
        {
            foreach (node n in temp_nodes.all_nodes)
            {
                if (n.forceX != 0 || n.forceY != 0)
                {//
                    node_with_fofce.Add(n);
                }
            }

        }


        public Int64 find_el_by_point(double x, double y)
        {
            foreach (element el in temp_elem.all_elements)
            {
                node n1 = temp_nodes.getnode(el.node1 - 1);
                node n2 = temp_nodes.getnode(el.node2 - 1);
                node n3 = temp_nodes.getnode(el.node3 - 1);


                /* int a = (x[1] - x[0]) * (y[2] - y[1]) - (x[2] - x[1]) * (y[1] - y[0]);
            int b = (x[2] - x[0]) * (y[3] - y[2]) - (x[3] - x[2]) * (y[2] - y[0]);
            int c = (x[3] - x[0]) * (y[1] - y[3]) - (x[1] - x[3]) * (y[3] - y[0]);*/

                double a = (n1.x - x) * (n2.y - n1.y) - (n2.x - n1.x) * (n1.y - y);
                double b = (n2.x - x) * (n3.y - n2.y) - (n3.x - n2.x) * (n2.y - y);
                double c = (n3.x - x) * (n1.y - n3.y) - (n1.x - n3.x) * (n3.y - y);

                if ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0))
                {
                    return el.number;
                }


            }
            return 0;

        }//m

        public element find_el_by_number(double num)
        {
            foreach (element el in temp_elem.all_elements)
            {
                if (el.number == num)
                    return el;
            }
            return temp_elem.all_elements[0];

        }


        public void element_destroy()
        {
            //el_destroy.Add(5);
            foreach (element el in temp_elem.all_elements)
            {
                /*if (el.number == 1715)
                {
                    MessageBox.Show(el.stress[0].ToString());
                }*/
                for (int i = 0; i < 7; ++i)
                {
                    if (el.stress[i] > mass_of_material[el.material - 1].SG)
                    {
                        el_destroy.Add(el.number);
                        break;
                    }
                }
            }
        }




        public void stress_in_node(element elem, int j)
        {


            element el;

            node[] node_n = new node[3];
            byte[] count = new byte[3];
            double[] strain = new double[3];
            int i, z;
            double rx, ry;
            double x_opt, y_opt;
            double x1, y1;
            double[] k = new double[3];
            double[] stress = new double[3];

            for (i = 0; i < 3; ++i)
            {
                //textBox20.Text = i.ToString();
                strain[i] = 0;
                count[i] = 0;
            }


            node_n[0] = temp_nodes.getnode(elem.node1 - 1);
            node_n[1] = temp_nodes.getnode(elem.node2 - 1);
            node_n[2] = temp_nodes.getnode(elem.node3 - 1);

            for (z = 0; z < temp_elem.count_of_elements; ++z)
            {
                el = find_el_by_number(z);
                for (i = 0; i < 3; ++i)
                {
                    if (el.node1 == node_n[i].number || el.node2 == node_n[i].number || el.node3 == node_n[i].number)
                    {
                        strain[i] = strain[i] + el.stress[j];
                        ++count[i];
                    }
                }
            }//for


            for (i = 0; i < 3; ++i)
            {
                strain[i] = mydiv(strain[i], count[i]);
            }

            rx = mydiv(node_n[0].x * strain[0] + node_n[1].x * strain[1] + node_n[2].x * strain[2], strain[0] + strain[1] + strain[2]);
            ry = mydiv(node_n[0].y * strain[0] + node_n[1].y * strain[1] + node_n[2].y * strain[2], strain[0] + strain[1] + strain[2]);

            x_opt = (node_n[0].x + node_n[1].x + node_n[2].x) / 3;
            y_opt = (node_n[0].y + node_n[1].y + node_n[2].y) / 3;

            for (z = 0; z < 3; ++z)
            {
                x1 = node_n[z].x + (x_opt - node_n[z].x) / 10000;
                y1 = node_n[z].y + (y_opt - node_n[z].y) / 10000;
                for (i = 0; i < 3; ++i)
                {
                    k[i] = mydiv(Math.Sqrt((Math.Pow((rx - node_n[i].x), 2) + Math.Pow((ry - node_n[i].y), 2))), Math.Sqrt(Math.Pow((x1 - node_n[i].x), 2) + Math.Pow((y1 - node_n[i].y), 2)));
                }
                stress[z] = mydiv(strain[0] * k[0] + strain[1] * k[1] + strain[2] * k[2], k[0] + k[1] + k[2]);

            }

                node n = temp_nodes.all_nodes[elem.node1 - 1];
                n.stress = stress[0];
                 n = temp_nodes.all_nodes[elem.node2 - 1];
                n.stress = stress[1];
                 n = temp_nodes.all_nodes[elem.node3 - 1];
                n.stress = stress[2];

        }


        //вспомогательная функция
        public double mydiv(double x, double y)
        {
            if (Math.Abs(y) < 0.1E-4900)
                return 0.1E-4900;
            else return x / y;
        }


        public void calculateS()
        {
            s = 0;
            for (int i = 0; i < temp_elem.all_elements.Count; i++)
            //for (int i = 0; i < 1; i++)
            {
                node n1 = temp_nodes.getnode(temp_elem.get_element(i).node1 - 1);
                node n2 = temp_nodes.getnode(temp_elem.get_element(i).node2 - 1);
                node n3 = temp_nodes.getnode(temp_elem.get_element(i).node3 - 1);

                s += geron(n1, n2, n3);
            }
        }

        public Double geron(node n1, node n2, node n3)
        {
            Double a = Math.Sqrt((n1.x - n2.x) * (n1.x - n2.x) + (n1.y - n2.y) * (n1.y - n2.y));
            Double b = Math.Sqrt((n3.x - n2.x) * (n3.x - n2.x) + (n3.y - n2.y) * (n3.y - n2.y));
            Double c = Math.Sqrt((n1.x - n3.x) * (n1.x - n3.x) + (n1.y - n3.y) * (n1.y - n3.y));

            double p = (a + b + c) / 2;

            return Math.Sqrt(p*(p-a)*(p-b)*(p-c)) ;
        }

        public void new_find_stress_in_el(int j)
        {

            foreach (element el in temp_elem.all_elements)
            {
                node[] node_n = new node[3];
                byte[] count = new byte[3];
                double[] strain = new double[3];
                int i, z;
                double rx, ry;
                double x_opt, y_opt;
                double x1, y1;
                double[] k = new double[3];
                double[] stress = new double[3];

                for (i = 0; i < 3; ++i)
                {
                    //textBox20.Text = i.ToString();
                    strain[i] = 0;
                    count[i] = 0;
                }


                node_n[0] = temp_nodes.getnode(el.node1 - 1);
                node_n[1] = temp_nodes.getnode(el.node2 - 1);
                node_n[2] = temp_nodes.getnode(el.node3 - 1);


                foreach (element elem in temp_elem.all_elements)
                {

                    for (i = 0; i < 3; ++i)
                    {
                        if (elem.node1 == node_n[i].number || elem.node2 == node_n[i].number || elem.node3 == node_n[i].number)
                        {
                            strain[i] = strain[i] + elem.stress[j];
                            ++count[i];
                        }
                    }
                }//for

                for (i = 0; i < 3; ++i)
                {
                    strain[i] = mydiv(strain[i], count[i]);
                }

                rx = mydiv(node_n[0].x * strain[0] + node_n[1].x * strain[1] + node_n[2].x * strain[2], strain[0] + strain[1] + strain[2]);
                ry = mydiv(node_n[0].y * strain[0] + node_n[1].y * strain[1] + node_n[2].y * strain[2], strain[0] + strain[1] + strain[2]);

                x_opt = (node_n[0].x + node_n[1].x + node_n[2].x) / 3;
                y_opt = (node_n[0].y + node_n[1].y + node_n[2].y) / 3;

                for (z = 0; z < 3; ++z)
                {
                    x1 = node_n[z].x + (x_opt - node_n[z].x) / 10000;
                    y1 = node_n[z].y + (y_opt - node_n[z].y) / 10000;
                    for (i = 0; i < 3; ++i)
                    {
                        k[i] = mydiv(Math.Sqrt((Math.Pow((rx - node_n[i].x), 2) + Math.Pow((ry - node_n[i].y), 2))), Math.Sqrt(Math.Pow((x1 - node_n[i].x), 2) + Math.Pow((y1 - node_n[i].y), 2)));
                    }
                    stress[z] = mydiv(strain[0] * k[0] + strain[1] * k[1] + strain[2] * k[2], k[0] + k[1] + k[2]);

                }


                if (el.number==1173)
                {
                    int q=0;
                    q++;

                }

                node n = temp_nodes.all_nodes[el.node1 - 1];
                n.stress = stress[0];
                n = temp_nodes.all_nodes[el.node2 - 1];
                n.stress = stress[1];
                n = temp_nodes.all_nodes[el.node3 - 1];
                n.stress = stress[2];
                // здесь что-то делаем с напряжениями для узлов треугольника пока не затерли
            }

        }


        public void getColorStress(int type)
        {
            //MessageBox.Show(temp_elem.all_elements.Count.ToString());
            /*foreach (element elem in temp_elem.all_elements)
            {
                stress_in_node(elem, type);
            }*/
            new_find_stress_in_el(type);
            /*foreach (element elem in temp_elem.all_elements)
            {
                node n = temp_nodes.all_nodes[elem.node1 - 1];
                n.stress = elem.stress[type];
                n = temp_nodes.all_nodes[elem.node2 - 1];
                n.stress = elem.stress[type];
                n = temp_nodes.all_nodes[elem.node3 - 1];
                n.stress = elem.stress[type];
            }*/

            colorStressMax = temp_nodes.all_nodes[0].stress;
            colorStressMin = temp_nodes.all_nodes[0].stress;

            for (int i = 1; i < temp_nodes.all_nodes.Count; i++)
            {
                if (temp_nodes.all_nodes[i].stress > colorStressMax)
                {
                    colorStressMax = temp_nodes.all_nodes[i].stress;
                }

                if (temp_nodes.all_nodes[i].stress < colorStressMin)
                {
                    colorStressMin = temp_nodes.all_nodes[i].stress;
                }

            }
            
        }

    }
}
