using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2d_graphics_d;


namespace OpenGLNamespace
{
    public class AnimationXY
    {
        int len;
        double currentLen;
        Double[] startX;
        Double[] startY;

        Double []dx;
        Double []dy;

        Data data;

        double dir;
        double usingDir = 1.0 / 11.0;

        public double Dir
        {
            set {
                if (dir != 0)
                {
                    dir = dir > 0 ? value : -value;
                }
                this.usingDir = value;
            }
            get { return this.usingDir; }
        }

        public Data Data
        {
            get => default;
            set
            {
            }
        }

        public AnimationXY(Data data)
        {
            int q = 0;
            len = 100;
            this.data = data;
            startX = new Double[data.temp_nodes.all_nodes.Count];
            startY = new Double[data.temp_nodes.all_nodes.Count];
            dx = new Double[data.temp_nodes.all_nodes.Count];
            dy = new Double[data.temp_nodes.all_nodes.Count];
            for (int i=0; i<data.temp_nodes.all_nodes.Count; i++)
            {
                node n =  data.temp_nodes.all_nodes[i];
                startX[i] = n.x;
                startY[i] = n.y;
                Double k = 300;
                if ((n.TypeBound == 1) || (n.TypeBound == 10) || (n.TypeBound == 11))
                {
                    dx[i] = 0;
                    dy[i] = 0;
                    q++;
                }
                else
                {
                    dx[i] = (n.movX / (double)len) * k;
                    dy[i] = (n.movY / (double)len) * k;
                }
            }

            
        }

        public void init()
        {
            currentLen = 0;
        }

        public void Update()
        {
            if (currentLen < 0)
            {
                currentLen = 0;
                dir = usingDir;
            }
            if (currentLen > len)
            {
                currentLen = len;
                dir = -usingDir;
            }
            for (int i = 0; i < data.temp_nodes.all_nodes.Count; i++)
            {
                node n = data.temp_nodes.all_nodes[i];
                n.x = startX[i] + currentLen * dx[i];
                n.y = startY[i] + currentLen * dy[i];
            }

            currentLen += dir;
        }
        public void Start()
        {
            dir = usingDir;
        }

        public void Stop()
        {
            dir = 0;
            currentLen = 0;
        }
    }
}
