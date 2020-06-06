using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public double a1;
        public double a2;
        public double len;
        public Vector3 mPos;
        Vector3 mView;
        Vector3 mUp;

        public Matrix4 matrix;

        public bool mouseRotate;
        public bool mouseMove;
        public Double myMouseYcoordVar;
        public Double myMouseYcoord;

        public Double myMouseXcoordVar;
        public Double myMouseXcoord;
        public MyCamera()
        {
            a1 = 3;
            a2 = -0.5;
            len = 0.5;

            mView.X = 0;
            mView.Y = 0;
            mView.Z = 0;

            mUp = Vector3.UnitY;            
        }

        public void setLookAt(float x, float y)
        {
            /*Vector4 v = new Vector4();
            v.X = y;
            v.Z = x;
            v.W = 1.0f;
            Matrix4 modelMatrix;
            Console.WriteLine(v.X + " " + v.Z);

            GL.GetFloat(GetPName.ModelviewMatrix, out modelMatrix);
            v *= modelMatrix;
            Console.WriteLine(modelMatrix.M11 + " " + modelMatrix.M12 + " " + modelMatrix.M13 + " " + modelMatrix.M14);
            Console.WriteLine(modelMatrix.M21 + " " + modelMatrix.M22 + " " + modelMatrix.M23 + " " + modelMatrix.M24);
            Console.WriteLine(modelMatrix.M31 + " " + modelMatrix.M32 + " " + modelMatrix.M33 + " " + modelMatrix.M34);
            Console.WriteLine(modelMatrix.M41 + " " + modelMatrix.M42 + " " + modelMatrix.M43 + " " + modelMatrix.M44);
            Console.WriteLine(v.X + " " + v.Z);
            v *= matrix;
            Console.WriteLine(v.X + " " + v.Z + "     " + v.W);
            //Vector4.Transform(ref v, ref matrix, out v);
            if (v.W > 0.000001f || v.W < -0.000001f)
            {
                v.X *= v.W;
                v.Y *= v.W;
                v.Z *= v.W;
            }
            mView = v.Xyz;*/


            // Console.WriteLine("View before: " + x + " " + y);
            mView.X = y;// -y;
            mView.Y = 0;
            mView.Z = x;// x;


            //mView.Normalize();
            mView.X *= 0.5f;
            mView.Z *= 0.5f;

            //mView.X += 1.0f;
            mView *= Matrix3.CreateRotationY((float) -(Math.PI / 6));
            mView.X -= 1.0f;

            //float sum = Math.Abs(mView.X) + Math.Abs(mView.Y) + Math.Abs(mView.Z);

            //mView /= (sum * 2);

            //mView.X -= 1;
            // Console.WriteLine("View: " + mView.X + " " + mView.Z);
            Matrix4 p = Matrix4.LookAt(mPos, mView, mUp);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
            matrix = p;
        }

        public void Look()
        {
            // Glu.gluLookAt(mPos.x, mPos.y, mPos.z, //Нами ранее обсуждаемая команда =)
            //               mView.x, mView.y, mView.z,
            //               mUp.x, mUp.y, mUp.z);

            //Matrix4 p = Matrix4.LookAt(mPos, mView, mUp);
            //GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matrix);
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
            Matrix4 a = Matrix4.CreateRotationX((float) a2);
            //Matrix4 b = Matrix4.CreateRotationY((float)(Math.Cos(a2) * Math.Cos(a1)));
            Matrix4 c = Matrix4.CreateRotationY((float) a1);
            Matrix4 scale = Matrix4.CreateScale((float) len);
            Matrix4 rotationMatrix = Matrix4.CreateTranslation(-mView) * scale * c *  a;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref rotationMatrix);
            matrix = rotationMatrix;
            //Console.WriteLine(a1 + " " + a2 + " " + len);
            //mPos.X = mView.X + (float)(len * Math.Cos(a2) * Math.Sin(a1));
            //mPos.Y = mView.Y + (float)(len * Math.Cos(a2) * Math.Cos(a1));
            //mPos.Z = mView.Z + (float)(len * Math.Sin(a2)) ;

            
            //mPos.X = mView.X + (float)Math.Cos(a2) * (float)Math.Cos(a1);
            //mPos.Y = mView.Y + (float)Math.Sin(a2);
            //mPos.Z = mView.Z + (float)Math.Cos(a2) * (float)Math.Sin(a1);

            
            //mPos.Normalize();
        }

        public void mouse_Events(GLControl AnT)
        {
            if (mouseRotate == true) //Если нажата левая кнопка мыши
            {
                a2 += (myMouseXcoordVar - myMouseXcoord) / 100.0;
                a1 += (myMouseYcoordVar - myMouseYcoord) / 100.0;
            }
            else
            {
                if (mouseMove)
                {
                    double dif = myMouseXcoordVar - myMouseXcoord;
                    if (dif != 0)
                    {
                        len -= (dif / 20);
                        if (len < 0.001)
                        {
                            len = 0.001;
                        }
                    }

                    /*Vector3 vVector = mView - mPos;

                    //vVector.Y = 0.0f; // Это запрещает камере подниматься вверх
                    vVector.Normalize();
                    vVector *= (float) len;

                    //mPos.X += vVector.X;
                    //mPos.Z += vVector.Z;
                    //mView.X += vVector.X;
                    //mView.Z += vVector.Z;

                    Vector3 mStrafe = Cross(mView, mPos, mUp).Normalized();
                    mStrafe *= -(float)len;

                    mPos += vVector;
                    mPos += mStrafe;
                    //mView += vVector;
                    //mView += mStrafe;
                    //mPos.X += mStrafe.X;
                    //mPos.Z += mStrafe.Z;

                    // Добавим теперь к взгляду
                    //mView.X += mStrafe.X;
                    //mView.Z += mStrafe.Z;*/



                }
            }

            myMouseXcoord = myMouseXcoordVar;
            myMouseYcoord = myMouseYcoordVar;
        }

        public void addLen(double d)
        {
            //len += d;
        }

    }
}
