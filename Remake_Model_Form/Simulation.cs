using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using _2d_graphics_d;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Remake_Model_Form;
using OpenTK.Graphics;
//using degreework;

namespace OpenGLNamespace
{
    public class TextRenderer : IDisposable
    {
        Bitmap bmp;
        Graphics gfx;
        int texture;
        //Rectangle rectGFX;
        Rectangle dirty_region;
        bool disposed;
        Font serif = new Font(FontFamily.GenericSerif, 99, FontStyle.Bold);
        // Конструктор нового экземпляра класса
        // width, height - ширина и высота растрового образа

      

        public TextRenderer(): this(100, 100) { }

        public TextRenderer(int width, int height)
        {
            if (GraphicsContext.CurrentContext == null) throw new InvalidOperationException("GraphicsContext не обнаружен");
            try
            {
                bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                gfx = Graphics.FromImage(bmp);
                // Используем сглаживание
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);
                // Свойства текстуры
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
                // Создаем пустую тектсуру, которую потом пополним растровыми данымми с текстом (см.
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

                Clear(Color.Transparent);
                
            } catch(Exception e) { }
        }
        // Заливка образа цветом color
        public void Clear(Color color)
        {
            gfx.Clear(color);
            dirty_region = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public void DrawString(string text, PointF point)
        {
            DrawString(text, serif, Brushes.Black, point);
        }

        // Выводит строку текта text в точке point растрового образе, используя фонт font и цвета brush
        // Начало координат растрового образа находится в его левом верхнем углу
        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            gfx.DrawString(text, font, brush, point);

            SizeF size = gfx.MeasureString(text, font);
            dirty_region = Rectangle.Round(RectangleF.Union(dirty_region, new RectangleF(point, size)));
            dirty_region = Rectangle.Intersect(dirty_region, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }
        // Получает обработчик texture (System.Int32) текструры, который связывается с TextureTarget.Texture2d
        // см.в OnRenderFrame: GL.BindTexture(TextureTarget.Texture2D, renderer.Texture)
        public int Texture
        {
            get
            {
                UploadBitmap();
                return texture;
            }
        }
        // Выгружеат растровые данные в текстуру OpenGL
        void UploadBitmap()
        {
            if (dirty_region != RectangleF.Empty)
            {
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(dirty_region,
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                    dirty_region.X, dirty_region.Y, dirty_region.Width, dirty_region.Height,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bmp.UnlockBits(data);

                dirty_region = Rectangle.Empty;
            }
        }
        void Dispose(bool manual)
        {
            if (!disposed)
            {
                if (manual)
                {
                    bmp.Dispose();
                    gfx.Dispose();
                    if (GraphicsContext.CurrentContext != null) GL.DeleteTexture(texture);
                }
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static bool CloseHandle(IntPtr handle);

        ~TextRenderer()
        {
            Dispose(true);
        }

    }

    public class Simulation //основной класс
    {
        //float a;
        public Data data;
        public GLControl AnT;//компонент на котором рисуем
        public Nodes nodes;//сохранил их отдельно сюда для удобства
        public Elements elements;
        public TextRenderer textRenderer;
        double scale;
        float scale2d;//переменная отвечающая за масштаб в 2d
        public string line1;
        public string line_proj;
        public string s;
        /*float xAngle;
        float yAngle;
        float zAngle;

        int oldX, oldY;
        bool mouseDown;*/

        public int type1;//плоский, два д, или с анимацией
        public int type2;//какой тип с анимацией

        public int colorType;//тип закраски

        Animation animation;//класс анимации

        public MyCamera cam;//класс камеры

        int x0;//в 2d отступ от начала кооринат
        int y0;

        int tempX0;//просто темповые переменные
        int tempY0;
        int mouseX;//координаты мыши
        int mouseY;

        AnimationXY animationXY;//класс анимации для перемещения по XY

        bool drawNumberOfNode;//рисовать ли закрепления
        bool drawNumberOfElement;
        bool drawNumberOfProperty;

        float singlePointX;
        float singlePointY;

        bool showBounds;
        bool showForce;

        Blink blink;
        bool showBlink;

        bool showZones;

        bool showYellow;


        Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            set { this.backgroundColor = value; }
            get { return backgroundColor; }
        }

        double forceScale;
        Double ZeroZone;
        public Vector3 point;
        public float pointRadius = 4.0f;
        bool showPoint;
        public Double myMouseXcoordVar;
        public Double myMouseXcoord;
        public bool mouseMove;

        public Simulation(GLControl AnT)//констуркор тут инициализация
        {
            ZeroZone = 10;
            forceScale = 1;

            showBounds = false;
            showForce = false;
            showBlink = false;
            showZones = false;
            drawNumberOfNode = false;
            drawNumberOfElement = false;
            drawNumberOfProperty = false;
            showYellow = false;
            cam = new MyCamera();
            //cam.Position_Camera(0, 5, -10, 0, 0, 0, 0, 1, 0);


            animation = new Animation(this);
            //mouseDown = false;
            //a = 0;
            scale = 0.01;
            scale2d = 4;

            /*xAngle = 20;
            yAngle = 25;
            zAngle = 10;*/

            type1 = 1;
            type2 = 1;

            colorType = -1;

            this.AnT = AnT;

            System.IO.StreamReader file =
    new System.IO.StreamReader(Application.StartupPath + "\\config.txt");
            string line = file.ReadLine();
            line_proj = line;
            s = line_proj.Substring(line_proj.LastIndexOf('\\') + 1);
            //MessageBox.Show(s);
            line1 = file.ReadLine();
            Form1.form.Text = "Проект" + " " + s + " " + "Модель" + " " + line1;
            file.Close();
            LoadData(line);

            animationXY = new AnimationXY(data);
            blink = new Blink(data);
            // textRenderer = new TextRenderer(8, 13);
            // Glut.glutInit();
            // Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            init3d();
            GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha,
                    BlendingFactorDest.OneMinusSrcAlpha);

        }


        public void init3d()//инициализируем 3д
        {
            if (cam == null)
            {
                cam = new MyCamera();
            }
            if (Form2.form2 != null)
            {
                Form2.form2.setComboBoxTo3D();
            }
            showPoint = false;
            //cam.Position_Camera(0, 5, -10, 0, 0, 0, 0, 1, 0);
            // очитка окна
            GL.ClearColor(BackgroundColor);
            
            // установка порта вывода в соотвествии с размерами элемента anT
            GL.Viewport(0, 0, AnT.Width, AnT.Height);


            // настройка проекции
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            //Glu.gluP
            double zNear = 0.01;
            double zFar = 3000;
            double degrees = 60;
            double aspect = (float)AnT.Width / (float)AnT.Height;
            double fH = Math.Tan(degrees / 360.0 * Math.PI) * zNear;
            double fW = fH * aspect;
            // GL.Frustum(-fW, fW, -fH, fH, zNear, zFar);
            // Glu.gluPerspective(60, (float)AnT.Width / (float)AnT.Height, 0.5, 3000);
            // GL.MatrixMode(MatrixMode.Modelview);
            // GL.LoadIdentity();

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView(((float) Math.PI) / 2.0f,
                    (float)AnT.Width / (float)AnT.Height, (float) zNear, (float) zFar);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref p);

            // настройка параметров OpenGL для визуализации
            cam.update();
            GL.Enable(EnableCap.DepthTest);
            float maxX = (float) nodes.all_nodes.Select((node) => node.x).Max();
            float maxY = (float) nodes.all_nodes.Select((node) => node.y).Max();
            cam.setLookAt(((float) (maxX * scale)), ((float)(maxY * scale)));
        }

        public void init2d()//инициализируем 2д
        {
            
            if (Form2.form2 != null)
            {
                Form2.form2.setComboBoxTo2D();
            }
            singlePointX = -1;
            singlePointX = -1;
            // x0 = 20;
            // y0 = 20;
            //Glut.glutInit();
            // инициализация режима окна
            //Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            // устанавливаем цвет очистки окна
            GL.ClearColor(BackgroundColor);
            // устанавливаем порт вывода, основываясь на размерах элемента управления AnT
            GL.Viewport(0, 0, AnT.Width, AnT.Height);
            // устанавливаем проекционную матрицу
            GL.MatrixMode(MatrixMode.Modelview);
            // очищаем ее
            GL.LoadIdentity();

            GL.Ortho(0.0, AnT.Width, 0.0, AnT.Height, 0, 30);
            // Glu.gluOrtho2D(0.0, AnT.Width, 0.0, AnT.Height);
            //Glu.gluOrtho2D(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0);
            // переходим к объектно-видовой матрице
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //ProgrammDrawingEngine = new anEngine(AnT.Width, AnT.Height, AnT.Width, AnT.Height);
            
        }
        public void LoadData(string path)//грузим данныe
        {
            data = new Data();
            Data.file_path = path;

            data.openfile_res1();
            data.openfile_res2();
            data.find_nodes_with_force();

            nodes = data.temp_nodes;
            elements = data.temp_elem;
            data.calculateS();
            data.element_destroy();

            //MessageBox.Show(data.colorStressMax.ToString()+"   "+data.colorStressMin.ToString());
            // MessageBox.Show(elements.all_elements.Count.ToString());

        }

        public void Update()//для поворотомв мышкой
        {
            if (type1 != 2)
            {
                cam.mouse_Events(AnT);
            } else
            {
                if (mouseMove)
                {
                    double dif = myMouseXcoord - myMouseXcoordVar;
                    if (dif != 0)
                    {
                        scale2d -= ((float)(dif / 20));
                        if (scale2d < 0.001f)
                        {
                            scale2d = 0.001f;
                        }
                    }
                }
                myMouseXcoordVar = myMouseXcoord;
            }
            cam.update();
        }



        public void Draw()//рисуем
        {
            animationXY.Update();
            if (type1 == 2)//выираем как рисовать
            {
                Draw2d();
            }
            else
            {
                Draw3d();
            }
        }


        public void Draw3d()//рисуем 3д
        {
            GL.ClearColor(BackgroundColor);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();
            GL.Color3(1.0f, 0, 0);

            cam.update();
            cam.Look();

            GL.PushMatrix();
            GL.Translate(-1, 0, 0);
            GL.Rotate(60, 0, 1, 0);
            GL.Scale(scale, scale, scale);


            //GL.glRotatef(xAngle, 1, 0, 0);
            //GL.glRotatef(yAngle, 0, 1, 0);
            //GL.glRotatef(zAngle, 0, 0, 1);

            //a += 0.5f;

            DrawCoor();
            DrawTriangles();
            if (type1 == 1)
            {
                if (showForce)
                {
                    DrawForce();
                }
                if (showBounds)
                {
                    DrawBounds();
                }
                if (showZones)
                {

                    DrawZones();
                }
            }

            if (showPoint)
            {
                GL.Disable(EnableCap.PolygonOffsetFill);
                //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                var color = Color.FromArgb(128, Color.Yellow);
                GL.Enable(EnableCap.Blend);
                GL.DepthMask(false);
                GL.Color4(color);
                /*Console.WriteLine(color.R + "  " + color.G + " " + color.B
                    + " " + color.A);*/
                drawSphere(point.X, point.Y, point.Z, pointRadius);
                GL.Disable(EnableCap.Blend);
                GL.DepthMask(true);
                //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            GL.PopMatrix();
            GL.Flush();
            AnT.Invalidate();
        }
        public void Draw2d()//рисуем в 2д
        {
            GL.ClearColor(BackgroundColor);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            // устанавливаем текущий цвет - красный
            GL.Color3(255, 0, 0);


            // активируем режим рисования линий, на основе
            // последовательного соединения всех вершин в отрезки
            if (showBounds)
            {
                DrawBounds2d();
            }
            DrawCoor2d();
            if (showZones)
            {
                DrawZones2d();
            }
            if (showForce)
            {
                DrawForce2d();
            }
            if ((singlePointX != -1) && (singlePointY != -1))
            {
                GL.Color4(Color.FromArgb(128, Color.Yellow));
                GL.Enable(EnableCap.Blend);
                GL.Begin(BeginMode.Polygon);
                for (int i = 0; i < 360; i++)
                {
                    float l = pointRadius;
                    float x = (float)(singlePointX + l * Math.Cos(i));
                    float y = (float)(singlePointY + l * Math.Sin(i));
                    GL.Vertex2(x, y);

                }
                GL.End();
                GL.Disable(EnableCap.Blend);
            }

            DrawTriangles();

            GL.Flush();
            AnT.Invalidate();
            //GL.Disable(EnableCap.LineSmooth);
        }

        public void DrawCoor()//рисуем координатную систему в 3д
        {
            int a = 10;
            int b = 5;
            double maxX = data.temp_nodes.all_nodes.Select(node => node.x).Max();
            double maxY = data.temp_nodes.all_nodes.Select(node => node.y).Max();
            double xlen = maxX + 20;
            double ylen = maxY + 20;
            const int zlen = 40;
            GL.Enable(EnableCap.LineSmooth);
            GL.Begin(BeginMode.Lines);  // указываем, что будем рисовать
            //GL.glLineWidth(0.88f);
            GL.Color3(0, 0, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, zlen, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(-xlen, 0, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, ylen);
            GL.End();

            GL.Begin(BeginMode.Triangles);  // указываем, что будем рисовать
            //GL.glLineWidth(0.88f);
            GL.Color3(0, 0, 0);

            GL.Vertex3(0, zlen, 0);
            GL.Vertex3(b, zlen - a, 0);
            GL.Vertex3(-b, zlen - a, 0);

            GL.Vertex3(-xlen, 0, 0);
            GL.Vertex3(-xlen + a, 0, b);
            GL.Vertex3(-xlen + a, 0, -b);

            GL.Vertex3(0, 0, ylen);
            GL.Vertex3(b, 0, ylen - a);
            GL.Vertex3(-b, 0, ylen - a);

            GL.End();

            GL.Disable(EnableCap.LineSmooth);

            DrawString3d("Y", 0, zlen + 1, 0);
            DrawString3d("X", ((int) -xlen - 1), 0, 0);
            DrawString3d("Z", 0, 0, ((int) ylen + 1));
            //GL.BindTexture(TextureTarget.Texture3D, textRenderer.Texture);
        }

        public void DrawCoor2d()//координатную систему в 2д
        {
            int a = 10;
            int b = 5;
            //int len = Convert.ToInt32(120*scale2d);
            int lenX = AnT.Width;
            int lenY = AnT.Height;
            GL.Color3(0, 0, 0);
            GL.Begin(BeginMode.Lines);
            // первая вершина будет находиться в начале координат
            GL.Vertex2(x0, y0);
            GL.Vertex2(x0 + lenX - 30, y0);
            GL.Vertex2(x0, y0);
            GL.Vertex2(x0, y0 + lenY - 30);
            GL.End();

            GL.Begin(BeginMode.Triangles);
            // первая вершина будет находиться в начале координат
            GL.Vertex2(x0 + lenX - 30, y0);
            GL.Vertex2(x0 + lenX - 30 - a, y0 + b);
            GL.Vertex2(x0 + lenX - 30 - a, y0 - b);

            GL.Vertex2(x0, y0 + lenY - 30);
            GL.Vertex2(x0 + b, y0 + lenY - 30 - a);
            GL.Vertex2(x0 - b, y0 + lenY - 30 - a);
            GL.End();

            DrawString2d("X", x0 + lenX - 20, y0);
            DrawString2d("Y", x0, y0 + lenY - 10);
        }

        public void DrawString3d(string str, int x, int y, int z)//надписьт в 3д
        {
            GL.Enable(EnableCap.Blend);
            GL.RasterPos3(x, z, y);
            for (int i = 0; i < str.Length; i++)
            {
                textRenderer = new TextRenderer();
                textRenderer.DrawString(str, new PointF(x, y));

                float Size = 16;
                GL.Color3(Color.Transparent);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, textRenderer.Texture);
                // Вывод квадрата с текстурой, содержащей текст (три строки)
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord3(x, y + Size, z);
                GL.Vertex3(x - Size, y - Size, z);
                GL.TexCoord3(x + Size, y + Size, z);
                GL.Vertex3(x + Size, y - Size, z);
                GL.TexCoord3(x + Size, y, z);
                GL.Vertex3(x + Size, y + Size, z);
                GL.TexCoord3(x, y, z);
                GL.Vertex3(x - Size, y + Size, z);
                GL.End();
                GL.Disable(EnableCap.Texture2D);
                // Glut.utBitmapCharacter(Glut.UT_BITMAP_8_BY_13, str[i]);
                textRenderer.Dispose();
            }
            GL.Disable(EnableCap.Blend);
        }

        public void DrawString2d(string str, int x, int y)//надпись в 2д
        {
            GL.Enable(EnableCap.Blend);
            // GL.Color3(0, 0, 0);
            // GL.RasterPos2(x, y);
            //for (int i = 0; i < str.Length; i++)
            //{
            textRenderer = new TextRenderer();
            textRenderer.DrawString(str, new PointF(x, y));

            float Size = 16;
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.Transparent);
            GL.BindTexture(TextureTarget.Texture2D, textRenderer.Texture);
            // Вывод квадрата с текстурой, содержащей текст (три строки)
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(x, y + Size);
            GL.Vertex2(x - Size, y - Size);
            GL.TexCoord2(x + Size, y + Size);
            GL.Vertex2(x + Size, y - Size);
            GL.TexCoord2(x + Size, y);
            GL.Vertex2(x + Size, y + Size);
            GL.TexCoord2(x, y);
            GL.Vertex2(x - Size, y + Size);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            textRenderer.Dispose();
            // Glut.utBitmapCharacter(Glut.UT_BITMAP_8_BY_13, str[i]);
            //}
        }


        public void DrawZones()
        {
            for (int i = 0; i < data.k; i++)
            {
                GL.Enable(EnableCap.LineSmooth);
                GL.Begin(BeginMode.LineLoop);
                GL.Color3(0, 0, 0);
                for (int j = 0; j < 16; j += 2)
                {
                    GL.Vertex3(-data.point[i, j], getStartHeight() + 0.1, data.point[i, j + 1]);
                }
                GL.End();
                GL.Disable(EnableCap.LineSmooth);
            }
        }

        public void DrawZones2d()
        {
            for (int i = 0; i < data.k; i++)
            {
                GL.Enable(EnableCap.LineSmooth);
                GL.Begin(BeginMode.LineLoop);
                GL.Color3(0, 0, 0);
                for (int j = 0; j < 16; j += 2)
                {
                    GL.Vertex2(x0 + scale2d * data.point[i, j], y0 + scale2d * data.point[i, j + 1]);
                }
                GL.End();
                GL.Disable(EnableCap.LineSmooth);
            }
        }

        public void DrawForce()//рисуем силы в 3д
        {
            //MessageBox.Show(data.node_with_fofce.Count.ToString());
            foreach (node n in data.node_with_fofce)
            {

                GL.Enable(EnableCap.LineSmooth);
                GL.Begin(BeginMode.Lines);

                GL.Color3(1, 0, 0);
                GL.Vertex3(-n.x, getStartHeight(), n.y);
                GL.Vertex3(-(n.x + n.forceX / 10 * forceScale), getStartHeight(), n.y + n.forceY / 10 * forceScale);
                GL.End();
                GL.Disable(EnableCap.LineSmooth);


                Double l = Math.Sqrt((n.forceX * n.forceX) + (n.forceY * n.forceY));
                Double al = Math.Acos(n.forceY / l);

                if (Math.Abs(Math.Sin(-al) * l - n.forceX) < 0.001)
                {
                    al = -al;
                }

                GL.Begin(BeginMode.Triangles);

                GL.Vertex3(-(n.x + n.forceX / 10 * forceScale), getStartHeight(), n.y + n.forceY / 10 * forceScale);
                GL.Vertex3(-(n.x + n.forceX / 10 * forceScale - 5 * Math.Sin(al + 0.5)), getStartHeight(), n.y + n.forceY / 10 * forceScale - 5 * Math.Cos(al + 0.5));
                GL.Vertex3(-(n.x + n.forceX / 10 * forceScale - 5 * Math.Sin(al - 0.5)), getStartHeight(), n.y + n.forceY / 10 * forceScale - 5 * Math.Cos(al - 0.5));

                GL.End();

            }
        }

        public void DrawForce2d()//рисуем силы в 2д
        {
            //MessageBox.Show(data.node_with_fofce.Count.ToString());
            foreach (node n in data.node_with_fofce)
            {

                GL.Enable(EnableCap.LineSmooth);
                GL.Begin(BeginMode.Lines);

                GL.Color3(1, 0, 0);
                GL.Vertex2(x0 + n.x * scale2d, y0 + n.y * scale2d);
                GL.Vertex2(x0 + (n.x + n.forceX / 40 * forceScale) * scale2d, y0 + (n.y + n.forceY / 40 * forceScale) * scale2d);
                GL.End();
                GL.Disable(EnableCap.LineSmooth);

                Double l = Math.Sqrt((n.forceX * n.forceX) + (n.forceY * n.forceY));
                Double al = Math.Acos(n.forceY / l);

                if (Math.Abs(Math.Sin(-al) * l - n.forceX) < 0.001)
                {
                    al = -al;
                }

                GL.Begin(BeginMode.Lines);

                GL.Vertex2(x0 + (n.x + n.forceX / 40 * forceScale) * scale2d, y0 + (n.y + n.forceY / 40 * forceScale) * scale2d);
                GL.Vertex2(x0 + (n.x + n.forceX / 40 * forceScale - 5 * Math.Sin(al + 0.3)) * scale2d, y0 + (n.y + n.forceY / 40 * forceScale - 5 * Math.Cos(al + 0.3)) * scale2d);
                GL.Vertex2(x0 + (n.x + n.forceX / 40 * forceScale) * scale2d, y0 + (n.y + n.forceY / 40 * forceScale) * scale2d);
                GL.Vertex2(x0 + (n.x + n.forceX / 40 * forceScale - 5 * Math.Sin(al - 0.3)) * scale2d, y0 + (n.y + n.forceY / 40 * forceScale - 5 * Math.Cos(al - 0.3)) * scale2d);
                GL.End();
            }
        }

        public void DrawBounds2d()//рисуем закрепления в 2д
        {
            foreach (node n in data.temp_nodes.all_nodes)
            {
                float l = 1.5f;
                float d = 0;
                float a = 4;
                float b = 2;

                float x;
                float y;
                if (n.TypeBound == 1)
                {

                    GL.Begin(BeginMode.Polygon);
                    GL.Color3(0.3f, 0.3f, 0.3f);
                    for (int i = 0; i < 360; i++)
                    {
                        x = (float)(n.x + d + l * Math.Cos(i));
                        y = (float)(n.y + d + l * Math.Sin(i));
                        GL.Vertex2(x0 + x * scale2d, y0 + y * scale2d);
                    }
                    GL.End();

                    x = (float)n.x + d - l - (a - l * 2) / 2;
                    y = (float)n.y + d - l - b;

                    GL.Begin(BeginMode.Polygon);
                    GL.Vertex2(x0 + x * scale2d, y0 + y * scale2d);
                    GL.Vertex2(x0 + (x + a) * scale2d, y0 + y * scale2d);
                    GL.Vertex2(x0 + (x + a) * scale2d, y0 + (y + b) * scale2d);
                    GL.Vertex2(x0 + x * scale2d, y0 + (y + b) * scale2d);
                    GL.End();
                    //MessageBox.Show("ds");
                }
                else
                    if (n.TypeBound == 10)
                {
                    GL.Begin(BeginMode.Polygon);
                    GL.Color3(0.3f, 0.3f, 0.3f);
                    for (int i = 0; i < 360; i++)
                    {
                        x = (float)(n.x + d + l * Math.Cos(i));
                        y = (float)(n.y + d + l * Math.Sin(i));
                        GL.Vertex2(x0 + x * scale2d, y0 + y * scale2d);
                    }
                    GL.End();

                    //x = (float)n.x + d - l - (a - l * 2) / 2;
                    x = (float)n.x + d - l - b;
                    y = (float)n.y + d - l - (a - l * 2) / 2;

                    GL.Begin(BeginMode.Polygon);
                    GL.Vertex2(x0 + x * scale2d, y0 + y * scale2d);
                    GL.Vertex2(x0 + (x + b) * scale2d, y0 + y * scale2d);
                    GL.Vertex2(x0 + (x + b) * scale2d, y0 + (y + a) * scale2d);
                    GL.Vertex2(x0 + x * scale2d, y0 + (y + a) * scale2d);
                    GL.End();
                }
                else
                        if (n.TypeBound == 11)
                {
                    GL.Begin(BeginMode.Polygon);
                    GL.Color3(0.3f, 0.3f, 0.3f);
                    for (int i = 0; i < 360; i++)
                    {
                        x = (float)(n.x + d + l * Math.Cos(i));
                        y = (float)(n.y + d + l * Math.Sin(i));
                        GL.Vertex2(x0 + x * scale2d, y0 + y * scale2d);
                    }
                    GL.End();

                    //x = (float)n.x + d - l - (a - l * 2) / 2;
                    x = (float)n.x + d;
                    y = (float)n.y + d;

                    GL.Begin(BeginMode.Polygon);
                    GL.Vertex2(x0 + x * scale2d, y0 + y * scale2d);

                    GL.Vertex2(x0 + (x - 1.5f) * scale2d, y0 + (y - 2.5f) * scale2d);
                    GL.Vertex2(x0 + (x + 1.5f) * scale2d, y0 + (y - 2.5f) * scale2d);

                    GL.End();
                }
            }
        }

        public void DrawBounds()//рисуем закрепдения в 3д
        {
            foreach (node n in data.temp_nodes.all_nodes)
            {
                if (n.TypeBound == 1)
                {
                    DrawBound1(n);
                }
                else
                    if (n.TypeBound == 10)
                {
                    DrawBound10(n);
                }
                else
                        if (n.TypeBound == 11)
                {
                    DrawBound11(n);
                }
            }
        }

        public void DrawBound1(node n)//первый тип
        {
            GL.Color3(0.3f, 0.3f, 0.3f);
            Double a = 4;
            Double b = 2;
            Double c = 0.5;

            Double X = n.x - b / 2;
            Double Y = n.y - a / 2;
            Double Z = getStartHeight() + 1.5;

            GL.Begin(BeginMode.Quads);

            GL.Vertex3(-X, Z, Y);
            GL.Vertex3(-(X + b), Z, Y);
            GL.Vertex3(-(X + b), Z, Y + a);
            GL.Vertex3(-X, Z, Y + a);


            GL.Vertex3(-X, Z + c, Y);
            GL.Vertex3(-(X + b), Z + c, Y);
            GL.Vertex3(-(X + b), Z + c, Y + a);
            GL.Vertex3(-X, Z + c, Y + a);


            GL.Vertex3(-X, Z, Y);
            GL.Vertex3(-(X + b), Z, Y);
            GL.Vertex3(-(X + b), Z + c, Y);
            GL.Vertex3(-X, Z + c, Y);


            GL.Vertex3(-X, Z, Y + a);
            GL.Vertex3(-(X + b), Z, Y + a);
            GL.Vertex3(-(X + b), Z + c, Y + a);
            GL.Vertex3(-X, Z + c, Y + a);


            GL.Vertex3(-(X + b), Z, Y);
            GL.Vertex3(-(X + b), Z, Y + a);
            GL.Vertex3(-(X + b), Z + c, Y + a);
            GL.Vertex3(-(X + b), Z + c, Y);



            GL.Vertex3(-X, Z, Y);
            GL.Vertex3(-X, Z, Y + a);
            GL.Vertex3(-X, Z + c, Y + a);
            GL.Vertex3(-X, Z + c, Y);


            GL.End();

            GL.PushMatrix();
            GL.Translate(-n.x, getStartHeight() + 3, n.y);
            drawSphere(-n.x * scale, -5, n.y * scale, 0.5);
            // Glut.glutSolidSphere(1, 109, 109);
            GL.PopMatrix();
        }

        public void DrawBound10(node n)//второй тип
        {
            GL.Color3(0.3f, 0.3f, 0.3f);
            Double a = 3;
            Double b = 0.5;
            Double c = 2;

            Double X = n.x - 1.5;
            Double Y = n.y - a / 2;
            Double Z = getStartHeight() + 1.5;

            GL.Begin(BeginMode.Quads);

            GL.Vertex3(-X, Z, Y);
            GL.Vertex3(-(X + b), Z, Y);
            GL.Vertex3(-(X + b), Z, Y + a);
            GL.Vertex3(-X, Z, Y + a);


            GL.Vertex3(-X, Z + c, Y);
            GL.Vertex3(-(X + b), Z + c, Y);
            GL.Vertex3(-(X + b), Z + c, Y + a);
            GL.Vertex3(-X, Z + c, Y + a);


            GL.Vertex3(-X, Z, Y);
            GL.Vertex3(-(X + b), Z, Y);
            GL.Vertex3(-(X + b), Z + c, Y);
            GL.Vertex3(-X, Z + c, Y);


            GL.Vertex3(-X, Z, Y + a);
            GL.Vertex3(-(X + b), Z, Y + a);
            GL.Vertex3(-(X + b), Z + c, Y + a);
            GL.Vertex3(-X, Z + c, Y + a);


            GL.Vertex3(-(X + b), Z, Y);
            GL.Vertex3(-(X + b), Z, Y + a);
            GL.Vertex3(-(X + b), Z + c, Y + a);
            GL.Vertex3(-(X + b), Z + c, Y);



            GL.Vertex3(-X, Z, Y);
            GL.Vertex3(-X, Z, Y + a);
            GL.Vertex3(-X, Z + c, Y + a);
            GL.Vertex3(-X, Z + c, Y);


            GL.End();

            GL.PushMatrix();
            GL.Translate(-n.x, getStartHeight() + 3, n.y);
            drawSphere(-n.x * scale, -5, n.y * scale, 0.5);
            GL.PopMatrix();
        }

        public void DrawBound11(node n)//третий тип
        {
            // MessageBox.Show("fdsf");
            GL.Color3(0.3f, 0.3f, 0.3f);
            GL.PushMatrix();
            GL.Translate(-n.x, getStartHeight() + 1, n.y);
            GL.Rotate(-90, 1, 0, 0);
            drawCone(1, 3, 200);
            GL.PopMatrix();

            GL.Color3(0.3f, 0.3f, 0.3f);
            GL.PushMatrix();
            GL.Translate(-n.x, getStartHeight() + 4, n.y);
            //Glut.glutSolidCone(1, 20, 2, 2);
            drawSphere(-n.x * scale, -5, n.y * scale, 1.5);
            GL.PopMatrix();
        }

        public void DrawTriangles()//рисуем треугольнкии
        {
            Double h1 = 0, h2 = 0, h3 = 0;
            for (int i = 0; i < elements.all_elements.Count; i++)
            {
                node n1 = nodes.getnode(elements.get_element(i).node1 - 1);
                node n2 = nodes.getnode(elements.get_element(i).node2 - 1);
                node n3 = nodes.getnode(elements.get_element(i).node3 - 1);
                if (type1 == 2)
                {
                    if (drawNumberOfNode)
                    {
                        DrawString2d(n1.r_number.ToString(), Convert.ToInt32(x0 + n1.x * scale2d), Convert.ToInt32(y0 + n1.y * scale2d));
                        DrawString2d(n2.r_number.ToString(), Convert.ToInt32(x0 + n2.x * scale2d), Convert.ToInt32(y0 + n2.y * scale2d));
                        DrawString2d(n3.r_number.ToString(), Convert.ToInt32(x0 + n3.x * scale2d), Convert.ToInt32(y0 + n3.y * scale2d));
                    }
                    if (drawNumberOfElement)
                    {
                        DrawString2d((i + 1).ToString(), Convert.ToInt32(x0 + ((n1.x + n2.x + n3.x) / 3) * scale2d), Convert.ToInt32(y0 + ((n1.y + n2.y + n3.y) / 3) * scale2d));
                    }
                    if (drawNumberOfProperty)
                    {
                        DrawString2d((elements.get_element(i).material).ToString(), Convert.ToInt32(x0 + ((n1.x + n2.x + n3.x) / 3) * scale2d), Convert.ToInt32(y0 + ((n1.y + n2.y + n3.y) / 3) * scale2d));
                    }
                }
            }
            for (int i = 0; i < elements.all_elements.Count; i++)
            //for (int i = 0; i < 1; i++)
            {
                node n1 = nodes.getnode(elements.get_element(i).node1 - 1);
                node n2 = nodes.getnode(elements.get_element(i).node2 - 1);
                node n3 = nodes.getnode(elements.get_element(i).node3 - 1);

                //getH(i, ref h1, ref h2, ref h3);
                animation.Update(i, ref h1, ref h2, ref h3);

                if (i == 1173)
                {
                    //MessageBox.Show(n1.stress.ToString());

                    int q = 1;
                    q++;
                }

                Color c1 = getColor(n1.stress);
                Color c2 = getColor(n2.stress);
                Color c3 = getColor(n3.stress);

                if (showBlink)
                {
                    blink.getColor(i, ref c1, ref c2, ref c3);
                }
                if (type1 == 2)
                {
                    Drawprism2d(n1.x, n1.y, n2.x, n2.y, n3.x, n3.y, h1, h2, h3, c1, c2, c3);
                    /*if (drawNumberOfNode)
                    {
                        DrawString2d(elements.get_element(i).node1.ToString(), Convert.ToInt32(x0 + n1.x * scale2d), Convert.ToInt32(y0 + n1.y * scale2d));
                        DrawString2d(elements.get_element(i).node2.ToString(), Convert.ToInt32(x0 + n2.x * scale2d), Convert.ToInt32(y0 + n2.y * scale2d));
                        DrawString2d(elements.get_element(i).node3.ToString(), Convert.ToInt32(x0 + n3.x * scale2d), Convert.ToInt32(y0 + n3.y * scale2d));
                    }
                    if (drawNumberOfElement)
                    {
                        DrawString2d((i + 1).ToString(), Convert.ToInt32(x0 + ((n1.x + n2.x + n3.x) / 3) * scale2d), Convert.ToInt32(y0 + ((n1.y + n2.y + n3.y) / 3) * scale2d));
                    }*/
                }
                else
                {
                    DrawPrism(n1.x, n1.y, n2.x, n2.y, n3.x, n3.y, h1, h2, h3, c1, c2, c3);
                }
                //DrawPrism(n1.x, n1.y, n2.x, n2.y, n3.x, n3.y, Data.mass_of_material[elements.get_element(i).material].TS*8);
                //MessageBox.Show(Data.mass_of_material[elements.get_element(i).material].TS.ToString());
            }

            /*for (int i = 0; i < elements.all_elements.Count; i++)
            {
                node n1 = nodes.getnode(elements.get_element(i).node1 - 1);
                node n2 = nodes.getnode(elements.get_element(i).node2 - 1);
                node n3 = nodes.getnode(elements.get_element(i).node3 - 1);
                if (type1 == 2)
                {
                    if (drawNumberOfNode)
                    {
                        DrawString2d(elements.get_element(i).node1.ToString(), Convert.ToInt32(x0 + n1.x * scale2d), Convert.ToInt32(y0 + n1.y * scale2d));
                        DrawString2d(elements.get_element(i).node2.ToString(), Convert.ToInt32(x0 + n2.x * scale2d), Convert.ToInt32(y0 + n2.y * scale2d));
                        DrawString2d(elements.get_element(i).node3.ToString(), Convert.ToInt32(x0 + n3.x * scale2d), Convert.ToInt32(y0 + n3.y * scale2d));
                    }
                    if (drawNumberOfElement)
                    {
                        DrawString2d((i + 1).ToString(), Convert.ToInt32(x0 + ((n1.x + n2.x + n3.x) / 3) * scale2d), Convert.ToInt32(y0 + ((n1.y + n2.y + n3.y) / 3) * scale2d));
                    }
                    if (drawNumberOfProperty)
                    {
                        DrawString2d((elements.get_element(i).material).ToString(), Convert.ToInt32(x0 + ((n1.x + n2.x + n3.x) / 3) * scale2d), Convert.ToInt32(y0 + ((n1.y + n2.y + n3.y) / 3) * scale2d));
                    }
                }
            }*/

            animation.updateLen();
            blink.Update();
        }
        public void Drawprism2d(Double x1, Double y1, Double x2, Double y2, Double x3, Double y3, Double h1, Double h2, Double h3, Color c1, Color c2, Color c3)//рисуем треугольники в 2д
        {
            GL.Begin(BeginMode.Triangles);  // указываем, что будем рисовать

            GL.Color3(c1.R / 255.0f, c1.G / 255.0f, c1.B / 255.0f);
            GL.Vertex2(x0 + x1 * scale2d, y0 + y1 * scale2d);
            GL.Color3(c2.R / 255.0f, c2.G / 255.0f, c2.B / 255.0f);
            GL.Vertex2(x0 + x2 * scale2d, y0 + y2 * scale2d);
            GL.Color3(c3.R / 255.0f, c3.G / 255.0f, c3.B / 255.0f);
            GL.Vertex2(x0 + x3 * scale2d, y0 + y3 * scale2d);

            GL.End();

            GL.Begin(BeginMode.LineStrip);  // указываем, что будем рисовать
            GL.Color3(0, 0, 0);

            GL.Vertex2(x0 + x1 * scale2d, y0 + y1 * scale2d);

            GL.Vertex2(x0 + x2 * scale2d, y0 + y2 * scale2d);

            GL.Vertex2(x0 + x3 * scale2d, y0 + y3 * scale2d);

            GL.Vertex2(x0 + x1 * scale2d, y0 + y1 * scale2d);

            GL.End();


            //System.Threading.Thread.Sleep(10000);
        }
        public void DrawPrism(Double x1, Double y1, Double x2, Double y2, Double x3, Double y3, Double h1, Double h2, Double h3, Color c1, Color c2, Color c3)//рисуем их в 3д
        {
            //MessageBox.Show(x1.ToString() + " " + y1.ToString() + " " + x2.ToString() + " " + y2.ToString());

            GL.Begin(BeginMode.Triangles);  // указываем, что будем рисовать
            GL.Color3(c1.R / 255.0, c1.G / 255.0, c1.B / 255.0);
            GL.Vertex3(-x1, 0, y1);
            GL.Color3(c2.R / 255.0, c2.G / 255.0, c2.B / 255.0);
            GL.Vertex3(-x2, 0, y2);
            GL.Color3(c3.R / 255.0, c3.G / 255.0, c3.B / 255.0);
            GL.Vertex3(-x3, 0, y3);

            GL.Color3(c1.R / 255.0, c1.G / 255.0, c1.B / 255.0);
            GL.Vertex3(-x1, h1, y1);
            GL.Color3(c2.R / 255.0, c2.G / 255.0, c2.B / 255.0);
            GL.Vertex3(-x2, h2, y2);
            GL.Color3(c3.R / 255.0, c3.G / 255.0, c3.B / 255.0);
            GL.Vertex3(-x3, h3, y3);

            GL.End();


            GL.Begin(BeginMode.Quads);  // указываем, что будем рисовать
            GL.Color3(0.5, 0.5, 0.5);

            GL.Vertex3(-x1, 0, y1);
            GL.Vertex3(-x2, 0, y2);
            GL.Vertex3(-x2, h2, y2);
            GL.Vertex3(-x1, h1, y1);

            GL.Vertex3(-x1, 0, y1);
            GL.Vertex3(-x3, 0, y3);
            GL.Vertex3(-x3, h3, y3);
            GL.Vertex3(-x1, h1, y1);

            GL.Vertex3(-x3, 0, y3);
            GL.Vertex3(-x2, 0, y2);
            GL.Vertex3(-x2, h2, y2);
            GL.Vertex3(-x3, h3, y3);

            GL.End();



            GL.Begin(BeginMode.LineStrip);  // указываем, что будем рисовать
            GL.Color3(0, 0, 0);
            GL.Vertex3(-x1, 0, y1);
            GL.Vertex3(-x2, 0, y2);
            GL.Vertex3(-x3, 0, y3);
            GL.End();

            GL.Begin(BeginMode.LineStrip);  // указываем, что будем рисовать
            GL.Color3(0, 0, 0);
            GL.Vertex3(-x1, h1, y1);
            GL.Vertex3(-x2, h2, y2);
            GL.Vertex3(-x3, h3, y3);
            GL.End();

            GL.Begin(BeginMode.Lines);  // указываем, что будем рисовать
            GL.Color3(0, 0, 0);
            GL.Vertex3(-x1, 0, y1);
            GL.Vertex3(-x1, h1, y1);

            GL.Vertex3(-x2, 0, y2);
            GL.Vertex3(-x2, h2, y2);

            GL.Vertex3(-x3, 0, y3);
            GL.Vertex3(-x3, h3, y3);
            GL.End();

        }

        public Color getColor(Double stress)//вычисляем цывет для напряжения
        {
            if (colorType == -1)
            {
                return Color.DarkGray;
            }
            Double temp = Math.Abs(data.colorStressMin);
            Double min = data.colorStressMin;
            Double max = data.colorStressMax;
            int raz = 0;
            if (showYellow)
            {
                if ((stress <= data.colorStressMax * (ZeroZone / 100.0)) && (stress >= data.colorStressMin * (ZeroZone / 100.0)))
                {
                    return Color.Yellow;
                }
            }
            if (min < 0)
            {
                min += temp;
                stress += temp;
                max += temp;
            }
            else
            {
                min -= temp;
                stress -= temp;
                max -= temp;
            }

            raz = (int)Math.Round(255 * stress / max);

            return Color.FromArgb(raz, 0, 255 - raz);
        }

        public void getH(int i, ref Double h1, ref Double h2, ref Double h3)//вычисляем высоты
        {
            if (type1 == 1)
            {
                h1 = getStartHeight();
                h2 = getStartHeight();
                h3 = getStartHeight();
            }

            if (type1 == 3)
            {
                if (type2 == 1)
                {
                    node n1 = nodes.getnode(elements.get_element(i).node1 - 1);
                    node n2 = nodes.getnode(elements.get_element(i).node2 - 1);
                    node n3 = nodes.getnode(elements.get_element(i).node3 - 1);
                    h1 = n1.movX * 1000;
                    h2 = n2.movX * 1000;
                    h3 = n3.movX * 1000;

                }

                if (type2 == 2)
                {
                    node n1 = nodes.getnode(elements.get_element(i).node1 - 1);
                    node n2 = nodes.getnode(elements.get_element(i).node2 - 1);
                    node n3 = nodes.getnode(elements.get_element(i).node3 - 1);
                    h1 = n1.movY * 1000;
                    h2 = n2.movY * 1000;
                    h3 = n3.movY * 1000;

                }

                if ((type2 >= 3) && (type2 <= 9))
                {
                    element elem = elements.all_elements[i];
                    h1 = elem.stress[type2 - 3] / 1000;
                    h2 = elem.stress[type2 - 3] / 1000;
                    h3 = elem.stress[type2 - 3] / 1000;
                }

                if (type2 == 10)
                {
                    node n1 = nodes.getnode(elements.get_element(i).node1 - 1);
                    node n2 = nodes.getnode(elements.get_element(i).node2 - 1);
                    node n3 = nodes.getnode(elements.get_element(i).node3 - 1);
                    Double s = data.geron(n1, n2, n3);

                    h1 = (1 - (s / data.s)) * 200 - 180;
                    h2 = (1 - (s / data.s)) * 200 - 180;
                    h3 = (1 - (s / data.s)) * 200 - 180;
                    //MessageBox.Show(h1.ToString());
                }

                if (type2 == 11)
                {
                    h1 = Data.mass_of_material[elements.get_element(i).material - 1].TS * 10;
                    h2 = Data.mass_of_material[elements.get_element(i).material - 1].TS * 10;
                    h3 = Data.mass_of_material[elements.get_element(i).material - 1].TS * 10;
                }
            }

            h1 /= 5.0;
            h2 /= 5.0;
            h3 /= 5.0;
        }

        public void addScale()//масштаб
        {
            if (type1 == 2)
            {
                scale2d++;
            }
            else
            {
                scale += 0.01;
            }

        }
        public void subScale()
        {
            if (type1 == 2)
            {
                scale2d--;
            }
            else
            {
                scale -= 0.01;
            }
        }

        public void showCoordinates2d(double xt, double yt)//показываем координаты
        {

            if (Form2.form2 != null)
            {
                Form2.form2.textBox1.Text = Math.Round(xt, 2).ToString();
                Form2.form2.textBox2.Text = Math.Round(yt, 2).ToString();
                //Form1.form.textBox1.Text = Math.Round(xt,2).ToString();
                //Form1.form.textBox2.Text = Math.Round(yt,2).ToString();
                make_stress_for_form(Form1.form.type_stress, xt, yt);

                movement_for_form(xt, yt);
                propertys_of_ke(xt, yt);
            }
        }
        public static Vector3 UnProject(Vector3 mouse, Matrix4 projection, Matrix4 view, Size viewport)
        {
            Vector4 vec;
            /*Console.WriteLine(projection.M11 + " " + projection.M12 + " " + projection.M13 + " " + projection.M14);
            Console.WriteLine(projection.M21 + " " + projection.M22 + " " + projection.M23 + " " + projection.M24);
            Console.WriteLine(projection.M31 + " " + projection.M32 + " " + projection.M33 + " " + projection.M34);
            Console.WriteLine(projection.M41 + " " + projection.M42 + " " + projection.M43 + " " + projection.M44);
            Console.WriteLine(view.M11 + " " + view.M12 + " " + view.M13 + " " + view.M14);
            Console.WriteLine(view.M21 + " " + view.M22 + " " + view.M23 + " " + view.M24);
            Console.WriteLine(view.M31 + " " + view.M32 + " " + view.M33 + " " + view.M34);
            Console.WriteLine(view.M41 + " " + view.M42 + " " + view.M43 + " " + view.M44);
            */

            float x = 2.0f * mouse.X / (float)viewport.Width - 1;
            float y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
            // Console.WriteLine(x + " " + y);
            vec.X = x;
            vec.Y = y;
            vec.Z = mouse.Z;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > 0.000001f || vec.W < -0.000001f)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            vec.X += 1;

            vec *= Matrix4.CreateRotationY((float) -(Math.PI / 3));

            return vec.Xyz;
        }

