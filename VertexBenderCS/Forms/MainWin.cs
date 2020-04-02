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
        //this.GLControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8,8,8,8), 24, 8, 8));

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

        private FrmProcess          _frmProcess;

        private bool                _isFirstMouse;
        private float               _mouseX;
        private float               _mouseY;
        private System.Timers.Timer _timer;
        private eRenderMode         _renderMode;
        private bool                _isPerspective;

        private MeshRenderer            _activeMesh;
        private Dictionary<Transform ,IsoCurveOutput>    _IsoCurveOutputs;
        
        private OpenTK.Input.KeyboardState KeyState { get; set; }
        private OpenTK.Input.MouseState MouseState { get; set; }

        public MainWin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PrepareUI();
            SubscribeEvents();
#if DEBUG
            SetupTestScene();
#endif
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        
        public void PrepareUI()
        {
            statusBar.BackColor = Color.FromArgb(50, 50, 50);
            statusBar.ForeColor = Color.MediumAquamarine;
            mainMenu.BackColor = Color.FromArgb(50, 50, 50);
            mainMenu.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());
            toolBar.Renderer = new CustomToolStripRenderer();


            GL.ClearColor(Color.FromArgb(50,50,50));
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            Text = "Vertex Bender Framework - " +
                    //GL.GetString(StringName.Vendor) + " " +
                    GL.GetString(StringName.Renderer) + " " +
                    GL.GetString(StringName.Version);

            GLControl_Resize(GLControl, EventArgs.Empty);

            _mouseX = (int)(Width * 0.5);
            _mouseY = (int)(Height * 0.5);
            _isFirstMouse = true;

            _timer = new System.Timers.Timer(8.0f);
            _timer.Elapsed += Update;
            _timer.Start();

            _frmProcess = null;
            //_Logger = new Logger();
            _SceneGraph = new SceneGraph();
            _camera = new Camera(GLControl.Width, GLControl.Height);
            _cameraController = new CameraController(_camera);
            _IsoCurveOutputs = new Dictionary<Transform, IsoCurveOutput>();
            _isPerspective = true;

            _renderMode = eRenderMode.shaded;

            InitTransformPanel();
            InitSpherePanel();
        }


        public void SubscribeEvents()
        {
            GLControl.MouseClick += GLControl_MouseClick;
            GLControl.KeyDown += GLControl_KeyDown;
            GLControl.KeyUp += GLControl_KeyUp;
            GLControl.Resize += GLControl_Resize;
            GLControl.Paint += GLControl_Paint;
            GLControl.MouseDown += GLControl_MouseDown;
            GLControl.MouseMove += GLControl_MouseMove;
            GLControl.MouseWheel += GLControl_MouseWheel;
            
            menuImportOff.Click += MenuImport_Click;

            menuProcessSP.Click += menuProcessSP_Click;
            menuProcessGC.Click += menuProcessGC_Click;
            menuProcessDescriptor.Click += menuProcessDescriptor_Click;
            menuProcessParametrization.Click += MenuProcessParametrization_Click;
            menuExit.Click += MenuExit_Click;
            menuIsoCurveExport.Click += MenuIsoCurveExport_Click;

            menuAddCube.Click += MenuAddCube_Click;
            menuAddPyramid.Click += MenuAddPyramid_Click;
            menuAddBottomlessPyramid.Click += MenuAddBottomlessPyramid_Click;
            menuAddTetrahedron.Click += MenuAddTetrahedron_Click;
            menuAddSphereCube.Click += MenuAddSphereCube_Click;
            menuAddSphereTetra.Click += MenuAddSphereTetra_Click;
            menuAddSphereIcosahedron.Click += MenuAddSphereIcosahedron_Click;

            toolbarPoint.Click += ToolbarPoint_Click;
            toolbarShaded.Click += ToolbarShaded_Click;
            toolbarWireframe.Click += ToolbarWireframe_Click;
            toolbarProjectionMode.Click += ToolbarProjectionMode_Click;
            toolbarIsBlinn.Click += ToolbarIsBlinn_Click;

            Logger.OnItemLogged += Logger_OnItemLogged;
            Logger.OnLogCleaned += Logger_OnLogCleaned;

            sceneGraphTree.AfterSelect += SceneGraphTree_AfterSelect;
            sceneGraphTree.KeyDown += SceneGraphTree_KeyDown;
            sceneGraphTree.MouseDown += SceneGraphTree_MouseClick;
            sceneGraphTree.Leave += SceneGraphTree_LostFocus;

            _SceneGraph.OnItemAdded += SceneGraph_OnItemAdded;
            _SceneGraph.OnItemDeleted+= SceneGraph_OnItemDeleted;
            _SceneGraph.OnSceneCleared += SceneGraph_OnSceneCleared;
        }



        private void SceneGraphTree_LostFocus(object sender, EventArgs e)
        {
            sceneGraphTree.HideSelection = false;
        }

        private void StarTest()
        {
            var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\Desktop\MeshsegBenchmark-1.0\data\off\169-2.off"), "test");
            //mesh.Scale = Vector3.One * 10;

            _activeMesh = mesh;

            var center = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "center");
            center.Position = mesh.Mesh.Center(); //- new Vector3(0, 0.04f, 0);


            center.Color = new Vector4(1, 0, 0, 1);
            _SceneGraph.AddObject(center);

            var m2 = Algorithm.SphereTest(mesh.Mesh, 10);
            var m3 = Algorithm.SphereTest(mesh.Mesh, 1);

            var sphere2 = new Mesh();
            var sphere3 = new Mesh();

            for (int i = 0; i < m2.PointsOnSphere.Count; i++)
            {
                sphere2.Vertices.Add(new Vertex(i, m2.PointsOnSphere[i], m2.Normals[i]));
                sphere3.Vertices.Add(new Vertex(i, m3.PointsOnSphere[i], m3.Normals[i]));
            }
            sphere2.Triangles = _activeMesh.Mesh.Triangles;
            sphere3.Triangles = _activeMesh.Mesh.Triangles;

            var renderer2 = new MeshRenderer(sphere2, "sphere2")
            {
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Position = _activeMesh.Position + new Vector3(0.05f, 0.0f, 0.0f),
                //Scale = mesh.Scale
            };

            var renderer3 = new MeshRenderer(sphere3, "sphere3")
            {
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Position = _activeMesh.Position + new Vector3(0.1f, 0.0f, 0.0f),
                // Scale = mesh.Scale
            };

            for (int i = 0; i < m2.Center.Count; i++)
            {
                var c2 = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "c2");
                c2.Position = m2.Center[i];
                c2.Color = new Vector4(1, 1, 0, 1);
                _SceneGraph.AddObject(c2);
            }

            //var c2 = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "c2");
            //c2.Position = m2.Center;
            //c2.Color = new Vector4(1, 1, 0, 1);

            //var c3 = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "c3");
            //c3.Position = m3.Center;
            //c3.Color = new Vector4(0, 0, 1, 1);


            //_SceneGraph.AddObject(c3);


            _SceneGraph.AddObject(mesh);
            _SceneGraph.AddObject(renderer2);
            _SceneGraph.AddObject(renderer3);
        }

        private void SetupTestScene()
        {
            var origin = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "origin");
            origin.Color = Vector4.One;
            _SceneGraph.AddObject(origin);

            //var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\OneDrive\DERSLER\Ceng789\proje ödev\meshes1\1) use for geodesic\fprint matrix\horse0.off"), "test");
            //_activeMesh = mesh;
            //_SceneGraph.AddObject(mesh);
            //var m = Algorithm.ParameterizeMeshToDisc(mesh.Mesh, eParameterizationMethod.Harmonic, (asd) => { });

            //DisplayDiscParametrizationOutput(m);


        }

        private void Update(object sender, System.Timers.ElapsedEventArgs e)
        {

            InvokeIfRequired(GLControl, () =>
            {
                GLControl.Invalidate();
                GLControl.Update();

                if (GLControl.Focused)
                {
                    _cameraController.Navigate(KeyState);
                    KeyState = OpenTK.Input.Keyboard.GetState();
                    MouseState = OpenTK.Input.Mouse.GetCursorState();
                    MouseState = OpenTK.Input.Mouse.GetState();
                }
            });

            //GLControl.Invoke((MethodInvoker)delegate ()
            //{
            //    GLControl.Invalidate();
            //    GLControl.Update();

            //    if (GLControl.Focused)
            //    {
            //        _cameraController.Navigate(KeyState);
            //        KeyState = OpenTK.Input.Keyboard.GetState();
            //        MouseState = OpenTK.Input.Mouse.GetCursorState();
            //        MouseState = OpenTK.Input.Mouse.GetState();
            //    }
            //});
        }

        private void InvokeIfRequired(Control control, MethodInvoker action)
        {
            if (control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                try
                {
                    control.Invoke(action);
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException e)
                {
                    if (!e.Message.Contains("invoke"))
                    {
                        throw e;
                    }
                }
            }
            else
            {
                action();
            }

        }


        #region Logger

        private void Logger_OnLogCleaned(string obj)
        {
            Log.Invoke((MethodInvoker)delegate ()
            {
                Log.Clear();
            });
        }

        private void Logger_OnItemLogged(string obj)
        {
            Log.Invoke((MethodInvoker)delegate ()
            {
                Log.AppendText(obj);
                Log.AppendText(Environment.NewLine);
            });
        }

        #endregion

        #region Process Output Displayers

        private void DrawPath(MeshRenderer mesh, List<int> path, Vector4 color, string name = "")
        {
            List<Vector3> lines1 = new List<Vector3>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                lines1.Add(mesh.Mesh.Vertices[path[i]].Coord);
                lines1.Add(mesh.Mesh.Vertices[path[i + 1]].Coord);
            }

            var r1 = new LineRenderer(lines1, name + " path");
            r1.Parent = mesh;

            r1.Position = mesh.Position;
            r1.Rotation = mesh.Rotation;
            r1.Scale = mesh.Scale;
            r1.Color = color;

            _SceneGraph.AddObject(r1);
        }

        private void DisplayShortestPathOutput(ShortestPathOutput output)
        {
            if (_activeMesh != null)
            {
                //foreach (var child in _activeMesh.Children)
                //{
                //    _SceneGraph.DeleteObject(child);
                //}

                var color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                if (output.Method == eShortestPathMethod.Astar)
                {
                    color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                }

                DrawPath(_activeMesh, output.Path, color, output.Type.ToString());

                //Logger.Append(output.Info);
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

                List<PrimitiveRenderer> points = new List<PrimitiveRenderer>();
                for (int i = 0; i < output.SampleIndices.Count; i++)
                {
                    PrimitiveRenderer obj = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.05f), Shader.DefaultShader, "sample-" + i + 1)
                    {
                        //Color = new Vector4(output.SamplePoints[i].Coord * 0.5f, 1.0f)
                        Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    };
                    obj.Position = output.SamplePoints[i].Coord;

                    //obj.Position = new Vector3(_activeMesh.ModelMatrix * new Vector4(obj.Position));
                    obj.Position += _activeMesh.Position;


                    // bad block but keep it for now..
                    obj.Parent = _activeMesh;

                    points.Add(obj);
                    _SceneGraph.AddObject(obj);
                }

                //Logger.Append(output.Info);
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

                PrimitiveRenderer indicator = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.05f), Shader.DefaultShader, "source")
                {
                    Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                };
                indicator.Position = _activeMesh.Mesh.Vertices[output.SourceIndex].Coord;

                indicator.Parent = _activeMesh;
                _SceneGraph.AddObject(indicator);


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

                //Logger.Append(output.Info);
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
                //Logger.Append(output.Info);
            }
        }

        private void DisplayDiscParametrizationOutput(DiscParameterizeOutput output)
        {
            if (_activeMesh != null)
            {
                foreach (var child in _activeMesh.Children)
                {
                    _SceneGraph.DeleteObject(child);
                }


                _activeMesh.SetMesh(output.Mesh);
                var outputmesh = new Mesh();

                for (int i = 0; i < output.Output.Count; i++)
                {
                    var v = new Vertex(i, new Vector3(output.Output[i].X, 0, output.Output[i].Y), Vector3.UnitY);
                    outputmesh.Vertices.Add(v);
                }
                outputmesh.Triangles = _activeMesh.Mesh.Triangles;

                var tex = Texture.LoadTexture(@"Resources\Image\UV1024.png", eTextureType.Diffuse);
                //var tex = Texture.LoadTexture(@"Resources\Image\tile.png", eTextureType.Diffuse);

                var renderer = new MeshRenderer(outputmesh, $"Disc-{_activeMesh.Name}")
                {
                    Position = _activeMesh.Position + new Vector3(0.2f, 0.0f, 0.0f),
                    EnableCull = false
                };
                renderer.Parent = _activeMesh;
                
                renderer.DiffuseTexture = tex;
                _activeMesh.DiffuseTexture = tex;

                _activeMesh.SetTextureBuffer(output.NormalizedUVCoords());
                renderer.SetTextureBuffer(output.NormalizedUVCoords());

                _SceneGraph.AddObject(renderer);
                

                DrawPath(_activeMesh, output.BoundaryPath, new Vector4(1, 0, 0, 0), $"path-{_activeMesh.Name}");

            }
        }

        private void DisplaySphereParametrizationOutput(SphereParameterizeOutput output)
        {
            if (_activeMesh != null)
            {
                //foreach (var child in _activeMesh.Children)
                //{
                //    _SceneGraph.DeleteObject(child);
                //}

                var sphere = new Mesh();

                for (int i = 0; i < output.PointsOnSphere.Count; i++)
                {
                    sphere.Vertices.Add(new Vertex(i, output.PointsOnSphere[i], output.Normals[i]));
                }
                sphere.Triangles = _activeMesh.Mesh.Triangles;

                var size = Box3.CalculateBoundingBox(output.PointsOnSphere).Size;

                var renderer = new MeshRenderer(sphere, "sphere")
                {
                    Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    Position = _activeMesh.Position + Vector3.UnitX * size
                };
                renderer.Rotation = _activeMesh.Rotation;


                var cube = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.01f * size),"center");
                cube.Position = output.Center[0];
                cube.Color = new Vector4(1, 0, 0, 1);

                _SceneGraph.AddObject(cube);
                //var tex = Texture.LoadTexture(@"Resources\Image\UV1024.png", eTextureType.Diffuse);
                //var tex = Texture.LoadTexture(@"Resources\Image\tile.png", eTextureType.Diffuse);
                //_activeMesh.DiffuseTexture = tex;
                //renderer.DiffuseTexture = tex;


                //_activeMesh.SetTextureBuffer(output.NormalizedUVCoords());
                //renderer.SetTextureBuffer(output.NormalizedUVCoords());

                _SceneGraph.AddObject(renderer);

            }
        }

        private void DisplayCutParametrizationOutput(CutSeamParameterizeOutput output)
        {
            if (_activeMesh != null)
            {

                //var m = Algorithm.ParameterizeMeshCutSeam(_activeMesh.Mesh, (asd) => { });
                var meshrend = new MeshRenderer(output.Cutmesh, "asdasd");
                _SceneGraph.AddObject(meshrend);
                _activeMesh = meshrend;
                DisplayDiscParametrizationOutput(output.Disc);
                DisplayShortestPathOutput(output.ShortestPath);
            }
        }

        private void ProcessResultReturned(IOutput output)
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
                case eOutputType.DiscParametrization:
                    DisplayDiscParametrizationOutput((DiscParameterizeOutput)output);
                    break;
                case eOutputType.SphereParametrization:
                    DisplaySphereParametrizationOutput((SphereParameterizeOutput)output);
                    break;
                case eOutputType.CutSeamParameterization:
                    DisplayCutParametrizationOutput((CutSeamParameterizeOutput)output);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region GlControl Events

        private void GLControl_MouseWheel(object sender,  System.Windows.Forms.MouseEventArgs e)
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
            //else if (e.Button == MouseButtons.Right && KeyState.IsKeyDown(OpenTK.Input.Key.ControlLeft))
            //{
            //    _cameraController.OrbitAround(offsetX, offsetY, _camera.Position + _camera.Front * 2);
            //}
            else if (e.Button == MouseButtons.Right)
            {
                _cameraController.Rotate(offsetX, offsetY);
            }
            _mouseX = e.X;
            _mouseY = e.Y;

        }

        private void GLControl_MouseClick(object sender, MouseEventArgs e)
        {
            //_mouseX = (float)e.X;
            //_mouseY = (float)e.Y;
            //_isFirstMouse = false;
        }

        private void GLControl_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    HalfWidth = this.ClientRectangle.Width / 2.0f;
            //    HalfHeight = this.ClientRectangle.Height / 2.0f;

            //    _mouseX = (e.X - HalfWidth) / HalfWidth;
            //    _mouseY = (HalfHeight - e.Y) / HalfHeight;

            //    mouseroll = true;
            //}

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
                _camera.AspectRatio = c.ClientSize.Width / (float)c.ClientSize.Height;
            }
        }

        private void GLControl_KeyDown(object sender, KeyEventArgs e)
        {
            _cameraController.Navigate(KeyState);
        }

        private void GLControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
