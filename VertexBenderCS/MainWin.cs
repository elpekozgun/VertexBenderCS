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

        private bool _isFirstMouse;
        private float _mouseX;
        private float _mouseY;

        private System.Timers.Timer _Timer;
        
        //TEST STUFF
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
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
            _isFirstMouse = true;

            _Timer = new System.Timers.Timer(8.0f);
            _Timer.Elapsed += Update;
            _Timer.Start();

            SubscribeEvents();
            SetupTestScene();

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


            btnDijkstra.Click += BtnDijkstra_Click;

            txtSource.TextChanged += TxtSource_TextChanged;
            txtTarget.TextChanged += TxtTarget_TextChanged;
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Back);

            Matrix4 model = Matrix4.CreateScale(50.0f);
            //Matrix4 rotation = Matrix4.CreateRotationX(-MathHelper.Pi / 2);
            //model = model * rotation;

            //Matrix4 model = Matrix4.Identity;


            _meshRenderer.Shader = _shader;
            _meshRenderer.Shader.Use();
            _meshRenderer.Shader.SetVec3("cameraPosition", _camera.Position);
            _meshRenderer.Shader.SetVec3("directLight.direction", -0.2f, -1.0f, -0.0f);
            _meshRenderer.Shader.SetVec3("directLight.ambient", 0.15f, 0.15f, 0.15f);
            _meshRenderer.Shader.SetVec3("directLight.diffuse", 0.5f, 0.5f, 0.5f);
            _meshRenderer.Shader.SetVec3("directLight.specular", 0.5f, 0.5f, 0.5f);
            _meshRenderer.Shader.SetFloat("material.shineness", 32.0f);
            _meshRenderer.Shader.SetMat4("Model", model);
            _meshRenderer.Shader.SetMat4("View", _camera.View);
            _meshRenderer.Shader.SetMat4("Projection", _camera.Projection);
            _meshRenderer.Render(eRenderMode.shaded);

            //_meshRenderer.Shader = _wiredShader;
            //_meshRenderer.Shader.Use();
            //_meshRenderer.Shader.SetMat4("Model", model);
            //_meshRenderer.Shader.SetMat4("View", _camera.View);
            //_meshRenderer.Shader.SetMat4("Projection", _camera.Projection);
            //_meshRenderer.Shader.SetVec4("Color", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            //_meshRenderer.Render(eRenderMode.wireFrame);



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
                _cameraController.OrbitAround(offsetX , offsetY , _meshRenderer.Center + new Vector3(0, 0.1f,0));
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
                GrabScreenshot().Save("screenshot.png");
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
            var mesh = Engine.Core.ObjectLoader.LoadOff(@"C:\Users\ozgun\Desktop\examples\64.off");



            //var lines = new List<Vector3>();
            //int i = 0;
            //var prime = mesh.Vertices[0];
            //while (i < 100)
            //{
            //    prime = mesh.Vertices[prime.Verts[i % prime.Verts.Count]];
            //    lines.Add(prime.Coord);
            //    i++;
            //}

            _mesh = mesh;
            //var mesh = Engine.Core.ObjectLoader.CreateCube(0.1f);
            _meshRenderer = new MeshRenderer(mesh);
            //_mesh.Shader = _shader;

            var a = Dijkstra.ConstructGraphFromMesh(mesh);

            Dijkstra.DijkstraArray(a, 0, 111, out int[] path);

            List<Vector3> lines = new List<Vector3>();

            for (int i = 0; i < path.Length; i++)
            {
                lines.Add(mesh.Vertices[path[i]].Coord);
            }

            _lineRenderer = new LineRenderer(lines);
        }

        private int? _src = null;
        private int? _trg = null;

        private void TxtTarget_TextChanged(object sender, EventArgs e)
        {
            var text = (sender as MaskedTextBox).Text;

            if (text != string.Empty)
            {
                _src = new int?(int.Parse(text));
            }

        }

        private void TxtSource_TextChanged(object sender, EventArgs e)
        {
            var text = (sender as MaskedTextBox).Text;

            if (text != string.Empty)
            {
                _trg = new int?(int.Parse(text));
            }
        }

        private void BtnDijkstra_Click(object sender, EventArgs e)
        {
            if (_src.HasValue && _trg.HasValue)
            {
                var a = Dijkstra.ConstructGraphFromMesh(_mesh);

                Dijkstra.DijkstraArray(a, _src.Value, _trg.Value, out int[] path);

                List<Vector3> lines = new List<Vector3>();

                if (path[0] == -1)
                {
                    return;
                }
                for (int i = 0; i < path.Length; i++)
                {
                    lines.Add(_mesh.Vertices[path[i]].Coord);
                }
                _lineRenderer = new LineRenderer(lines);

                Dijkstra.CreateBitmap(a, path, @"C:\Users\ozgun\Desktop\" + _src.Value.ToString() + " -" + _trg.Value.ToString());
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
