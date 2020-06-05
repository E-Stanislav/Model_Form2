using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2d_graphics_d
{
    public struct element
    {
        public Int64 number;//номер элемента, его отображаем 
        public Int32 node1;//номер первого узла треугольника
        public Int32 node2;//второго
        public Int32 node3;//третьего
        public Int32 material;//свойство материала треугольника
        public Double[] stress;// массив, в котором хранятся напряжения треугольника
       
    }

    


    public class Elements
    {
        //хранит все треугольники
        public  List<element> all_elements = new List<element>();
        public  Int64 count_of_elements; //число треугольников


        //возвращает элемент с соответствующим номером
        public  element get_element(Int32 i)
        {
            return all_elements[i];
        }


    }
}