        public void onMouseDown(MouseEventArgs e)//нажали на мышку
        {
            /*mouseDown = true;
            oldX = x;
            oldY = y;*/
            if (type1 == 2)
            {
                int y = AnT.Height - e.Y;
                double xt = (e.X - x0) / (type1 == 2 ? scale2d : scale);
                double yt = (y - y0) / (type1 == 2 ? scale2d : scale);

                singlePointX = e.X;
                singlePointY = y;
                showCoordinates2d(xt, yt);
            }
            else
            {
                float x = e.X;
                float y = e.Y;
                int[] viewport = new int[4];
                Matrix4 modelMatrix, projMatrix;

                GL.GetFloat(GetPName.ModelviewMatrix, out modelMatrix);
                GL.GetFloat(GetPName.ProjectionMatrix, out projMatrix);
                GL.GetInteger(GetPName.Viewport, viewport);

                Vector3 _start = UnProject(new Vector3(x, y, 0.0f), cam.matrix, modelMatrix, new Size(viewport[2], viewport[3]));
                Vector3 _end = UnProject(new Vector3(x, y, 1.0f), cam.matrix, modelMatrix, new Size(viewport[2], viewport[3]));
                Vector3 vector = _end - _start;
                float k = -_start.Y / vector.Y;
                Vector3 result = _start + (k * vector);
                result /= 0.01f;
                showPoint = true;
                point = result;
                


            // Pass coordinates of point to a_Position
                showCoordinates2d(-result.X, result.Z);
            }

            if (e.Button == MouseButtons.Left)
                cam.mouseRotate = true; //Если нажата левая кнопка мыши

            if (e.Button == MouseButtons.Middle)
            {
                cam.mouseMove = true; //Если нажата средняя кнопка мыши
                mouseMove = true;
                myMouseXcoordVar = e.Y;
            }

            cam.myMouseYcoord = e.X; //Передаем в нашу глобальную переменную позицию мыши по Х
            cam.myMouseXcoord = e.Y;

            mouseX = e.X;
            mouseY = e.Y;

            tempX0 = x0;
            tempY0 = y0;

            myMouseXcoord = e.Y;
        }

