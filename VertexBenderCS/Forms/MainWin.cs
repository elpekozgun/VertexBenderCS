using System;
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
using System.Threading;
using System.Runtime.InteropServices;
using VertexBenderCS.Forms;
using System.IO;

namespace VertexBenderCS.Forms
{

    public partial class MainWin : Form
    {
        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        //[DllImport("User32.dll")]
        //private static extern IntPtr GetWindowDC(IntPtr hWnd);

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    const int WM_NCPAINT = 0x85;
        //    if (m.Msg == WM_NCPAINT)
        //    {
        //        IntPtr hdc = GetWindowDC(m.HWnd);
        //        if ((int)hdc != 0)
        //        {
        //            Graphics g = Graphics.FromHdc(hdc);
        //            g.FillRectangle(new SolidBrush(Color.FromArgb(255,255,50,50)), new Rectangle(0, 0, 4800, 4800));
        //            g.Flush();
        //            ReleaseDC(m.HWnd, hdc);
        //        }
        //    }
        //}

        private Camera              _camera;
        private CameraController    _cameraController;
        private SceneGraph          _SceneGraph;
        private Logger              _Logger;

        private FrmProcess          _frmProcess;

        private bool                _isFirstMouse;
        private float               _mouseX;
        private float               _mouseY;
        private System.Timers.Timer _Timer;

        private MeshRenderer            _activeMesh;
        private Dictionary<Transform ,IsoCurveOutput>    _IsoCurveOutputs;

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
            
            menuImportOff.Click += MenuImport_Click;

            menuProcessSP.Click += menuProcessSP_Click;
            menuProcessGC.Click += menuProcessGC_Click;
            menuProcessDescriptor.Click += menuProcessDescriptor_Click;
            menuExit.Click += MenuExit_Click;
            menuIsoCurveExport.Click += MenuIsoCurveExport_Click;

            _Logger.OnItemLogged += Logger_OnItemLogged;
            _Logger.OnLogCleaned += Logger_OnLogCleaned;

            sceneGraphTree.AfterSelect += SceneGraphTree_AfterSelect;
            sceneGraphTree.KeyDown += SceneGraphTree_KeyDown;
            _SceneGraph.OnItemAdded += SceneGraph_OnItemAdded;
            _SceneGraph.OnItemDeleted+= SceneGraph_OnItemDeleted;
            _SceneGraph.OnSceneCleared += SceneGraph_OnSceneCleared;
        }



        private void Logger_OnLogCleaned(string obj)
        {
            Log.Clear();
        }

        private void Logger_OnItemLogged(string obj)
        {
            Log.AppendText(obj);
            Log.AppendText(Environment.NewLine);
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
                    var split = d.FileName.Split(new char[] { '\\'});
                    var name = split[split.Length - 1];

                    //_SceneGraph.Clean();
                    var obj = new MeshRenderer(ObjectLoader.LoadOff(d.FileName), name);
                    sceneGraphTree.SelectedNode = null;
                    _SceneGraph.AddObject(obj);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            statusBar.BackColor = Color.FromArgb(50, 50, 50);
            statusBar.ForeColor = Color.MediumAquamarine;
            mainMenu.BackColor = Color.FromArgb(50, 50, 50);
            mainMenu.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());
            toolBar.Renderer = new CustomToolStripRenderer();

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

            _frmProcess = null;
            _Logger = new Logger();
            _SceneGraph = new SceneGraph();
            _camera = new Camera(GLControl.Width, GLControl.Height);
            _cameraController = new CameraController(_camera);
            _IsoCurveOutputs = new Dictionary<Transform, IsoCurveOutput>();
            
            InitTransformPanel();
            SubscribeEvents();
            
            //SetupScene();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
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

            //_coordinateLabel.Text = "X: " + _camera.Position.X + "\n" + "Y: " + _camera.Position.Y + "Z: " + _camera.Position.Z;
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

        #region Scene Graph

        private void SceneGraph_OnSceneCleared()
        {
            sceneGraphTree.Nodes.Clear();
        }

        private void SceneGraph_OnItemDeleted(Transform obj)
        {
            var deletionNode = GetNodeFromTree(obj, (SceneNode)sceneGraphTree.SelectedNode);
            if (deletionNode != null)
            {
                deletionNode.Remove();
            }
        }

