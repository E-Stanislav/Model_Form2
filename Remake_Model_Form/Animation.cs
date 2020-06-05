using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2d_graphics_d;

namespace OpenGLNamespace
{
    class Animation
    {
        int len;
        double currentLen;
        Simulation simulation;
        Double[,] start;
        Double[,] d;
        double dir = 1.0 / 11.0;
        public double Dir
        {
            set { this.dir = value; }
            get { return this.dir; }
        }

        public Animation(Simulation simulation)
        {
            len = 100;
            this.simulation = simulation;
            currentLen = len+1;
        }

        public void init(int type1, int type2)
        {
            start = new Double[simulation.elements.all_elements.Count, simulation.nodes.all_nodes.Count];
            d = new Double[simulation.elements.all_elements.Count, simulation.nodes.all_nodes.Count];
           
            Double h1=0, h2=0, h3=0;
            for (int i = 0; i < simulation.elements.all_elements.Count; i++)
            {
                node n1 = simulation.nodes.getnode(simulation.elements.get_element(i).node1 - 1);
                node n2 = simulation.nodes.getnode(simulation.elements.get_element(i).node2 - 1);
                node n3 = simulation.nodes.getnode(simulation.elements.get_element(i).node3 - 1);

                simulation.getH(i, ref h1, ref h2, ref h3);

               
                start[i, simulation.elements.get_element(i).node1 - 1] = h1;
                start[i, simulation.elements.get_element(i).node2 - 1] = h2;
                start[i, simulation.elements.get_element(i).node3 - 1] = h3;
                
            }

            simulation.type1 = type1;
            simulation.type2 = type2;


            for (int i = 0; i < simulation.elements.all_elements.Count; i++)
            {
                node n1 = simulation.nodes.getnode(simulation.elements.get_element(i).node1 - 1);
                node n2 = simulation.nodes.getnode(simulation.elements.get_element(i).node2 - 1);
                node n3 = simulation.nodes.getnode(simulation.elements.get_element(i).node3 - 1);

                simulation.getH(i, ref h1, ref h2, ref h3);


                    d[i, simulation.elements.get_element(i).node1 - 1] = (h1 - start[i, simulation.elements.get_element(i).node1 - 1]) / len;


                    d[i, simulation.elements.get_element(i).node2 - 1] = (h2 - start[i, simulation.elements.get_element(i).node2 - 1]) / len;


                    d[i, simulation.elements.get_element(i).node3 - 1] = (h3 - start[i, simulation.elements.get_element(i).node3 - 1]) / len;


            }
            currentLen = 0;
        }

        public void Update(int i, ref Double h1, ref Double h2, ref Double h3)
        {
            
            if (currentLen > len)
            {
                return;
            }

            
                h1 = start[i, simulation.elements.get_element(i).node1 - 1] + d[i, simulation.elements.get_element(i).node1 - 1] * currentLen;
            
            
                h2 = start[i, simulation.elements.get_element(i).node2 - 1] + d[i, simulation.elements.get_element(i).node2 - 1] * currentLen;
            
            
                h3 = start[i, simulation.elements.get_element(i).node3 - 1] + d[i, simulation.elements.get_element(i).node3 - 1] * currentLen;
            



        }

        public void updateLen()
        {
            if (currentLen > len)
            {
                return;
            }
            if (currentLen < len)
            {
                currentLen += dir;
                if (currentLen > len)
                {
                    currentLen = len;
                }
            }

        }

    }
}
