using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Engine.GLApi;
using OpenTK.Graphics;
using OpenTK.Platform;
using Engine.Core;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Engine.Processing;
using System.Diagnostics;

namespace VertexBenderCS
{
    public partial class MainWin : Form
    {
        private Camera _camera;
        private CameraController _cameraController;

        private bool _isFirstMouse;
        private float _mouseX;
        private float _mouseY;

        private System.Timers.Timer _Timer;
        
        //TEST STUFF
        private Shader _wiredShader;
        private Shader _shader;

        private List<MeshRenderer> _objects;
        private List<MeshRenderer> _samplePointRenderers;
        private List<LineRenderer> _lines;

        private List<Vector3> _sampleCoords; 

        public MainWin()
        {
            InitializeComponent();
        }

        private Keys KeyState { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.FromArgb(25, 25, 25));
            GL.Enable(EnableCap.DepthTest);

            Text = "Vertex Bender Framework - " +
                    //GL.GetString(StringName.Vendor) + " " +
                    GL.GetString(StringName.Renderer);/*+ " " +
                    GL.GetString(StringName.Version);*/

            GLControl_Resize(GLControl, EventArgs.Empty);

            _mouseX = (int)(Width * 0.5);
            _mouseY = (int)(Height * 0.5);
            _isFirstMouse = true;

            _Timer = new System.Timers.Timer(8.0f);
            _Timer.Elapsed += Update;
            _Timer.Start();

            SubscribeEvents();
            SetupTestScene();

            _src = new int?(int.Parse(txtSource.Text));
            _trg = new int?(int.Parse(txtTarget.Text));

            
        }

        private void Update(object sender, System.Timers.ElapsedEventArgs e)
        {
            GLControl.Invalidate();

            if (KeyState == Keys.Q)
            {
                _cameraController.Navigate(eCameraMovement.up);
            }
            if (KeyState == Keys.E)
            {
                _cameraController.Navigate(eCameraMovement.down);
            }
            if (KeyState == Keys.W)
            {
                _cameraController.Navigate(eCameraMovement.forward);
            }
            if (KeyState == Keys.S)
            {
                _cameraController.Navigate(eCameraMovement.backward);
            }
            if (KeyState == Keys.A)
            {
                _cameraController.Navigate(eCameraMovement.left);
            }
            if (KeyState == Keys.D)
            {
                _cameraController.Navigate(eCameraMovement.right);
            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //Application.Idle -= Application_Idle;

            base.OnClosing(e);
        }

        public void SubscribeEvents()
        {
            GLControl.KeyDown += new KeyEventHandler(GLControl_KeyDown);
            GLControl.KeyUp += new KeyEventHandler(GLControl_KeyUp);
            GLControl.Resize += new EventHandler(GLControl_Resize);
            GLControl.Paint += new PaintEventHandler(GLControl_Paint);
            GLControl.MouseDown += GLControl_MouseDown;
            GLControl.MouseMove += GLControl_MouseMove;
            GLControl.MouseWheel += GLControl_MouseWheel;
            //Application.Idle += Application_Idle;

            menuImport.Click += MenuImport_Click;


            btnGeodesicMatrix.Click += BtnGeodesicMatrix_Click;
            btnDijkstra.Click += BtnDijkstra_Click;
            btnFPS.Click += BtnFPS_Click;

            txtSource.TextChanged += TxtSource_TextChanged;
            txtTarget.TextChanged += TxtTarget_TextChanged;
        }



        private void MenuImport_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog();
            d.ValidateNames = true;
            d.Filter = "Off files (*.off)|*.off|All Files(*.*)|*.* ";
            if (d.ShowDialog() == DialogResult.OK)
            {
                ////////// TEMP ///////
                _objects.Clear();
                _lines.Clear();
                _samplePointRenderers.Clear();
                _sampleCoords.Clear();
                ///////////////////////
                var v = d.FileName.Substring(d.FileName.Length - 4);
                if (v.ToLower() == ".off")
                {
                    var obj = new MeshRenderer(ObjectLoader.LoadOff(d.FileName));
                    _objects.Add(obj);
                }
            }
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Back);

