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

namespace VertexBenderCS
{
    public partial class MainWin : Form
    {
        private Camera _camera;
        private CameraController _cameraController;

        private bool _isFirstMouse = true;
        private float _mouseX;
        private float _mouseY;

        private System.Timers.Timer _Timer;
        
        //TEST STUFF
        private MeshRenderer _mesh;
        private Shader _wiredShader;
        private Shader _shader;
        private LineRenderer _lineRenderer;


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

            SubscribeEvents();
            SetupTestScene();

            _Timer = new System.Timers.Timer(8.0f);
            _Timer.Elapsed += Update;
            _Timer.Start();
        }

        private void Update(object sender, System.Timers.ElapsedEventArgs e)
        {
            GLControl.Invalidate();           
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
        }

        private void SetupTestScene()
        {

            _camera = new Camera(GLControl.Width, GLControl.Height);
            _cameraController = new CameraController(_camera);

            int idv = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\ModelLoadingVertexShaderWithLight.glsl", ShaderType.VertexShader);
            int idf = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\ModelLoadingFragmentShaderLighted.glsl", ShaderType.FragmentShader);
            _shader = ShaderBuilder.CreateShader("shaded", idv, idf);

            idv = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\WireframeVertex.glsl", ShaderType.VertexShader);
            idf = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\WireframeFragment.glsl", ShaderType.FragmentShader);
            _wiredShader = ShaderBuilder.CreateShader("wired", idv, idf);

            //idv = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\smoothLineVertex.glsl", ShaderType.VertexShader);
            //idf = ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\smoothLineFragment.glsl", ShaderType.FragmentShader);
            //_lineShader = ShaderBuilder.CreateShader("smoothLine", idv, idf);


            var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\MyDemo\0.off");
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

            //var lines = new List<Vector3>();
            //int i = 0;
            //var prime = mesh.Vertices[0];
            //while (i < 100)
            //{
            //    prime = mesh.Vertices[prime.Verts[i % prime.Verts.Count]];
            //    lines.Add(prime.Coord);
            //    i++;
            //}


            //var mesh = Engine.Core.ObjectLoader.CreateCube(0.1f);
            _mesh = new MeshRenderer(mesh);
            //_mesh.Shader = _shader;

            var a = Dijkstra.ConstructGraphFromMesh(mesh);

            var b = Dijkstra.FindShortestPath(a, mesh.Vertices.Count,101, 491, out int[] path);

            List<Vector3> lines = new List<Vector3>();

            for (int i = 0; i < path.Length; i++)
            {
                lines.Add(mesh.Vertices[path[i]].Coord);
            }

            _lineRenderer = new LineRenderer(lines);

        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Back);

            Matrix4 model = Matrix4.Identity;

            _mesh.Shader = _shader;
            _mesh.Shader.Use();
            _mesh.Shader.SetVec3("cameraPosition", _camera.Position);
            _mesh.Shader.SetVec3("directLight.direction", -0.2f, -1.0f, -0.0f);
            _mesh.Shader.SetVec3("directLight.ambient", 0.15f, 0.15f, 0.15f);
            _mesh.Shader.SetVec3("directLight.diffuse", 0.5f, 0.5f, 0.5f);
            _mesh.Shader.SetVec3("directLight.specular", 0.5f, 0.5f, 0.5f);
            _mesh.Shader.SetFloat("material.shineness", 32.0f);
            _mesh.Shader.SetMat4("Model", model);
            _mesh.Shader.SetMat4("View", _camera.View);
            _mesh.Shader.SetMat4("Projection", _camera.Projection);
            _mesh.Render(eRenderMode.shaded);

            _mesh.Shader = _wiredShader;
            _mesh.Shader.Use();
            _mesh.Shader.SetMat4("Model", model);
            _mesh.Shader.SetMat4("View", _camera.View);
            _mesh.Shader.SetMat4("Projection", _camera.Projection);
            _mesh.Shader.SetVec4("Color", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            _mesh.Render(eRenderMode.wireFrame);



            _lineRenderer.Shader = _wiredShader;
            _lineRenderer.Shader.Use();
            _lineRenderer.Shader.SetMat4("Model", model);
            _lineRenderer.Shader.SetMat4("View", _camera.View);
            _lineRenderer.Shader.SetMat4("Projection", _camera.Projection);
            _lineRenderer.Shader.SetVec4("Color", new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
            _lineRenderer.Render();

            //Matrix4 scale = Matrix4.CreateScale(1.1f);

            


            GL.Flush();
            GLControl.SwapBuffers();
        }

        private void ProcessMouseInput(int x, int y)
        {

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
            _mouseX = e.X;
            _mouseY = e.Y;

            if (e.Button == MouseButtons.Middle)
            {
                _cameraController.Pan(offsetX, offsetY);
            }
            else if (e.Button == MouseButtons.Right && (KeyState & Keys.ControlKey) == Keys.ControlKey)
            {
                _cameraController.OrbitAround(offsetX , offsetY , _mesh.Center + new Vector3(0, 0.1f,0));
            }
            else if(e.Button == MouseButtons.Right)
            {
                _cameraController.Rotate(offsetX , offsetY);
            }

            //statusBar.Text = "X: " + offsetX + "\n" + "Y: " + offsetY;
            //statusBar.Text = KeyState.ToString();
        }

        private void GLControl_MouseDown(object sender, MouseEventArgs e)
        {
        }

        //private void Application_Idle(object sender, EventArgs e)
        //{
        //    while (GLControl.IsIdle)
        //    {
        //        Render();
        //    }
        //}

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

            if (e.KeyData == Keys.Q)
            {
                _cameraController.Navigate(eCameraMovement.up);
            }
            if (e.KeyData == Keys.E)
            {
                _cameraController.Navigate(eCameraMovement.down);
            }
            if (e.KeyData == Keys.W)
            {
                _cameraController.Navigate(eCameraMovement.forward);
            }
            if (e.KeyData== Keys.S)
            {
                _cameraController.Navigate(eCameraMovement.backward);
            }
            if (e.KeyData == Keys.A)
            {
                _cameraController.Navigate(eCameraMovement.left);
            }
            if (e.KeyData == Keys.D)
            {
                _cameraController.Navigate(eCameraMovement.right);
            }
        }

        private void GLControl_KeyUp(object sender, KeyEventArgs e)
        {
            KeyState = Keys.None;
            if (e.KeyCode == Keys.F12)
            {
                GrabScreenshot().Save("screenshot.png");
            }
        }

        private void GLControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        Bitmap GrabScreenshot()
        {
            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
            bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte,
                data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
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