        public void onMouseUp()//отпустили мышку
        {
            //MessageBox.Show(elements.all_elements.Count.ToString());
            //mouseDown = false;
            cam.mouseRotate = false;
            cam.mouseMove = false;
            mouseMove = false;
        }

        public void onMouseMove(int x, int y)//двигаем мышку
        {
            /*if (mouseDown)
            {
                yAngle += (x - oldX) * 0.2f;
                xAngle += (y - oldY) * 0.2f;
                oldX = x;
                oldY = y;
            }*/

            if (type1 == 2)
            {
                if (cam.mouseRotate)
                {
                    shift2d(x, y);
                }
            }

            cam.myMouseXcoordVar = y;
            cam.myMouseYcoordVar = x;
            myMouseXcoord = y;



        }

        public void shift2d(int x, int y)//двигаем 2д
        {
            x0 = tempX0 + x - mouseX;
            y0 = tempY0 - y + mouseY;
        }

        public void setType1(int type)//задаем первый тип
        {
            setType1(type, true);
        }

        public void setType1(int type, bool animationInit)
        {
            int temp = type1;
            if (animationInit)
            {
                animation.init(type, type2);
            }
            type1 = type;
            if (type == 2)
            {
                init2d();
            }
            else
            {
                if (temp == 2)
                {
                    init3d();
                }
            }
        }