            //Matrix4 model = Matrix4.CreateScale(50.0f);
            //Matrix4 rotation = Matrix4.CreateRotationX(-MathHelper.Pi / 2);
            //model = model * rotation;
            Matrix4 model = Matrix4.Identity;

            foreach (var obj in _objects)
            {
                obj.Shader = _shader;
                obj.Shader.Use();
                obj.Shader.SetVec3("cameraPosition", _camera.Position);
                obj.Shader.SetVec3("directLight.direction", -0.2f, -1.0f, 0.0f);
                obj.Shader.SetVec3("directLight.ambient", 0.25f, 0.25f, 0.25f);
                obj.Shader.SetVec3("directLight.diffuse", 0.5f, 0.5f, 0.4f);
                obj.Shader.SetVec3("directLight.specular", 0.1f, 0.1f, 0.1f);
                obj.Shader.SetFloat("material.shineness", 2.0f);
                obj.Shader.SetMat4("Model", model);
                obj.Shader.SetMat4("View", _camera.View);
                obj.Shader.SetMat4("Projection", _camera.Projection);
                obj.Render(eRenderMode.shaded);
            }

            for (int i = 0; i < _samplePointRenderers.Count; i++)
            {
                var obj = _samplePointRenderers[i];
                //model = Matrix4.CreateScale(50.0f);
                //rotation = Matrix4.CreateRotationX(-MathHelper.Pi / 2);
                //model = Matrix4.CreateTranslation(_sampleCoords[i]) * rotation * model;
                model = Matrix4.CreateTranslation(_sampleCoords[i]);

                var color = new Vector4(_sampleCoords[i]);

                obj.Shader = _wiredShader;
                obj.Shader.Use();
                obj.Shader.SetMat4("Model", model);
                obj.Shader.SetMat4("View", _camera.View);
                obj.Shader.SetMat4("Projection", _camera.Projection);
                obj.Shader.SetVec4("Color", color);
                obj.Shader.SetMat4("Model", model);
                obj.Shader.SetMat4("View", _camera.View);
                obj.Shader.SetMat4("Projection", _camera.Projection);
                obj.Render(eRenderMode.shaded);
            }

            model = Matrix4.Identity;
            foreach (var line in _lines)
            {
                line.Shader = _wiredShader;
                line.Shader.Use();
                line.Shader.SetMat4("Model", model);
                line.Shader.SetMat4("View", _camera.View);
                line.Shader.SetMat4("Projection", _camera.Projection);
                line.Shader.SetVec4("Color", line.Color);
                line.Render();
            }