#if DEBUG
                GrabScreenshot().Save(@"C:\users\ozgun\desktop\screenshot.png");
#else
                GrabScreenshot().Save(@"screenshot.png");
#endif
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
                var node = new SceneNode(obj);
                sceneGraphTree.Nodes.Add(node);
                sceneGraphTree.SelectedNode = node;
            }
            else
            {
                var node = new SceneNode(obj);
                node.Transform.Parent = (selectedNode as SceneNode).Transform;
                sceneGraphTree.SelectedNode.Nodes.Add(node);
                //sceneGraphTree.SelectedNode = node;
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

                var q = _activeMesh.Rotation;
                var rot = _activeMesh.Rotation.EulerAngles();
                
                numericRotX.Value = new decimal(rot.X);
                numericRotY.Value = new decimal(rot.Y);
                numericRotZ.Value = new decimal(rot.Z);

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
                
                if (_activeMesh.Mesh is Sphere)
                {
                    var sphere = (_activeMesh.Mesh as Sphere);

                    numericSubdivision.ValueChanged -= NumericSubdivision_ValueChanged;
                    numericSize.ValueChanged -= NumericSize_ValueChanged;

                    spherePanel.Visible = true;

                    numericSize.Value = (decimal)sphere.Size;
                    numericSubdivision.Value = (decimal)sphere.Subdivision;

                    numericSubdivision.ValueChanged += NumericSubdivision_ValueChanged;
                    numericSize.ValueChanged += NumericSize_ValueChanged;
                }
            }
            else
            {
                transformPanel.Visible = false;
                spherePanel.Visible = false;
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

        private void SceneGraphTree_MouseClick(object sender, MouseEventArgs e)
        {
            var node = sceneGraphTree.GetNodeAt(e.Location);
            if (node == null)
            {
                sceneGraphTree.HideSelection = true;
                sceneGraphTree.SelectedNode = null;
            }
        }


#endregion

#region Component Panel

        public void InitTransformPanel()
        {
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

        private void InitSpherePanel()
        {
            spherePanel.Visible = false;

            numericSize.ValueChanged += NumericSize_ValueChanged;
            numericSubdivision.ValueChanged += NumericSubdivision_ValueChanged;
        }

        private void NumericSubdivision_ValueChanged(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                if (_activeMesh.Mesh is Sphere)
                {
                    var sphere = _activeMesh.Mesh as Sphere;

                    var s = PrimitiveObjectFactory.Sphere(sphere.Size, (int)numericSubdivision.Value, sphere.Type);
                    _activeMesh.SetMesh(s);
                }
            }
        }

        private void NumericSize_ValueChanged(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                if (_activeMesh.Mesh is Sphere)
                {
                    var sphere = _activeMesh.Mesh as Sphere;

                    var s = PrimitiveObjectFactory.Sphere((float)numericSize.Value, sphere.Subdivision, sphere.Type);
                    _activeMesh.SetMesh(s);
                }
            }
        }

