        


//считает все типы напряжений в точке. Принимает координаты х и у вершины, результат хранит в массиве st[]
//public void cps_m_stress(double x, double y)
//{
//    Int64 elem;
//    element el;
//    byte[] count=new byte[3];
//    double [] k=new double[3];
//    node[] node_n = new node[3];
//    double[,] strain = new double[3, 6];
//    double rx, ry;
//    byte i, j;
//    double[] st = new double[6];

     
//    elem = data.find_el_by_point(x, y);
//    el = data.find_el_by_number(elem);
//    node_n[0] = data.temp_nodes.getnode(el.node1 - 1);
//    node_n[1] = data.temp_nodes.getnode(el.node2 - 1);
//    node_n[2] = data.temp_nodes.getnode(el.node3 - 1);

//    for (i = 0; i != 3; ++i)
//    {
//        for (j = 0; j != 6;++j )
//        {
//            strain[i, j] = 0;
//            count[i] = 0;
//        }

//    }

//    foreach(element ele in data.temp_elem.all_elements)
//    {
                

//    for (i = 0; i != 3; ++i)
//    {
//        if (ele.node1 == node_n[i].number || ele.node2 == node_n[i].number || ele.node3 == node_n[i].number)
//        {
                  

//        for (j = 0; j != 6; ++j)
//        {
//            strain[i,j]=strain[i,j]+ele.stress[j];
                    
                  
//        }
//        ++count[i];
//        //textBox13.Text = count[0].ToString();
//    }
//        }//if


//    }//fr



//    for (j = 0; j != 6; ++j)
//    {
//        for (i = 0; i != 3; ++i)
//        {
//            strain[i,j]=mydiv(strain[i,j], count[i]);
//        }
//        rx = mydiv(node_n[0].x * strain[0, j] + node_n[1].x * strain[1, j] + node_n[2].x * strain[2, j], strain[0, j] + strain[1, j] + strain[2, j]);
//        ry = mydiv(node_n[0].y * strain[0, j] + node_n[1].y * strain[1, j] + node_n[2].y * strain[2, j], strain[0, j] + strain[1, j] + strain[2, j]);

//        for (i = 0; i != 3; ++i)
//        {
//            k[i] = mydiv(Math.Sqrt((Math.Pow((rx - node_n[i].x), 2) + Math.Pow((ry - node_n[i].y), 2))), Math.Sqrt(Math.Pow((x - node_n[i].x), 2) + Math.Pow((y - node_n[i].y), 2)));
//        }
//        st[j]=mydiv(strain[0, j] * k[0] + strain[1, j] * k[1] + strain[2, j] * k[2], k[0] + k[1] + k[2]);
//    }
             
//}


////вспомогательная функция
//public double mydiv(double x, double y)
//{
//    if (Math.Abs(y) < 0.1E-4900)
//        return 0.1E-4900;
//    else return x / y;
//}