        public void setType2(int type)//задаем второй тип
        {
            if (type1 == 2)
            {
                init3d();
            }
            animation.init(3, type);
        }

        public void getColorStress(int type)//вычисляем цвет
        {
            /*colorType = type;
            data.getColorStress(type);
            Form3.form3.label1.Text = data.colorStressMax.ToString();
            Form3.form3.label2.Text = data.colorStressMin.ToString();
            //Form1.form.label1.Text = data.colorStressMax.ToString();
            //Form1.form.label2.Text = data.colorStressMin.ToString();

            //int pos = Form1.form.pictureBox1.Top;
            int pos = Form3.form3.pictureBox1.Top;
            double r = Form3.form3.pictureBox1.Height / (data.colorStressMax - data.colorStressMin);
            pos += Convert.ToInt32(r * data.colorStressMax);
           // double r = Form1.form.pictureBox1.Height/(data.colorStressMax - data.colorStressMin);
            //pos += Convert.ToInt32(r * data.colorStressMax);
            Form3.form3.label3.Top = pos;
            Form3.form3.setPictureBox();

            //Form1.form.label3.Top = pos;
            //Form1.form.setPictureBox();*/
            colorType = type;
            if (type == -1)
                return;
            data.getColorStress(type);
            Form3.form3.label1.Text = Math.Round(data.colorStressMax, 2).ToString();
            Form3.form3.label2.Text = Math.Round(data.colorStressMin, 2).ToString();

            int pos = Form3.form3.pictureBox1.Top;

            double r = Form3.form3.pictureBox1.Height / (data.colorStressMax - data.colorStressMin);
            pos += Convert.ToInt32(r * data.colorStressMax);

            Form3.form3.label3.Top = pos;
            Form3.form3.label4.Top = Form3.form3.pictureBox1.Top + Convert.ToInt32(r * (data.colorStressMax / 3.0));
            Form3.form3.label5.Top = Form3.form3.pictureBox1.Top + Convert.ToInt32(r * (data.colorStressMax * 2 / 3.0));
            Form3.form3.label6.Top = Form3.form3.pictureBox1.Top + Convert.ToInt32(r * (data.colorStressMax - data.colorStressMin / 3.0));
            Form3.form3.label7.Top = Form3.form3.pictureBox1.Top + Convert.ToInt32(r * (data.colorStressMax - data.colorStressMin * 2 / 3.0));

            Form3.form3.label4.Text = Convert.ToString(Convert.ToInt32(data.colorStressMax * 2 / 3.0));
            Form3.form3.label5.Text = Convert.ToString(Convert.ToInt32(data.colorStressMax / 3.0));
            Form3.form3.label6.Text = Convert.ToString(Convert.ToInt32(data.colorStressMin / 3.0));
            Form3.form3.label7.Text = Convert.ToString(Convert.ToInt32(data.colorStressMin * 2 / 3.0));

            Form3.form3.setPictureBox();
            Form3.form3.setPictureBox();
            Form3.form3.setPictureBox();
        }