#endregion

#region Drawing

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Front);

            _camera.IsOrtho = !_isPerspective;

            _SceneGraph.RenderAll(_camera, _renderMode);

            GL.Flush();
            GLControl.SwapBuffers();
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

        private void menuProcessSP_Click(object sender, EventArgs e)
        {

            if (_activeMesh != null)
            {
                var mesh = _activeMesh.Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.ShortestPath);
                _frmProcess.OnResultReturned += ProcessResultReturned;
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
                _frmProcess.OnResultReturned += ProcessResultReturned;
                _frmProcess.ShowDialog();
            }
        }

        private void MenuProcessParametrization_Click(object sender, EventArgs e)
        {
            if (_activeMesh != null)
            {
                var mesh = _activeMesh.Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.Parametrization);
                _frmProcess.OnResultReturned += ProcessResultReturned;
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
                    obj.Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                    obj.Mesh.Name = obj.Name;

                    sceneGraphTree.SelectedNode = null;
                    _SceneGraph.AddObject(obj);
                }
            }
        }

        private void MenuAddSphereTetra_Click(object sender, EventArgs e)
        {
            var sphere = PrimitiveObjectFactory.Sphere(1, 4, eSphereGenerationType.Tetrahedron);
            var sphereMesh = new MeshRenderer(sphere, "Sphere");
            _SceneGraph.AddObject(sphereMesh);
            
        }

        private void MenuAddSphereCube_Click(object sender, EventArgs e)
        {
            var sphere = PrimitiveObjectFactory.Sphere(1, 4, eSphereGenerationType.Cube);
            var sphereMesh = new MeshRenderer(sphere, "Sphere");
            _SceneGraph.AddObject(sphereMesh);
        }

        private void MenuAddSphereIcosahedron_Click(object sender, EventArgs e)
        {
            var sphere = PrimitiveObjectFactory.Sphere(1, 4, eSphereGenerationType.icosahedron);
            var sphereMesh = new MeshRenderer(sphere, "Sphere");
            _SceneGraph.AddObject(sphereMesh);
        }

        private void MenuAddTetrahedron_Click(object sender, EventArgs e)
        {
            var tetrahedron = new MeshRenderer(PrimitiveObjectFactory.Tetrahedron(1), "Tetrahedron");
            _SceneGraph.AddObject(tetrahedron);
        }

        private void MenuAddBottomlessPyramid_Click(object sender, EventArgs e)
        {
            var pyramid = new MeshRenderer(PrimitiveObjectFactory.PyramidNoBottom(1, 1, new Vector3(0.4f, 0.4f, 0.2f)), "Bottomless Pyramid");
            _SceneGraph.AddObject(pyramid);
        }

        private void MenuAddPyramid_Click(object sender, EventArgs e)
        {
            var pyramid = new PrimitiveRenderer(PrimitiveObjectFactory.Pyramid(1, 1, Vector2.Zero), "Pyramid");
            _SceneGraph.AddObject(pyramid);
            
        }

        private void MenuAddCube_Click(object sender, EventArgs e)
        {
            var cube = new MeshRenderer(PrimitiveObjectFactory.Cube(1), "Cube");
            _SceneGraph.AddObject(cube);
            
        }

