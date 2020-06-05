using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace _2d_graphics_d
{
    // структура хранящая информацию о узле(вершине треугольника)
    public class node
    {
        public Int32 number; //номер элемента
        public Double x;//координата х
        public Double y;//координата у
        public Double forceX;//приложенная сила по оси х
        public Double forceY;//приложенная сила по оси у
        public Byte TypeBound; // тип закрепления
        public Double movX; //перемещение по х
        public Double movY; //перемещение по у
        public Int32 r_number; //номер элемента, который надо отобразить на экране
        public Double stress;
    }


    public class Nodes
    {
        // здесь хранятся все узлы 
        public  List<node> all_nodes = new List<node>();
        public  Int32 count_of_nodes; //число узлов
        //здесь хранятся номера узлов к которым приложены силы
        public  List<int> numbers_f = new List<int>();
        
        //
        // ищем узел по его координатам, возвращаем номер этого узла
        public Int32 findnode(Double x, Double y)
        {
            Int32 i = 0;
            foreach (node n in all_nodes)
            {
                if (n.x == x && n.y == y)
                    break;
                else ++i;
            }
            return i;
        }

        //по номеру r_number(отображаем его на экране) возвращает его системный номер number
        public Int32 find_r_node(Int32 m)
        {
            Int32 i = 0;
            foreach (node n in all_nodes)
            {
                if (n.r_number == m)
                    break;
                else ++i;
            }

            return i;
        }

        //ищет узел по его номеру, возвращает структуру узел; 
        // !!! если использовать этот метод с element.node, то передавать в метод (element.node-1)
        public  node getnode(Int32 i)
        {
            return all_nodes[i];
        }

        public void setStress(int i, Double stress)
        {
            all_nodes[i].stress = stress;
        }
 

    }
}