        public Double getStartHeight()//высота начальная
        {
            return Data.mass_of_material[0l].TS * 10;
        }

        public void keyPressed(KeyEventArgs e)//нажата клавища
        {


            if (e.KeyCode == Keys.W)
                y0 -= 1;

            if (e.KeyCode == Keys.S)
                y0 += 1;

            if (e.KeyCode == Keys.A)
                x0 += 1;

            if (e.KeyCode == Keys.D)
                x0 -= 1;

            if (e.KeyCode == Keys.Q)
                scale2d += 0.1f;

            if (e.KeyCode == Keys.E)
                scale2d -= 0.1f;
            


            /*
             if (e.Button == MouseButtons.Left)
                cam.mouseRotate = true; //Если нажата левая кнопка мыши

            if (e.Button == MouseButtons.Middle)
                cam.mouseMove = true; //Если нажата средняя кнопка мыши
                */
        }

        public void startAnimationXY()//начинаем анимацию
        {
            animationXY.init();
            animationXY.Start();
        }

        public void endAnimationXY()
        {
            animationXY.Stop();
        }

        public void setDrawNumberOfElement(bool q)
        {
            drawNumberOfElement = q;
        }

        public void setDrawNumberOfNode(bool q)
        {
            drawNumberOfNode = q;
        }

