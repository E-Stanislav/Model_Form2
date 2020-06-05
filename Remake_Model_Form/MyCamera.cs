using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Tao.OpenGl;
// для работы с библиотекой FreeGLUT
//using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl
//using Tao.Platform.Windows;
namespace OpenGLNamespace
{
    public class MyCamera
    {
        double a1;
        double a2;
        public double len;
        public Vector3D mPos;
        Vector3D mView;
        Vector3D mUp;

        public bool mouseRotate;
        public bool mouseMove;
        public Double myMouseYcoordVar;
        public Double myMouseYcoord;

        public Double myMouseXcoordVar;
        public Double myMouseXcoord;
        public MyCamera()
        {
            a1 = 1;
            a2 = 1;
            len = 0.25;

            mView.x= -1;
            mView.y = 0;
            mView.z = 0;

            mUp.x = 0;
            mUp.y = 1;
            mUp.z = 0;
            
        }

        public void setLookAt(float x, float y)
        {
            mView.x = x / 3.0f - 1;
            mView.y = 0;
            mView.z = y / 3.0f;
        }

        public void Look()
        {
            // Glu.gluLookAt(mPos.x, mPos.y, mPos.z, //Нами ранее обсуждаемая команда =)
            //               mView.x, mView.y, mView.z,
            //               mUp.x, mUp.y, mUp.z);

            Matrix4 p = Matrix4.LookAt(mPos.x, mPos.y, mPos.z, mView.x, mView.y, mView.z, mUp.x, mUp.y, mUp.z);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
            /*Vector3D forward = new Vector3D();
            Vector3D side = new Vector3D();
            Vector3D up = new Vector3D();
            forward.x = mView.x - mPos.x;
            forward.y = mView.y - mPos.y;
            forward.z = mView.z - mPos.z;

            side.x = forward.x * mUp.x;
            side.y = forward.y * mUp.y;
            side.z = forward.z * mUp.z;

            var max = Math.Max(side.x, Math.Max(side.y, side.z));
            side.x = side.x / max;
            side.y = side.y / max;
            side.z = side.z / max;

            up.x = side.x * forward.x;
            up.y = side.y * forward.y;
            up.z = side.z * forward.z;

            double[] d =
            {
                side.x, up.x, - forward.x,
                side.y, up.y, - forward.y,
                side.z, up.z, - forward.z,
            };

            GL.MultMatrix(d);
            GL.Translate(-mPos.x, -mPos.y, -mPos.z);*/
        }

        public void update()
        {
            mPos.x = mView.x + (float)(len * Math.Cos(a2) * Math.Sin(a1));
            mPos.y = mView.y + (float)(len * Math.Cos(a2) * Math.Cos(a1));
            mPos.z = mView.z + (float)(len * Math.Sin(a2)) ;
        }

        public void mouse_Events(GLControl AnT)
        {
            if (mouseRotate == true) //Если нажата левая кнопка мыши
            {
                a1 += (myMouseXcoordVar - myMouseXcoord) / 100.0;
                a2 += (myMouseYcoordVar - myMouseYcoord) / 100.0;
            }
            else
            {
                if (mouseMove)
                {
                    len += (myMouseXcoordVar - myMouseXcoord) / 20.0;
                }
            }

            myMouseXcoord = myMouseXcoordVar;
            myMouseYcoord = myMouseYcoordVar;
        }

        public void addLen(double d)
        {
            len += d;
        }
    }
}