        private SceneNode GetNodeFromTree(Transform obj, SceneNode Parent)
        {
            if (obj == Parent.Transform)
            {
                return Parent;
            }
            foreach (SceneNode node in Parent.Nodes)
            {
                if (node.Transform == obj)
                {
                    return GetNodeFromTree(obj, node);
                }
            }
            return null;
        }

        private void SceneGraph_OnItemAdded(Transform obj)
        {
            var selectedNode = sceneGraphTree.SelectedNode;
            if (selectedNode == null)
            {
                sceneGraphTree.Nodes.Add(new SceneNode(obj));
            }
            else
            {
                sceneGraphTree.SelectedNode.Nodes.Add(new SceneNode(obj));
            }
        }

        private void SceneGraphTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var item = (e.Node as SceneNode).Transform;
            if (item is MeshRenderer)
            {
                _activeMesh = item as MeshRenderer;

                numericScaleX.ValueChanged -= NumericScale_ValueChanged;
                numericScaleY.ValueChanged -= NumericScale_ValueChanged;
                numericScaleZ.ValueChanged -= NumericScale_ValueChanged;

                numericPosX.ValueChanged -= NumericPos_ValueChanged;
                numericPosY.ValueChanged -= NumericPos_ValueChanged;
                numericPosZ.ValueChanged -= NumericPos_ValueChanged;

                numericRotX.ValueChanged -= NumericRot_ValueChanged;
                numericRotY.ValueChanged -= NumericRot_ValueChanged;
                numericRotZ.ValueChanged -= NumericRot_ValueChanged;

                transformPanel.Visible = true;
                labelTransform.Text = _activeMesh.Name;
                numericPosX.Value = new decimal(_activeMesh.Position.X);
                numericPosY.Value = new decimal(_activeMesh.Position.Y);
                numericPosZ.Value = new decimal(_activeMesh.Position.Z);

                //Quaternion to Euler angles.
                var q = _activeMesh.Rotation;
                var eX = Math.Atan2(-2 * (q.Y * q.Z - q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z);
                var eY = Math.Asin(2 * (q.X * q.Z + q.W * q.Y));
                var eZ = Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z);

                numericRotX.Value = new decimal(MathHelper.RadiansToDegrees(eX));
                numericRotY.Value = new decimal(MathHelper.RadiansToDegrees(eY));
                numericRotZ.Value = new decimal(MathHelper.RadiansToDegrees(eZ));

                numericScaleX.Value  = new decimal(_activeMesh.Scale.X);
                numericScaleY.Value  = new decimal(_activeMesh.Scale.Y);
                numericScaleZ.Value  = new decimal(_activeMesh.Scale.Z);

                numericScaleX.ValueChanged += NumericScale_ValueChanged;
                numericScaleY.ValueChanged += NumericScale_ValueChanged;
                numericScaleZ.ValueChanged += NumericScale_ValueChanged;

                numericPosX.ValueChanged += NumericPos_ValueChanged;
                numericPosY.ValueChanged += NumericPos_ValueChanged;
                numericPosZ.ValueChanged += NumericPos_ValueChanged;

                numericRotX.ValueChanged += NumericRot_ValueChanged;
                numericRotY.ValueChanged += NumericRot_ValueChanged;
                numericRotZ.ValueChanged += NumericRot_ValueChanged;

                chartIsoCurve.Hide();
                if(_IsoCurveOutputs.TryGetValue(_activeMesh, out IsoCurveOutput output))
                {
                    UpdateChart(output);
                }
            }
        }

        private void SceneGraphTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (sceneGraphTree.SelectedNode != null)
                {
                    var obj = (sceneGraphTree.SelectedNode as SceneNode).Transform;
                    _SceneGraph.DeleteObject(obj);
                    if (_IsoCurveOutputs.ContainsKey(obj))
                    {
                        _IsoCurveOutputs.Remove(obj);
                    }
                }
            }
        }


        #endregion

        #region Transform Panel

        public void InitTransformPanel()
        {

            numericPosX.Controls[0].Visible = false;
            numericPosY.Controls[0].Visible = false;
            numericPosZ.Controls[0].Visible = false;
            numericRotX.Controls[0].Visible = false;
            numericRotY.Controls[0].Visible = false;
            numericRotZ.Controls[0].Visible = false;
            numericScaleX.Controls[0].Visible = false;
            numericScaleY.Controls[0].Visible = false;
            numericScaleZ.Controls[0].Visible = false;
            transformPanel.Visible = false;

            numericScaleX.ValueChanged += NumericScale_ValueChanged;
            numericScaleY.ValueChanged += NumericScale_ValueChanged;
            numericScaleZ.ValueChanged += NumericScale_ValueChanged;

            numericPosX.ValueChanged += NumericPos_ValueChanged;
            numericPosY.ValueChanged += NumericPos_ValueChanged;
            numericPosZ.ValueChanged += NumericPos_ValueChanged;

            numericRotX.ValueChanged += NumericRot_ValueChanged;
            numericRotY.ValueChanged += NumericRot_ValueChanged;
            numericRotZ.ValueChanged += NumericRot_ValueChanged;

        }

        private void NumericRot_ValueChanged(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                _activeMesh.Rotation = Quaternion.FromEulerAngles
                (
                    MathHelper.DegreesToRadians((float)numericRotX.Value),
                    MathHelper.DegreesToRadians((float)numericRotY.Value),
                    MathHelper.DegreesToRadians((float)numericRotZ.Value)
                );

                foreach (var child in _activeMesh.Children)
                {
                    child.Rotation = _activeMesh.Rotation;
                }

            }
        }

        private void NumericPos_ValueChanged(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                _activeMesh.Position = new Vector3
                (
                    (float)numericPosX.Value, 
                    (float)numericPosY.Value, 
                    (float)numericPosZ.Value
                );

                foreach (var child in _activeMesh.Children)
                {
                    child.Position = _activeMesh.Position;
                }
            }
        }

        private void NumericScale_ValueChanged(object sender, EventArgs e) 
        {
            if (_activeMesh != null)
            {
                _activeMesh.Scale = new Vector3
                (
                    (float)numericScaleX.Value, 
                    (float)numericScaleY.Value, 
                    (float)numericScaleZ.Value
                );
                foreach (var child in _activeMesh.Children)
                {
                    child.Scale = _activeMesh.Scale;
                }
            }
        }

        #endregion

        #region Drawing

        private void Render()
        {
            //lock (RenderLock)
            {

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.CullFace(CullFaceMode.Back);

                //lock(RenderLock)
                {
                    _SceneGraph.RenderAll(_camera);
                }

                GL.Flush();
                GLControl.SwapBuffers();
            }
        }

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

        private void UpdateChart(IsoCurveOutput output)
        {
            chartIsoCurve.Series[0].Points.Clear();
            for (int i = 0; i < output.IsoCurves.Count; i++)
            {
                chartIsoCurve.Series[0].Points.AddXY(i, output.IsoCurveDistances[i]);
            }
            chartIsoCurve.Show();
        }

        #endregion

        #region Menu Items

        private void DisplayShortestPathOutput(ShortestPathOutput output)
        {
            if (_activeMesh != null)
            {
                //foreach (var child in _activeMesh.Children)
                //{
                //    _SceneGraph.DeleteObject(child);
                //}

                List<Vector3> lines1 = new List<Vector3>();
                for (int i = 0; i < output.Path.Count - 1; i++)
                {
                    lines1.Add(_activeMesh.Mesh.Vertices[output.Path[i]].Coord);
                    lines1.Add(_activeMesh.Mesh.Vertices[output.Path[i + 1]].Coord);
                }

                var r1 = new LineRenderer(lines1, output.Type.ToString() + " path");
                r1.Parent = _activeMesh;

                r1.Position = _activeMesh.Position;
                r1.Rotation = _activeMesh.Rotation;
                r1.Scale = _activeMesh.Scale;

                r1.Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                if (output.Method == eShortestPathMethod.Astar)
                {
                    r1.Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                }
                
                _SceneGraph.AddObject(r1);

                _Logger.Append(output.Info);
            }
        }

        private void DisplaySamplingOutput(SampleOutput output)
        {
            if (_activeMesh != null)
            {
                foreach (var child in _activeMesh.Children)
                {
                    _SceneGraph.DeleteObject(child);
                }

                List<MeshRenderer> points = new List<MeshRenderer>();
                for (int i = 0; i < output.SampleIndices.Count; i++)
                {
                    MeshRenderer obj = new MeshRenderer(PrimitiveObjectFactory.CreateCube(0.05f), Shader.DefaultUnlitShader, "sample-" + i + 1)
                    {
                        //Color = new Vector4(output.SamplePoints[i].Coord * 0.5f, 1.0f)
                        Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    };
                    obj.Position =  output.SamplePoints[i].Coord;

                    //obj.Position = new Vector3(_activeMesh.ModelMatrix * new Vector4(obj.Position));
                    obj.Position += _activeMesh.Position;

                    
                    // bad block but keep it for now..
                    obj.Parent = _activeMesh;

                    points.Add(obj);
                    _SceneGraph.AddObject(obj);
                }

                _Logger.Append(output.Info);
            }
        }

        private void DisplayIsoCurveOutput(IsoCurveOutput output)
        {
            if (_activeMesh != null)
            {
                if (_IsoCurveOutputs.ContainsKey(_activeMesh))
                {
                    _IsoCurveOutputs.Remove(_activeMesh);
                }
                _IsoCurveOutputs.Add(_activeMesh, output);

                foreach (var child in _activeMesh.Children)
                {
                    _SceneGraph.DeleteObject(child);
                }

                MeshRenderer gizmo = new MeshRenderer(PrimitiveObjectFactory.CreateCube(0.05f), Shader.DefaultUnlitShader, "source")
                {
                    Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                };
                gizmo.Position = _activeMesh.Mesh.Vertices[output.SourceIndex].Coord;

                gizmo.Parent = _activeMesh;
                _SceneGraph.AddObject(gizmo);


                for (int i = 0; i < output.IsoCurveDistances.Length; i++)
                {
                    var line = new LineRenderer(output.IsoCurves[i], "sample-" + i);
                    line.Position = _activeMesh.Position;
                    line.Rotation = _activeMesh.Rotation;
                    line.Scale = _activeMesh.Scale;
                    
                    line.Parent = _activeMesh;
                    _SceneGraph.AddObject(line);

                    line.Color = new Vector4(1, 0, 0, 1);
                }

                UpdateChart(output);

                _Logger.Append(output.Info);
            }
        }

        private void DisplayAverageGeodesicOutput(AverageGeodesicOutput output)
        {
            if (_activeMesh != null)
            {
                float max = output.Distances.Max();

                var color = new Vector3[output.Distances.Length];
                for (int i = 0; i < color.Length; i++)
                {
                    color[i] = ProcessOutputHandler.ColorPixelVector(output.Distances[i], max) * 0.5f;
                }

                _activeMesh.SetColorBuffer(color);
                _Logger.Append(output.Info);
            }
        }

        private void ShortestPathOnResultReturned(IOutput output)
        {
            switch (output.Type)
            {
                case eOutputType.shortestPath:
                    DisplayShortestPathOutput((ShortestPathOutput)output);
                    break;
                case eOutputType.Sampling:
                    DisplaySamplingOutput((SampleOutput)output);
                    break;
                case eOutputType.IsoCurve:
                    DisplayIsoCurveOutput((IsoCurveOutput)output);
                    break;
                case eOutputType.AverageGeodesicDistance:
                    DisplayAverageGeodesicOutput((AverageGeodesicOutput)output);
                    break;
                default:
                    break;
            }
        }

        private void menuProcessSP_Click(object sender, EventArgs e)
        {

            if (_activeMesh != null)
            {
                var mesh = _activeMesh.Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.ShortestPath);
                _frmProcess.OnResultReturned += ShortestPathOnResultReturned;
                _frmProcess.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a model to process", "No models selected", MessageBoxButtons.OK);
            }
        }

        private void menuProcessGC_Click(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                var a = Algorithm.GaussianCurvature(_activeMesh.Mesh);
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

                _activeMesh.SetColorBuffer(color);
            }
        }

        private void menuProcessDescriptor_Click(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                var mesh = _activeMesh.Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.Descriptor);
                _frmProcess.OnResultReturned += ShortestPathOnResultReturned;
                _frmProcess.ShowDialog();
            }
        }

        private void MenuIsoCurveExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            dialog.Title = "Save csv Files";
            dialog.DefaultExt = "csv";
            dialog.FilterIndex = 2;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProcessOutputHandler.SaveIsoCurveOutputs(_IsoCurveOutputs, dialog.FileName);
            }

        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Toolbar



        #endregion

    }

}