        public void setDrawNumberOfProperty(bool q)
        {
            drawNumberOfProperty = q;
        }

        public void setShowBounds(bool show)
        {
            showBounds = show;
        }

        public void setShowForce(bool show)
        {
            showForce = show;
        }

        public void setShowBlink(bool show)
        {
            showBlink = show;
        }

        public void setShowZones(bool show)
        {
            showZones = show;
        }

        public void setForceScale(double scale)
        {
            forceScale = scale;
        }

        public void setShowYellow(bool q)
        {
            showYellow = q;
        }

        public void setZeroZone(Double zone)
        {
            ZeroZone = zone;
        }

        public Double getZeroZone()
        {
            return ZeroZone;
        }




        public void cps_m_stress_for_form(double x, double y)
        {
            Int64 elem;
            element el;
            byte[] count = new byte[3];
            double[] k = new double[3];
            node[] node_n = new node[3];
            double[,] strain = new double[3, 6];
            double rx, ry;
            byte i, j;
            double[] st = new double[6];


            elem = data.find_el_by_point(x, y);
            el = data.find_el_by_number(elem);
            node_n[0] = data.temp_nodes.getnode(el.node1 - 1);
            node_n[1] = data.temp_nodes.getnode(el.node2 - 1);
            node_n[2] = data.temp_nodes.getnode(el.node3 - 1);

            for (i = 0; i != 3; ++i)
            {
                for (j = 0; j != 6; ++j)
                {
                    strain[i, j] = 0;
                    count[i] = 0;
                }

            }

            foreach (element ele in data.temp_elem.all_elements)
            {


                for (i = 0; i != 3; ++i)
                {
                    if (ele.node1 == node_n[i].number || ele.node2 == node_n[i].number || ele.node3 == node_n[i].number)
                    {
                        //textBox13.Text = ele.number.ToString();

                        for (j = 0; j != 6; ++j)
                        {
                            strain[i, j] = strain[i, j] + ele.stress[j];


                        }
                        ++count[i];
                        //textBox13.Text = count[0].ToString();
                    }
                }//if


            }//fr



            for (j = 0; j != 6; ++j)
            {
                for (i = 0; i != 3; ++i)
                {
                    strain[i, j] = mydiv(strain[i, j], count[i]);
                }
                rx = mydiv(node_n[0].x * strain[0, j] + node_n[1].x * strain[1, j] + node_n[2].x * strain[2, j], strain[0, j] + strain[1, j] + strain[2, j]);
                ry = mydiv(node_n[0].y * strain[0, j] + node_n[1].y * strain[1, j] + node_n[2].y * strain[2, j], strain[0, j] + strain[1, j] + strain[2, j]);

                for (i = 0; i != 3; ++i)
                {
                    k[i] = mydiv(Math.Sqrt((Math.Pow((rx - node_n[i].x), 2) + Math.Pow((ry - node_n[i].y), 2))), Math.Sqrt(Math.Pow((x - node_n[i].x), 2) + Math.Pow((y - node_n[i].y), 2)));
                }
                st[j] = mydiv(strain[0, j] * k[0] + strain[1, j] * k[1] + strain[2, j] * k[2], k[0] + k[1] + k[2]);
            }

            Form2.form2.textBox3.Text = Math.Round(st[0], 2).ToString();
            Form2.form2.textBox4.Text = Math.Round(st[1], 2).ToString();
            Form2.form2.textBox5.Text = Math.Round(st[2], 2).ToString();
            Form2.form2.textBox6.Text = Math.Round(st[3], 2).ToString();
            Form2.form2.textBox7.Text = Math.Round(st[4], 2).ToString();
            Form2.form2.textBox8.Text = Math.Round(st[5], 2).ToString();

            double iii = data.find_el_by_point(x, y);
            element eeel = data.find_el_by_number(iii);
            Form2.form2.textBox9.Text = Math.Round(el.stress[6], 2).ToString();


        }