            GL.Flush();
            GLControl.SwapBuffers();
        }

        private void GLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            _cameraController.Zoom(e.Delta);
        }

        private void GLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isFirstMouse)
            {
                _mouseX = (float)e.X;
                _mouseY = (float)e.Y;
                _isFirstMouse = false;
            }

            float offsetX = e.X - _mouseX;
            float offsetY = _mouseY - e.Y;


            if (e.Button == MouseButtons.Middle)
            {
                _cameraController.Pan(offsetX, offsetY);
            }
            else if (e.Button == MouseButtons.Right && (KeyState & Keys.ControlKey) == Keys.ControlKey)
            {
                _cameraController.OrbitAround(offsetX , offsetY , Vector3.Zero);
            }
            else if(e.Button == MouseButtons.Right)
            {
                _cameraController.Rotate(offsetX , offsetY);
            }
            _mouseX = e.X;
            _mouseY = e.Y;

            statusBar.Text = "X: " + _camera.Position.X + "\n" + "Y: " + _camera.Position.Y + "Z: " + _camera.Position.Z;
            //statusBar.Text = KeyState.ToString();
        }

        private void GLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks > 1)
            {
                _cameraController.Reset();
            }

        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
            {
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);
            }

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);

            if (_camera != null)
            {
                _camera.AspectRatio = Width / (float)Height;
            }
        }

        private void GLControl_KeyDown(object sender, KeyEventArgs e)
        {
            KeyState = e.KeyData;
        }

        private void GLControl_KeyUp(object sender, KeyEventArgs e)
        {
            KeyState = Keys.None;
            if (e.KeyCode == Keys.F12)
            {
                GrabScreenshot().Save(@"C:\users\ozgun\desktop\screenshot.png");
            }
        }

        private void GLControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        Bitmap GrabScreenshot()
        {
            Bitmap bmp = new Bitmap(GLControl.Width, GLControl.Height);
            System.Drawing.Imaging.BitmapData data =
            bmp.LockBits(GLControl.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, GLControl.ClientSize.Width, GLControl.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte,
                data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        // TEST REGION
        private void SetupTestScene()
        {
            _objects = new List<MeshRenderer>();
            _lines = new List<LineRenderer>();
            _samplePointRenderers = new List<MeshRenderer>();
            _sampleCoords = new List<Vector3>();

            _camera = new Camera(GLControl.Width, GLControl.Height);
            _cameraController = new CameraController(_camera);

            int idv = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\ModelLoadingVertexShaderWithLight.glsl", ShaderType.VertexShader);
            int idf = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\ModelLoadingFragmentShaderLighted.glsl", ShaderType.FragmentShader);
            _shader = ShaderBuilder.CreateShader("shaded", idv, idf);

            idv = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\WireframeVertex.glsl", ShaderType.VertexShader);
            idf = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\WireframeFragment.glsl", ShaderType.FragmentShader);
            _wiredShader = ShaderBuilder.CreateShader("wired", idv, idf);


            _objects.Clear();
            //var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\fprint matrix\horse0.off"));
            var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\fprint matrix\man0.off"));
            //var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\timing\centaur.off"));
            //var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\woman.off"));
            _objects.Add(mesh);

            //idv = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\smoothLineVertex.glsl", ShaderType.VertexShader);
            //idf = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\smoothLineFragment.glsl", ShaderType.FragmentShader);
            //_lineShader = ShaderBuilder.CreateShader("smoothLine", idv, idf);


            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\MyDemo\0.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\timing\centaur.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\timing\weirdSphere.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\timing\man.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\fprint matrix\horse0.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\fprint matrix\man0.off");


            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\cat.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\centaur.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\gorilla.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\man2.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\man3.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\man4.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\neptune.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\2) use for the other tasks\woman.off");
            //var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\Desktop\examples\64.off");



            //var lines = new List<Vector3>();
            //int i = 0;
            //var prime = mesh.Vertices[0];
            //while (i < 100)
            //{
            //    prime = mesh.Vertices[prime.Verts[i % prime.Verts.Count]];
            //    lines.Add(prime.Coord);
            //    i++;
            //}

            //_mesh = mesh;
            //var mesh = Engine.Core.ObjectLoader.CreateCube(0.1f);
            //_meshRenderer = new MeshRenderer(mesh);
            //_mesh.Shader = _shader;

            //var a = Dijkstra.ConstructGraphFromMesh(mesh);

            //Dijkstra.DijkstraArray(a, 0, 111, out int[] path);

            //List<Vector3> lines = new List<Vector3>();

            //for (int i = 0; i < path.Length; i++)
            //{
            //    lines.Add(mesh.Vertices[path[i]].Coord);
            //}

            //_lineRenderer = new LineRenderer(lines);
        }

        private int? _src = null;
        private int? _trg = null;

        private void TxtTarget_TextChanged(object sender, EventArgs e)
        {
            var text = (sender as MaskedTextBox).Text;

            if (text != string.Empty)
            {
                _trg = new int?(int.Parse(text));
            }

        }

        private void TxtSource_TextChanged(object sender, EventArgs e)
        {
            var text = (sender as MaskedTextBox).Text;

            if (text != string.Empty)
            {
                _src = new int?(int.Parse(text));
            }
        }

        private void BtnDijkstra_Click(object sender, EventArgs e)
        {
            if (_src.HasValue && _trg.HasValue)
            {
                _lines.Clear();
                Stopwatch watch = new Stopwatch();
                Graph g = new Graph(_objects[0].Mesh);

                Log.AppendText("\n starting: \n");

                //var b = Algorithm.ConstructGraphFromMesh(_objects[0].Mesh);
                watch.Start();
                //var d1 = Algorithm.DijkstraArray(b, _src.Value, _trg.Value, out List<int> path);
                watch.Stop();
                var a1 = watch.ElapsedMilliseconds;
                //Log.AppendText("Array: " + d1.ToString() + "   , elapsed: " + a1 + "\n");

                watch.Reset();
                
                watch.Start();
                var d2 = Algorithm.DijkstraMinHeap(g, _src.Value, _trg.Value, out List<int> path2);
                watch.Stop();
                var a2 = watch.ElapsedMilliseconds;
                Log.AppendText("Min Heap: " + d2.ToString() + "   , elapsed: " + a2 + "\n");

                watch.Reset();

                watch.Start();
                var d3 = Algorithm.DijkstraFibonacciHeap(g, _src.Value, _trg.Value, out List<int> path3);
                watch.Stop();
                var a3 = watch.ElapsedMilliseconds;
                Log.AppendText("Fibonacci Heap: " + d3.ToString() + "   , elapsed: " + a3);

                watch.Reset();

                List<Vector3> lines1 = new List<Vector3>();
                List<Vector3> lines2 = new List<Vector3>();
                List<Vector3> lines3 = new List<Vector3>();

                //for (int i = 0; i < path.Count; i++)
                //{
                //    lines1.Add(_objects[0].Mesh.Vertices[path[i]].Coord);
                //}

                for (int i = 0; i < path2.Count; i++)
                {
                    lines2.Add(_objects[0].Mesh.Vertices[path2[i]].Coord);
                }

                //for (int i = 0; i < path2.Count; i++)
                //{
                //    lines3.Add(_objects[0].Mesh.Vertices[path2[i]].Coord);
                //}


                var r1 = new LineRenderer(lines1);
                r1.Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                var r2 = new LineRenderer(lines2);
                r2.Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                var r3 = new LineRenderer(lines3);
                r3.Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                _lines.Add(r1);
                _lines.Add(r2);
                _lines.Add(r3);

            }
        }

        private void BtnGeodesicMatrix_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            Graph g = new Graph(_objects[0].Mesh);

            watch.Start();
            var matrix = Algorithm.CreateLinearGeodesicDistanceMatrix(g);
            ProcessOutputHandler.CreateBitmapGeodesicDistance(matrix, @"C:\users\ozgun\desktop\out");
            watch.Stop();
            var a4 = watch.ElapsedMilliseconds;
            Log.AppendText("output created" + ", elapsed: " + a4);

        }

        private void BtnFPS_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            Graph g = new Graph(_objects[0].Mesh);
            //UndirectedGraph g = new UndirectedGraph(_objects[0].Mesh);
            _samplePointRenderers.Clear();

            watch.Start();
            //var samples = Algorithm.FarthestPointSampling(g, 10);
            var samples = Algorithm.FarthestPointSampling(g, 10);
            //ProcessOutputHandler.CreateBitmapGeodesicDistance(matrix, @"C:\users\ozgun\desktop\out");
            watch.Stop();
            var a4 = watch.ElapsedMilliseconds;
            Log.AppendText("\n output created" + ", elapsed: " + a4);

            for (int i = 0; i < samples.Count ; i++)
            {
                MeshRenderer obj = new MeshRenderer(ObjectLoader.CreateCube(0.05f));
                _sampleCoords.Add(samples[i].Coord);
                _samplePointRenderers.Add(obj);
            }
        }


        [STAThread]
        public static void Main()
        {
            using (MainWin example = new MainWin())
            {
                example.ShowDialog();
            }
        }


    }
}
