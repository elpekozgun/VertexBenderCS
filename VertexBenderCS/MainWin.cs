﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Engine.GLApi;
using Engine.Core;
using System.Collections.Generic;
using Engine.Processing;
using System.Diagnostics;
using System.Linq;

namespace VertexBenderCS
{
    public partial class MainWin : Form
    {
        private Camera              _camera;
        private CameraController    _cameraController;
        private SceneGraph          _SceneGraph;

        private bool                _isFirstMouse;
        private float               _mouseX;
        private float               _mouseY;
        private System.Timers.Timer _Timer;
        

        public MainWin()
        {
            InitializeComponent();
        }

        private Keys KeyState { get; set; }

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
            btnGauss.Click += BtnGauss_Click;
            btnAverageGeo.Click += BtnAverageGeo_Click;
            btnIsoCurve.Click += BtnIsoCurve_Click;

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
                var v = d.FileName.Substring(d.FileName.Length - 4);
                if (v.ToLower() == ".off")
                {
                    _SceneGraph.Clean();
                    var obj = new MeshRenderer(ObjectLoader.LoadOff(d.FileName));
                    _SceneGraph.AddObject(obj);
                }
            }
        }

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


            GL.ClearColor(Color.FromArgb(25, 25, 25));
            GL.Enable(EnableCap.DepthTest);

            Text = "Vertex Bender Framework - " +
                    //GL.GetString(StringName.Vendor) + " " +
                    GL.GetString(StringName.Renderer) + " " +
                    GL.GetString(StringName.Version);

            GLControl_Resize(GLControl, EventArgs.Empty);

            _mouseX = (int)(Width * 0.5);
            _mouseY = (int)(Height * 0.5);
            _isFirstMouse = true;

            _Timer = new System.Timers.Timer(8.0f);
            _Timer.Elapsed += Update;
            _Timer.Start();

            SubscribeEvents();
            SetupScene();

            _src = new int?(int.Parse(txtSource.Text));
            _trg = new int?(int.Parse(txtTarget.Text));




        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //Application.Idle -= Application_Idle;

            base.OnClosing(e);
        }

        #region GlControl Events

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

        #endregion

        private void SetupScene()
        {
            _SceneGraph = new SceneGraph();
            _camera = new Camera(GLControl.Width, GLControl.Height);
            _cameraController = new CameraController(_camera);

            //Start with a model
            var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\fprint matrix\man0.off"));
            _SceneGraph.AddObject(mesh);
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Back);

            _SceneGraph.RenderAll(_camera);

            GL.Flush();
            GLControl.SwapBuffers();
        }

        // TEST REGION

        #region TEST

        private Bitmap GrabScreenshot()
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
                Stopwatch watch = new Stopwatch();
                var mesh = (_SceneGraph.SceneItems[0] as MeshRenderer).Mesh;

                Log.AppendText("\n starting: \n");

                var start = _src.Value;
                var end = _trg.Value;

                //watch.Start();
                //var d1 = Algorithm.DijkstraArray(mesh, start, end);
                //watch.Stop();
                //var a1 = watch.ElapsedMilliseconds;
                //Log.AppendText("Array: " + d1.TargetDistance.ToString() + "   , elapsed: " + a1 + "\n");

                watch.Start();
                var d1 = Algorithm.AStarMinHeap(mesh, start, end);
                watch.Stop();
                var a1 = watch.ElapsedMilliseconds;
                Log.AppendText("AStar: " + d1.TargetDistance.ToString() + "   , elapsed: " + a1 + "\n");

                watch.Reset();

                watch.Start();
                var d2 = Algorithm.DijkstraMinHeap(mesh, start, end, false);
                watch.Stop();
                var a2 = watch.ElapsedMilliseconds;
                Log.AppendText("Min Heap: " + d2.TargetDistance.ToString() + "   , elapsed: " + a2 + "\n");

                watch.Reset();

                watch.Start();
                var d3 = Algorithm.DijkstraFibonacciHeap(mesh, start, end, false);
                watch.Stop();
                var a3 = watch.ElapsedMilliseconds;
                Log.AppendText("Fibonacci Heap: " + d3.TargetDistance.ToString() + "   , elapsed: " + a3);

                watch.Reset();

                List<Vector3> lines1 = new List<Vector3>();
                List<Vector3> lines2 = new List<Vector3>();
                List<Vector3> lines3 = new List<Vector3>();

                for (int i = 0; i < d1.Path.Count - 1; i++)
                {
                    lines1.Add(mesh.Vertices[d1.Path[i]].Coord);
                    lines1.Add(mesh.Vertices[d1.Path[i + 1]].Coord);
                }

                for (int i = 0; i < d3.Path.Count - 1; i++)
                {
                    lines2.Add(mesh.Vertices[d3.Path[i]].Coord);
                    lines2.Add(mesh.Vertices[d3.Path[i + 1]].Coord);
                }

                var r1 = new LineRenderer(lines1);
                r1.Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                var r2 = new LineRenderer(lines2);
                r2.Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                var r3 = new LineRenderer(lines3);
                r3.Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                _SceneGraph.AddObject(r1);
                _SceneGraph.AddObject(r2);
                _SceneGraph.AddObject(r3);

            }
        }

        private void BtnGeodesicMatrix_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            var matrix = Algorithm.CreateGeodesicDistanceMatrix((_SceneGraph.SceneItems[0] as MeshRenderer).Mesh);
            ProcessOutputHandler.CreateBitmapGeodesicDistance(matrix, @"C:\users\ozgun\desktop\out");
            watch.Stop();
            var a4 = watch.ElapsedMilliseconds;
            Log.AppendText("output created" + ", elapsed: " + a4);

        }

        private void BtnFPS_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            Graph g = new Graph((_SceneGraph.SceneItems[0] as MeshRenderer).Mesh);

            watch.Start();
            var samples = Algorithm.FarthestPointSampling(g, 100);
            watch.Stop();
                
            var a4 = watch.ElapsedMilliseconds;
            AppendLogSafe("\n output created" + ", elapsed: " + a4);

            for (int i = 0; i < samples.SampelIndices.Count; i++)
            {
                MeshRenderer obj = new MeshRenderer(PrimitiveObjectFactory.CreateCube(0.05f), Shader.DefaultUnlitShader);
                obj.Color = new Vector4(samples.SamplePoints[i].Coord * 0.5f, 1.0f);
                obj.Transform.Position = samples.SamplePoints[i].Coord;
                _SceneGraph.AddObject(obj);
            }
        }

        private void BtnGauss_Click(object sender, EventArgs e)
        {
            //var a = Algorithm.GaussianCurvature(_objects[0].Mesh);
            var obj = (_SceneGraph.SceneItems[0] as MeshRenderer);
            var a = Algorithm.GaussianCurvature(obj.Mesh);
            var max = a.Max();
            var min = a.Min();
            max -= min;
            var color = new Vector3[a.Count];
            for (int i = 0; i < color.Length; i++)
            {
                var val = MathHelper.Clamp(Math.Abs(a[i]), 0.0f, 0.4f);
                if (a[i] < 0)
                {
                    color[i] = new Vector3(0.0f, 0.0f, val);
                }
                else
                {
                    color[i] = new Vector3(val, 0.0f, 0.0f);
                }

            }

            obj.SetColorBuffer(color);
        }

        private void BtnAverageGeo_Click(object sender, EventArgs e)
        {
            var obj = (_SceneGraph.SceneItems[0] as MeshRenderer);
            Graph g = new Graph(obj.Mesh);
            var a = Algorithm.AverageGeodesicDistance(g);

            float max = a.Max();

            var color = new Vector3[a.Count];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = ProcessOutputHandler.ColorPixelVector(a[i], max);
            }

            obj.SetColorBuffer(color);

        }

        private void BtnIsoCurve_Click(object sender, EventArgs e)
        {
            var obj = (_SceneGraph.SceneItems[0] as MeshRenderer);
            var graph = new Graph(obj.Mesh);
            var samples = Algorithm.FarthestPointSampling(graph, 1);

            for (int i = 0; i < samples.SampelIndices.Count; i++)
            {
                MeshRenderer gizmo = new MeshRenderer(PrimitiveObjectFactory.CreateCube(0.05f));
                gizmo.Transform.Position = samples.SamplePoints[i].Coord;
                _SceneGraph.AddObject(gizmo);
            }

            var output = Algorithm.IsoCurveSignature(obj.Mesh, samples.SampelIndices[0], 40);

            chartIsoCurve.Series[0].Points.Clear();

            for (int i = 0; i < output.IsoCurveDistances.Length; i++)
            {
                var line = new LineRenderer(output.IsoCurves[i]);
                _SceneGraph.AddObject(line);
                line.Color = new Vector4(1, 0, 0, 1);
                chartIsoCurve.Series[0].Points.AddXY(i, output.IsoCurveDistances[i]);
            }

            chartIsoCurve.Show();
        }

        delegate void SafeCallDelegate(string text);
        private void AppendLogSafe(string text)
        {
            if (Log.InvokeRequired)
            {
                var d = new SafeCallDelegate(AppendLogSafe);
                Log.Invoke(d, new object[] { text });
            }
            else
            {
                Log.AppendText(text);
            }
        }

        #endregion

    }
}