        public void cm_m_stress_for_form(double x, double y)
        {
            Int64 elem;
            element el;
            byte[] count = new byte[3];
            double[] k = new double[3];
            node[] node_n = new node[3];
            double[,] strain = new double[3, 6];
            double rx, ry;
            byte i, j;
            double[] st = new double[6];


            elem = data.find_el_by_point(x, y);
            el = data.find_el_by_number(elem);
            node_n[0] = data.temp_nodes.getnode(el.node1 - 1);
            node_n[1] = data.temp_nodes.getnode(el.node2 - 1);
            node_n[2] = data.temp_nodes.getnode(el.node3 - 1);

            for (i = 0; i != 3; ++i)
            {
                for (j = 0; j != 6; ++j)
                {
                    strain[i, j] = 0;
                    count[i] = 0;
                }

            }

            foreach (element ele in data.temp_elem.all_elements)
            {


                for (i = 0; i != 3; ++i)
                {
                    if (ele.node1 == node_n[i].number || ele.node2 == node_n[i].number || ele.node3 == node_n[i].number)
                    {
                        //textBox13.Text = ele.number.ToString();

                        for (j = 0; j != 6; ++j)
                        {
                            strain[i, j] = strain[i, j] + ele.stress[j];


                        }
                        ++count[i];
                        //textBox13.Text = count[0].ToString();
                    }
                }//if


            }//fr

            rx = (node_n[0].x + node_n[1].x + node_n[2].x) / 3;
            ry = (node_n[0].y + node_n[1].y + node_n[2].y) / 3;

            for (j = 0; j != 6; ++j)
            {
                for (i = 0; i != 3; ++i)
                {
                    strain[i, j] = mydiv(strain[i, j], count[i]);
                }

                for (i = 0; i != 3; ++i)
                {
                    k[i] = mydiv(Math.Sqrt((Math.Pow((rx - node_n[i].x), 2) + Math.Pow((ry - node_n[i].y), 2))), Math.Sqrt(Math.Pow((x - node_n[i].x), 2) + Math.Pow((y - node_n[i].y), 2)));
                }
                st[j] = mydiv(strain[0, j] * k[0] + strain[1, j] * k[1] + strain[2, j] * k[2], k[0] + k[1] + k[2]);
            }
            /*Form1.form.t
            textBox13.Text = Math.Round(st[0], 2).ToString();
            textBox14.Text = Math.Round(st[1], 2).ToString();
            textBox15.Text = Math.Round(st[2], 2).ToString();
            textBox16.Text = Math.Round(st[3], 2).ToString();
            textBox17.Text = Math.Round(st[4], 2).ToString();
            textBox18.Text = Math.Round(st[5], 2).ToString();*/
            Form2.form2.textBox3.Text = Math.Round(st[0], 2).ToString();
            Form2.form2.textBox4.Text = Math.Round(st[1], 2).ToString();
            Form2.form2.textBox5.Text = Math.Round(st[2], 2).ToString();
            Form2.form2.textBox6.Text = Math.Round(st[3], 2).ToString();
            Form2.form2.textBox7.Text = Math.Round(st[4], 2).ToString();
            Form2.form2.textBox8.Text = Math.Round(st[5], 2).ToString();

            double iii = data.find_el_by_point(x, y);
            element eeel = data.find_el_by_number(iii);
            Form2.form2.textBox9.Text = Math.Round(el.stress[6], 2).ToString();
        }


        public void ke_m_stress_for_form(double x, double y)
        {
            double i = data.find_el_by_point(x, y);
            element el = data.find_el_by_number(i);

            Form2.form2.textBox3.Text = Math.Round(el.stress[0], 2).ToString();
            Form2.form2.textBox4.Text = Math.Round(el.stress[1], 2).ToString();
            Form2.form2.textBox5.Text = Math.Round(el.stress[2], 2).ToString();
            Form2.form2.textBox6.Text = Math.Round(el.stress[3], 2).ToString();
            Form2.form2.textBox7.Text = Math.Round(el.stress[4], 2).ToString();
            Form2.form2.textBox8.Text = Math.Round(el.stress[5], 2).ToString();
            Form2.form2.textBox9.Text = Math.Round(el.stress[6], 2).ToString();
        }

        public double mydiv(double x, double y)
        {
            if (Math.Abs(y) < 0.1E-4900)
                return 0.1E-4900;
            else return x / y;
        }


        public void make_stress_for_form(string type, double x, double y)
        {
            if (type == "cpc")
            {
                cps_m_stress_for_form(x, y);
            }
            if (type == "cm")
            {
                cm_m_stress_for_form(x, y);
            }
            if (type == "ke")
            {
                ke_m_stress_for_form(x, y);
            }
        }