#endregion

#region Toolbar

        private void ToolbarProjectionMode_Click(object sender, EventArgs e)
        {
            _isPerspective = !_isPerspective;
            if (_isPerspective)
            {
                toolbarProjectionMode.Image = Resources.Perspective2;
                return;
            }
            toolbarProjectionMode.Image = Resources.Ortho;
        }

        private void ToolbarWireframe_Click(object sender, EventArgs e)
        {
            if (toolbarWireframe.Checked)
            {
                _renderMode |= eRenderMode.wireFrame;
                return;
            }
            _renderMode ^= eRenderMode.wireFrame;
        }

        private void ToolbarShaded_Click(object sender, EventArgs e)
        {
            if (toolbarShaded.Checked)
            {
                _renderMode |= eRenderMode.shaded;
                return;
            }
            _renderMode ^= eRenderMode.shaded;
        }

        private void ToolbarPoint_Click(object sender, EventArgs e)
        {
            if (toolbarPoint.Checked)
            {
                _renderMode |= eRenderMode.pointCloud;
                return;
            }
            _renderMode ^= eRenderMode.pointCloud;
        }

        private void ToolbarIsBlinn_Click(object sender, EventArgs e)
        {
            if (_SceneGraph != null)
            {
                _SceneGraph.IsBlinnPhong = !_SceneGraph.IsBlinnPhong;
            }
        }

#endregion


    }

}