        public void movement_for_form(double x, double y)
        {
            Int64 num_of_el, i, j;
            element el;
            node[] node_n = new node[3];
            double[] dx = new double[3];
            double[] dy = new double[3];
            double[,] A = new double[3, 3];
            double[,] m = new double[3, 4];
            double[] u = new double[3];
            double[] v = new double[3];
            double uxy, vxy;
            double temp;
            bool solved = true;

            num_of_el = data.find_el_by_point(x, y);
            //richTextBox1.Text += num_of_el.ToString();
            //richTextBox1.Text += "\n";
            el = data.find_el_by_number(num_of_el);
            node_n[0] = data.temp_nodes.getnode(el.node1 - 1);
            node_n[1] = data.temp_nodes.getnode(el.node2 - 1);
            node_n[2] = data.temp_nodes.getnode(el.node3 - 1);

            for (i = 0; i != 3; ++i)
            {
                A[i, 0] = 1.0;
                A[i, 1] = node_n[i].x;
                //richTextBox1.Text += A[i,2].ToString();
                //richTextBox1.Text += "\n";

                A[i, 2] = node_n[i].y;
                //richTextBox1.Text += A[i, 3].ToString();
                //richTextBox1.Text += "\n";
                dx[i] = node_n[i].movX;
                //richTextBox1.Text += dx[i].ToString();
                //richTextBox1.Text += "\n";
                dy[i] = node_n[i].movY;
                //richTextBox1.Text += dy[i].ToString();
                //richTextBox1.Text += "\n";
            }

            for (i = 0; i != 3; ++i)
            {
                for (j = 0; j != 3; ++j)
                {
                    m[i, j] = A[i, j];
                }
                m[i, 3] = dx[i];
            }
            solved = solveGJ(m, 3);

            //


            //

            if (!solved)
            {

            }


            for (i = 0; i != 3; ++i)
            {
                u[i] = m[i, 3];
            }



            for (i = 0; i != 3; ++i)
            {
                for (j = 0; j != 3; ++j)
                {
                    m[i, j] = A[i, j];
                }
                m[i, 3] = dy[i];
            }
            solved = solveGJ(m, 3);

            if (!solved)
            {

            }


            for (i = 0; i != 3; ++i)
            {
                v[i] = m[i, 3];
            }


            uxy = u[0] + u[1] * x + u[2] * y;
            vxy = v[0] + v[1] * x + v[2] * y;

            Form2.form2.textBox10.Text = uxy.ToString("0.###E+000");
            Form2.form2.textBox11.Text = vxy.ToString("0.###E+000");
            //textBox11.Text = uxy.ToString("0.###E+000");
            //textBox12.Text = vxy.ToString("0.###E+000");


        }//m

        public bool solveGJ(double[,] m, int nRow)
        {
            int i, j, k, maxRow;
            double c, temp;
            double eps;
            eps = 1E-10;
            for (i = 0; i != nRow; ++i)
            {
                maxRow = i;
                for (j = i + 1; j != nRow; ++j)
                {
                    if (Math.Abs(m[j, i]) > Math.Abs(m[maxRow, i]))
                        maxRow = j;
                }
                for (k = 0; k != nRow; ++k)
                {
                    temp = m[i, k];
                    m[i, k] = m[maxRow, k];
                    m[maxRow, k] = temp;
                }

                if (Math.Abs(m[i, i]) < eps)
                    return false;

                for (j = i + 1; j != nRow; ++j)
                {
                    c = m[j, i] / m[i, i];
                    for (k = i; k != nRow + 1; ++k)
                    {
                        m[j, k] = m[j, k] - m[i, k] * c;
                    }

                }
            }

            for (i = nRow - 1; i >= 0; --i)
            {
                c = m[i, i];
                for (j = 0; j != i; ++j)
                {
                    for (k = nRow; k >= 0; --k)
                    {
                        m[j, k] = m[j, k] - m[i, k] * m[j, i] / c;

                    }

                }
                m[i, i] = m[i, i] / c;
                m[i, nRow] = m[i, nRow] / c;
            }



            return true;

        }

        public void grid_with_force()
        {
            Form2.form2.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            Form2.form2.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Form2.form2.dataGridView1.ColumnCount = 6;
            Form2.form2.dataGridView1.Columns[0].Name = "№";
            Form2.form2.dataGridView1.Columns[1].Name = "X";
            Form2.form2.dataGridView1.Columns[2].Name = "Y";
            Form2.form2.dataGridView1.Columns[3].Name = "Force X";
            Form2.form2.dataGridView1.Columns[4].Name = "Force Y";
            Form2.form2.dataGridView1.Columns[5].Name = "Result";


            double fx = 0, fy = 0, f = 0;
            Form2.form2.dataGridView1.RowCount = data.node_with_fofce.Count + 1;
            int i = 0;
            foreach (node n in data.node_with_fofce)
            {
                //dataGridView1.Rows[0].Cells[0].Value = data.node_with_fofce.Count;
                Form2.form2.dataGridView1.Rows[i].Cells[0].Value = n.r_number;
                Form2.form2.dataGridView1.Rows[i].Cells[1].Value = Math.Round(n.x, 2);
                Form2.form2.dataGridView1.Rows[i].Cells[2].Value = Math.Round(n.y, 2);
                Form2.form2.dataGridView1.Rows[i].Cells[3].Value = Math.Round(n.forceX, 2);
                Form2.form2.dataGridView1.Rows[i].Cells[4].Value = Math.Round(n.forceY, 2);
                Form2.form2.dataGridView1.Rows[i].Cells[5].Value = Math.Round((Math.Sqrt(Math.Pow((n.forceY), 2) + Math.Pow((n.forceX), 2))), 2);
                f += Math.Sqrt(Math.Pow((n.forceY), 2) + Math.Pow((n.forceX), 2));
                fx += n.forceX;
                fy += n.forceY;
                ++i;
            }
            //Form2.form2.dataGridView1.Rows[i].Cells[0].Value = null;
            //Form2.form2.dataGridView1.Rows[i].Cells[1].Value = null;
            //Form2.form2.dataGridView1.Rows[i].Cells[2].Value = null;
            Form2.form2.dataGridView1.Rows[i].Cells[3].Value = Math.Round(fx, 2);
            Form2.form2.dataGridView1.Rows[i].Cells[4].Value = Math.Round(fy, 2);
            Form2.form2.dataGridView1.Rows[i].Cells[5].Value = Math.Round(f, 2);

        }


        public void propertys_of_ke(double x, double y)
        {
            double i = data.find_el_by_point(x, y);
            element el = data.find_el_by_number(i);
            //textBox4.Text = Data.xxx.ToString();
            Form2.form2.textBox12.Text = el.number.ToString();
            Form2.form2.textBox13.Text = Math.Round(Data.mass_of_material[el.material - 1].E, 2).ToString();
            Form2.form2.textBox14.Text = Math.Round(Data.mass_of_material[el.material - 1].MU, 2).ToString();
            Form2.form2.textBox15.Text = Math.Round(Data.mass_of_material[el.material - 1].SG, 2).ToString();
            Form2.form2.textBox16.Text = Math.Round(Data.mass_of_material[el.material - 1].TS, 2).ToString();
            //openfile_res2();
            //aaa = 7200000000;
            //richTextBox1.Text += Data.mass_of_material[0].E.ToString();
            //richTextBox1.Text += aaa;
            //richTextBox1.Text += "&";

            //richTextBox1.Text += "\n";
            /*textBox5.Text = Math.Round(Data.mass_of_material[el.material].E, 2).ToString();
            textBox6.Text = Math.Round(Data.mass_of_material[el.material].MU, 2).ToString();
            textBox7.Text = Math.Round(Data.mass_of_material[el.material].SG, 2).ToString();
            textBox8.Text = Math.Round(Data.mass_of_material[el.material].TS, 2).ToString();*/
        }

        private void drawSphere(double x0, double y0, double z0, double r)
        {
            const float PI = 3.141592f;
            double x, y, z, alpha, beta; // Storage for coordinates and angles        
            double radius = r;
            int gradation = 20;

            // Console.WriteLine("Sphere: " + x0 + " " + y0 + " " + z0);
            for (alpha = 0.0; alpha < PI; alpha += PI / gradation)
            {
                GL.Begin(BeginMode.TriangleStrip);
                for (beta = 0.0; beta < 2.01 * PI; beta += PI / gradation)
                {
                    x = radius * Math.Cos(beta) * Math.Sin(alpha) + x0;
                    y = radius * Math.Sin(beta) * Math.Sin(alpha) + y0;
                    z = radius * Math.Cos(alpha) + z0;
                    GL.Vertex3(x, y, z);
                    x = radius * Math.Cos(beta) * Math.Sin(alpha + PI / gradation) + x0;
                    y = radius * Math.Sin(beta) * Math.Sin(alpha + PI / gradation) + y0;
                    z = radius * Math.Cos(alpha + PI / gradation) + z0;
                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
        }


        // note: qreal is a typedef of either float or double
        private void drawCone(double rd, double h, int n)
        {
            double angInc = 360.0 / n * Math.PI;

            // draw cone top
            GL.Begin(BeginMode.TriangleFan);
            for (int i = 0; i < n; ++i)
            {
                GL.Vertex3(rd * Math.Cos(angInc * i), rd * Math.Cos(angInc * i), h);
            }
            GL.End();

            // draw cone bottom
            GL.Begin(BeginMode.TriangleFan);
            GL.Begin(BeginMode.TriangleFan);
            for (int i = 0; i < n; ++i)
            {
                GL.Vertex3(rd * Math.Cos(angInc * i), rd * Math.Cos(angInc * i), 0);
            }
            GL.End();
        }

        public void updateAnimation(int speed)
        {
            animation.Dir = 1.0 / (1.0 + (11 - speed));
            animationXY.Dir = 1.0 / (1.0 + (11 - speed));
        }

        ~Simulation() 
        {
            textRenderer.Dispose();
        }

        public MyCamera MyCamera
        {
            get => default;
            set
            {
            }
        }

        public AnimationXY Animation_2D
        {
            get => default;
            set
            {
            }
        }

        public Animation Animation_3D
        {
            get => default;
            set
            {
            }
        }

        public Data Data
        {
            get => default;
            set
            {
            }
        }
    }
}
