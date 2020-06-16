using Engine.Core;
using Engine.GLApi;
using Engine.Processing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VertexBenderCS.Forms
{

    public partial class MainWin : Form
    {
        //this.GLControl = new GLControl(new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8,8,8,8), 24, 8, 8));

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

        private Camera _camera;
        private CameraController _cameraController;
        private SceneGraph _SceneGraph;
        //private SceneTools _SceneTools;
        private Transform _selectedTransform;

        private FrmProcess _frmProcess;
        private FrmFinalize _frmFinalize;

        private bool _isFirstMouse;
        private float _mouseX;
        private float _mouseY;
        private System.Timers.Timer _timer;
        private eRenderMode _renderMode;
        private bool _isPerspective;

        private bool _Update = false;

        private bool _editMode;
        private bool _mouseOnGL;
        private int _direction = -1;

        private Dictionary<Transform, IsoCurveOutput> _IsoCurveOutputs;

        private OpenTK.Input.KeyboardState KeyState { get; set; }
        private OpenTK.Input.MouseState MouseState { get; set; }

        private void Update(object sender, System.Timers.ElapsedEventArgs e)
        {
            InvokeIfRequired(GLControl, () =>
            {
                if (GLControl.Focused)
                {
                    _cameraController.Navigate(KeyState);
                    KeyState = OpenTK.Input.Keyboard.GetState();
                    MouseState = OpenTK.Input.Mouse.GetCursorState();
                    MouseState = OpenTK.Input.Mouse.GetState();
                    ApplyEdit();
                }

                GLControl.Invalidate();
            });
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

        #region Initialization

        public MainWin()
        {
            InitializeComponent();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PrepareUI();
            SubscribeEvents();
            //#if DEBUG
            SetupTestScene();
            //#endif
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

            GL.ClearColor(Color.FromArgb(66, 167, 250));
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

            _timer = new System.Timers.Timer(8);
            _timer.Elapsed += Update;
            _timer.Start();

            _frmProcess = null;
            _frmFinalize = null;
            _SceneGraph = new SceneGraph();
            _camera = new Camera(GLControl.Width, GLControl.Height)
            {
                Position = new Vector3(7.3f, 7.3f, 7.3f)
            };
            _cameraController = new CameraController(_camera);
            _IsoCurveOutputs = new Dictionary<Transform, IsoCurveOutput>();
            _isPerspective = true;

            _renderMode = eRenderMode.shaded;

            InitTransformPanel();
            InitSpherePanel();
            InitVolumeRendererPanel();
            InitMeshRendererPanel();
            InitializeDirLightSegment();
        }

        private void InitializeDirLightSegment()
        {
            directLightX.Value = (int)MathHelper.RadiansToDegrees(_SceneGraph.DirectionalLightX);
            directLightY.Value = (int)MathHelper.RadiansToDegrees(_SceneGraph.DirectionalLightY);
            directLightZ.Value = (int)MathHelper.RadiansToDegrees(_SceneGraph.DirectionalLightZ);

            labelDirLightX.Text = directLightX.Value.ToString();
            labelDirLightY.Text = directLightY.Value.ToString();
            labelDirLightZ.Text = directLightZ.Value.ToString();

        }

        public void SubscribeEvents()
        {
            GLEvents();
            MenuEvents();
            ToolbarEvents();
            SceneGraphEvents();

            Logger.OnItemLogged += Logger_OnItemLogged;
            Logger.OnLogCleaned += Logger_OnLogCleaned;







            // temp

        }

        private void GLEvents()
        {
            GLControl.MouseClick += GLControl_MouseClick;
            GLControl.KeyDown += GLControl_KeyDown;
            GLControl.KeyUp += GLControl_KeyUp;
            GLControl.Resize += GLControl_Resize;
            GLControl.Paint += GLControl_Paint;
            GLControl.MouseDown += GLControl_MouseDown;
            GLControl.MouseMove += GLControl_MouseMove;
            GLControl.MouseWheel += GLControl_MouseWheel;
            GLControl.MouseEnter += GLControl_MouseEnter;
            GLControl.MouseLeave += GLControl_MouseLeave;
        }

        private void MenuEvents()
        {
            menuImportOff.Click += MenuImport_Click;
            menuImportVol.Click += MenuImportVol_Click;
            menuImportNifti.Click += MenuImportNifti_Click;
            menuImportVolAsPC.Click += MenuImportVolAsPC_Click;

            menuProcessSP.Click += MenuProcessSP_Click;
            menuProcessGC.Click += MenuProcessGC_Click;
            menuProcessDescriptor.Click += MenuProcessDescriptor_Click;
            menuProcessParametrization.Click += MenuProcessParametrization_Click;
            menuProcessPostProcessing.Click += MenuProcessPostProcessing_Click;
            menuExit.Click += MenuExit_Click;

            menuIsoCurveExport.Click += MenuIsoCurveExport_Click;
            menuOffExport.Click += MenuOffExport_Click;
            menuStlExport.Click += MenuStlExport_Click;
            menuSTlBinExport.Click += MenuSTlBinExport_Click;

            menuAddCube.Click += MenuAddCube_Click;
            menuAddPyramid.Click += MenuAddPyramid_Click;
            menuAddBottomlessPyramid.Click += MenuAddBottomlessPyramid_Click;
            menuAddTetrahedron.Click += MenuAddTetrahedron_Click;
            menuAddSphereCube.Click += MenuAddSphereCube_Click;
            menuAddSphereTetra.Click += MenuAddSphereTetra_Click;
            menuAddSphereIcosahedron.Click += MenuAddSphereIcosahedron_Click;
        }


        private void ToolbarEvents()
        {
            toolbarProjectionMode.Click += ToolbarProjectionMode_Click;
            toolbarShaded.Click += ToolbarShaded_Click;
            toolbarWireframe.Click += ToolbarWireframe_Click;
            toolbarPoint.Click += ToolbarPoint_Click;
            toolbarShowNormals.Click += ToolbarShowNormals_Click;

            btnShowDirectLight.Click += BtnShowDirectLight_Click;
            btnShowHeadLight.Click += BtnShowHeadLight_Click;
            btnShowGrid.Click += BtnShowGrid_Click;

            directLightX.ValueChanged += DirectLightX_ValueChanged;
            directLightY.ValueChanged += DirectLightY_ValueChanged;
            directLightZ.ValueChanged += DirectLightZ_ValueChanged;     
        }


        private void SceneGraphEvents()
        {
            sceneGraphTree.AfterSelect += SceneGraphTree_AfterSelect;
            sceneGraphTree.KeyDown += SceneGraphTree_KeyDown;
            sceneGraphTree.MouseDown += SceneGraphTree_MouseClick;
            sceneGraphTree.Leave += SceneGraphTree_LostFocus;

            _SceneGraph.OnItemAdded += SceneGraph_OnItemAdded;
            _SceneGraph.OnItemDeleted += SceneGraph_OnItemDeleted;
            _SceneGraph.OnSceneCleared += SceneGraph_OnSceneCleared;
        }

        #endregion

        #region Tests

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                var mr = (_selectedTransform as MeshRenderer).Mesh;

                HoleFiller filler = new HoleFiller(mr);
                filler.FillHoles();

                //Algorithm.Smoothen(ref mr, 5);

                var meshrend = new MeshRenderer(mr, "final")
                {
                    //Position = new Vector3(0, 0, -0.05f),
                    EnableCull = false,
                    Color = new Vector4(1, 0, 0, 1)
                };
                _SceneGraph.AddObject(meshrend);
            }
        }



        private void SetupTestScene()
        {
            //GeometryShaderTest();
            //UltrasonLoadTest();
            //ComputeTest();
            //HoleFillTest();
            //HoleFillTestBasic();
            //CoarseTri();
            PointCloudTest();
        }

        private void StarTest()
        {
            var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\Desktop\star-shaped-meshes\teddy.off"), "test");
            //var mesh = new MeshRenderer(ObjectLoader.LoadOff(@"C:\Users\ozgun\Desktop\MeshsegBenchmark-1.0\data\off\169-2.off"), "test");
            //mesh.Scale = Vector3.One * 10;

            //_activeMesh = mesh;

            var center = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "center");
            center.Position = mesh.Mesh.Center(); //- new Vector3(0, 0.04f, 0);


            center.Color = new Vector4(1, 0, 0, 1);

            var m2 = Algorithm.SphereTest(mesh.Mesh, 250);
            var m3 = Algorithm.SphereTest(mesh.Mesh, 1);

            var sphere2 = new Mesh();
            var sphere3 = new Mesh();

            for (int i = 0; i < m2.PointsOnSphere.Count; i++)
            {
                sphere2.Vertices.Add(i, new Vertex(i, m2.PointsOnSphere[i], m2.Normals[i]));
                sphere3.Vertices.Add(i, new Vertex(i, m3.PointsOnSphere[i], m3.Normals[i]));
            }
            sphere2.Triangles = mesh.Mesh.Triangles;
            sphere3.Triangles = mesh.Mesh.Triangles;

            var renderer2 = new MeshRenderer(sphere2, "sphere2")
            {
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Position = mesh.Position + new Vector3(0.05f, 0.0f, 0.0f),
                //Scale = mesh.Scale
            };
            _SceneGraph.AddObject(renderer2);

            var renderer3 = new MeshRenderer(sphere3, "sphere3")
            {
                Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                Position = mesh.Position + new Vector3(0.1f, 0.0f, 0.0f),
                // Scale = mesh.Scale
            };
            //_SceneGraph.AddObject(renderer3);

            for (int i = 0; i < m2.Center.Count; i++)
            {
                var c2 = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.001f), "c2");
                c2.Position = m2.Center[i];
                c2.Color = new Vector4(0, (i / (float)(m2.Center.Count - 1)), 0, 1);
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
            _selectedTransform = mesh;
            _SceneGraph.AddObject(center);
        }

        private void GeometryShaderTest()
        {
            var mesh = new Mesh();
            mesh.Vertices.Add(0, new Vertex(0, Vector3.Zero));
            var pointCloud = new PointCloudRenderer(mesh, new List<short>() { 255 }, 1);
            _SceneGraph.AddObject(pointCloud);
        }

        private void ComputeTest()
        {
            var output = ObjectLoader.LoadVol(@"C:\Users\ozgun\Desktop\IMG_20200227_6_5.vol");

            sceneGraphTree.SelectedNode = null;

            var volRenderer = new VolumeRenderer(output, "Compute")
            {
                Intensity = 60,
                DownSample = 6,
                Method = eMarchMethod.GpuBoost,
                SmoothenRadius = 0.01f
            };
            volRenderer.Compute();
            _SceneGraph.AddObject(volRenderer);

            sceneGraphTree.SelectedNode = null;

            var sphere = PrimitiveObjectFactory.Sphere(1, 4, eSphereGenerationType.Cube);
            //_sphereRenderer = new MeshRenderer(sphere, Shader.Unlit, "Sphere")
            //{
            //    Scale = new Vector3(0.01f, 0.01f, 0.01f)
            //};
            //_SceneGraph.AddObject(_sphereRenderer);
            //_sphereRenderer.IsEnabled = false;

        }

        private void HoleFillTest()
        {
            var output = ObjectLoader.LoadVol(@"C:\Users\ozgun\Desktop\IMG_20200227_6_5.vol");

            sceneGraphTree.SelectedNode = null;

            var volRenderer = new VolumeRenderer(output, "Compute")
            {
                Intensity = 60,
                DownSample = 6,
                Method = eMarchMethod.GpuBoost,
                SmoothenRadius = 0.01f
            };
            volRenderer.Compute();
            _SceneGraph.AddObject(volRenderer);

            var mesh = volRenderer.FinalizeMesh(true, true, false, 4, 4);
            Algorithm.FillHoles(ref mesh);

            var meshrend = new MeshRenderer(mesh, "final")
            {
                Position = new Vector3(0, 0, 0.4f),
                Color = volRenderer.Color
            };
            _SceneGraph.AddObject(meshrend);

        }

        private void CoarseTri()
        {
            var mesh = ObjectLoader.LoadOff(@"C:\Users\ozgun\Desktop\pumpkin.off");

            var meshRenderer = new MeshRenderer(mesh, "face");
            _SceneGraph.AddObject(meshRenderer);
            meshRenderer.EnableCull = false;

            //Mesh mesh = new Mesh();
            //Dictionary<int, Vertex> b = new Dictionary<int, Vertex>();
            HoleFiller filler = new HoleFiller(mesh);
            filler.FillHoles(5);
            meshRenderer.ShowNormals = false;
            meshRenderer.SetMesh(mesh);

            //var output = Algorithm.AverageGeodesicDistance(new Graph(mesh), 8, 3350, (x) => { });
            //float max = output.Distances.Max();

            //var color = new Vector3[output.Distances.Length];
            //for (int i = 0; i < color.Length; i++)
            //{
            //    color[i] = ProcessOutputHandler.ColorPixelVector(output.Distances[i], max) * 0.5f;
            //}

            //meshRenderer.SetColorBuffer(color);

            //var meshrend = new MeshRenderer(mesh, "final")
            //{
            //    Position = new Vector3(0, 0, -0.05f),
            //    EnableCull = false
            //};
            //_SceneGraph.AddObject(meshrend);
        }

        private void PointCloudTest()
        {
            var output = ObjectLoader.LoadVol(@"C:\Users\ozgun\Desktop\IMG_20200227_6_1.vol", out VolOutput volOutput);
            //var output2 = ObjectLoader.LoadNifti(@"C:\Users\ozgun\Desktop\ceng599 project\CT\4821\4823\20130118_0933183DHEADs002a002.nii", out VolOutput volOutput2);
            var pointRenderer = new PointCloudRenderer(output, volOutput.IntensityMap.Select(x => x.Value).ToList(), volOutput.Spacing);
            //var pointRenderer2 = new PointCloudRenderer(output2, volOutput2.IntensityMap.Select(x => x.Value).ToList(), volOutput2.Spacing);

            
            sceneGraphTree.SelectedNode = null;
            _SceneGraph.AddObject(pointRenderer);
            sceneGraphTree.SelectedNode = null;
            //_SceneGraph.AddObject(pointRenderer2);
            
        }

        #endregion

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
            if (_selectedTransform != null)
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

                var mesh = _selectedTransform as MeshRenderer;

                DrawPath(mesh, output.Path, color, output.Type.ToString());

                //Logger.Append(output.Info);
            }
        }

        private void DisplaySamplingOutput(SampleOutput output)
        {
            if (_selectedTransform != null)
            {
                foreach (var child in _selectedTransform.Children)
                {
                    _SceneGraph.DeleteObject(child);
                }

                List<MeshRenderer> points = new List<MeshRenderer>();
                for (int i = 0; i < output.SampleIndices.Count; i++)
                {
                    MeshRenderer obj = new MeshRenderer(PrimitiveObjectFactory.Sphere(0.05f, 4, eSphereGenerationType.Cube), Shader.Standard, "sample-" + i + 1)
                    {
                        //Color = new Vector4(output.SamplePoints[i].Coord * 0.5f, 1.0f)
                        Color = new Vector4(1.0f, 0.0f, 1.0f, 0.8f),
                        EnableBlend = true
                    };
                    obj.Position = output.SamplePoints[i].Coord;

                    //obj.Position = new Vector3(_activeMesh.ModelMatrix * new Vector4(obj.Position));
                    obj.Position += _selectedTransform.Position;


                    // bad block but keep it for now..
                    obj.Parent = _selectedTransform;

                    points.Add(obj);
                    _SceneGraph.AddObject(obj);
                }

                //Logger.Append(output.Info);
            }
        }

        private void DisplayIsoCurveOutput(IsoCurveOutput output)
        {
            if (_selectedTransform != null)
            {
                if (_IsoCurveOutputs.ContainsKey(_selectedTransform))
                {
                    _IsoCurveOutputs.Remove(_selectedTransform);
                }
                _IsoCurveOutputs.Add(_selectedTransform, output);

                foreach (var child in _selectedTransform.Children)
                {
                    _SceneGraph.DeleteObject(child);
                }

                var mesh = _selectedTransform as MeshRenderer;

                PrimitiveRenderer indicator = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.05f), Shader.Standard, "source")
                {
                    Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                };
                indicator.Position = mesh.Mesh.Vertices[output.SourceIndex].Coord;

                indicator.Parent = _selectedTransform;
                _SceneGraph.AddObject(indicator);


                for (int i = 0; i < output.IsoCurveDistances.Length; i++)
                {
                    var line = new LineRenderer(output.IsoCurves[i], "sample-" + i);
                    line.Position = _selectedTransform.Position;
                    line.Rotation = _selectedTransform.Rotation;
                    line.Scale = _selectedTransform.Scale;

                    line.Parent = _selectedTransform;
                    _SceneGraph.AddObject(line);

                    line.Color = new Vector4(1, 0, 0, 1);
                }

                UpdateChart(output);

                //Logger.Append(output.Info);
            }
        }

        private void DisplayAverageGeodesicOutput(AverageGeodesicOutput output)
        {
            if (_selectedTransform != null)
            {
                float max = output.Distances.Max();

                var color = new Vector3[output.Distances.Length];
                for (int i = 0; i < color.Length; i++)
                {
                    color[i] = ProcessOutputHandler.ColorPixelVector(output.Distances[i], max) * 0.5f;
                }
                var mesh = _selectedTransform as MeshRenderer;

                mesh.SetColorBuffer(color);
                //Logger.Append(output.Info);
            }
        }

        private void DisplayDiscParametrizationOutput(DiscParameterizeOutput output)
        {
            if (_selectedTransform != null)
            {
                foreach (var child in _selectedTransform.Children)
                {
                    _SceneGraph.DeleteObject(child);
                }

                var mesh = _selectedTransform as MeshRenderer;

                mesh.SetMesh(output.Mesh);
                var outputmesh = new Mesh();

                for (int i = 0; i < output.Output.Count; i++)
                {
                    var v = new Vertex(i, new Vector3(output.Output[i].X, 0, output.Output[i].Y), Vector3.UnitY);
                    outputmesh.Vertices.Add(v.Id, v);
                }
                outputmesh.Triangles = mesh.Mesh.Triangles;

                var tex = Texture.LoadTexture(@"Resources\Image\UV1024.png", eTextureType.Diffuse);
                //var tex = Texture.LoadTexture(@"Resources\Image\tile.png", eTextureType.Diffuse);

                var renderer = new MeshRenderer(outputmesh, $"Disc-{_selectedTransform.Name}")
                {
                    Position = _selectedTransform.Position + new Vector3(0.2f, 0.0f, 0.0f),
                    EnableCull = false
                };
                renderer.Parent = _selectedTransform;

                renderer.DiffuseTexture = tex;
                mesh.DiffuseTexture = tex;

                mesh.SetTextureBuffer(output.NormalizedUVCoords());
                renderer.SetTextureBuffer(output.NormalizedUVCoords());

                _SceneGraph.AddObject(renderer);


                DrawPath(mesh, output.BoundaryPath, new Vector4(1, 0, 0, 0), $"path-{_selectedTransform.Name}");

            }
        }

        private void DisplaySphereParametrizationOutput(SphereParameterizeOutput output)
        {
            if (_selectedTransform != null)
            {
                //foreach (var child in _activeMesh.Children)
                //{
                //    _SceneGraph.DeleteObject(child);
                //}

                var mesh = _selectedTransform as MeshRenderer;

                var sphere = new Mesh();

                for (int i = 0; i < output.PointsOnSphere.Count; i++)
                {
                    sphere.Vertices.Add(i, new Vertex(i, output.PointsOnSphere[i], output.Normals[i]));
                }

                sphere.Triangles = mesh.Mesh.Triangles;

                var size = Box3.CalculateBoundingBox(output.PointsOnSphere).Size;

                var renderer = new MeshRenderer(sphere, "sphere")
                {
                    Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    Position = _selectedTransform.Position + Vector3.UnitX * size
                };
                renderer.Rotation = _selectedTransform.Rotation;


                var cube = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(0.01f * size), "center");
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
            if (_selectedTransform != null)
            {

                //var m = Algorithm.ParameterizeMeshCutSeam(_activeMesh.Mesh, (asd) => { });
                var meshrend = new MeshRenderer(output.Cutmesh, "asdasd");
                _SceneGraph.AddObject(meshrend);
                _selectedTransform = meshrend;
                DisplayDiscParametrizationOutput(output.Disc);
                DisplayShortestPathOutput(output.ShortestPath);
            }
        }

        private void DisplayPostProcessOutput(PostProcessOutput output)
        {
            var meshrend = new MeshRenderer(output.newMesh, "post Processed");
            _SceneGraph.AddObject(meshrend);
            sceneGraphTree.SelectedNode = null;
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
                case eOutputType.PostProcessing:
                    DisplayPostProcessOutput((PostProcessOutput)output);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region GlControl Events

        private void GLControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (KeyState.IsKeyDown(OpenTK.Input.Key.LControl))
            {
                if (_editMode)
                {
                    _SceneGraph.SceneTools.VolumeEditor.Scale += (Vector3.One * 0.001f * e.Delta / Math.Abs(e.Delta));
                }

                var volRend = (_selectedTransform as VolumeRenderer);
                if (volRend != null)
                {
                    volRend.SmoothenRadius = _SceneGraph.SceneTools.VolumeEditor.Scale.X;
                }
                return;
            }
            _cameraController.Zoom(e.Delta, KeyState.IsKeyDown(OpenTK.Input.Key.ShiftLeft));
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
            else if (e.Button == MouseButtons.Right && KeyState.IsKeyDown(OpenTK.Input.Key.ControlLeft))
            {
                _cameraController.OrbitAround(offsetX, offsetY, _camera.Position + _camera.Front * 2);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _cameraController.Rotate(offsetX, offsetY);
            }
            _mouseX = e.X;
            _mouseY = e.Y;

            Status.Text = $"{_camera.Position.X}, {_camera.Position.Y}, {_camera.Position.Z}";

            if (_editMode && _mouseOnGL)
            {
                var volRend = (_selectedTransform as VolumeRenderer);
                if (volRend != null)
                {
                    Vector3 pos = _camera.ScreenToWorld(_mouseX, _mouseY, GLControl.Width, GLControl.Height);

                    volRend.ComputeIntersection(pos, (pos - _camera.Position).Normalized(), out Vector3 result);
                    _SceneGraph.SceneTools.VolumeEditor.Position = result;
                }
            }
        }

        private void GLControl_MouseClick(object sender, MouseEventArgs e)
        {
            //_mouseX = (float)e.X;
            //_mouseY = (float)e.Y;
            //_isFirstMouse = false;
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
            GLControl c = sender as GLControl;

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

        private void GLControl_MouseLeave(object sender, EventArgs e)
        {
            if (_editMode)
            {
                Cursor.Show();
                _SceneGraph.SceneTools.VolumeEditor.IsEnabled = false;
                _mouseOnGL = false;
            }

        }

        private void GLControl_MouseEnter(object sender, EventArgs e)
        {
            if (_editMode)
            {
                _mouseOnGL = true;
                _SceneGraph.SceneTools.VolumeEditor.IsEnabled = true;
                Cursor.Hide();
            }
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
            //sceneGraphTree.SelectedNode = null;
        }

        private void SceneGraphTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var item = (e.Node as SceneNode).Transform;
            HideAllPanels();

            if (item != null)
            {
                _SceneGraph.SceneTools.SetCartesianPosition(item.Position);

                _selectedTransform = item;
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
                labelTransform.Text = _selectedTransform.Name;
                numericPosX.Value = new decimal(_selectedTransform.Position.X);
                numericPosY.Value = new decimal(_selectedTransform.Position.Y);
                numericPosZ.Value = new decimal(_selectedTransform.Position.Z);

                var q = _selectedTransform.Rotation;
                var rot = _selectedTransform.Rotation.EulerAngles();

                numericRotX.Value = new decimal(rot.X);
                numericRotY.Value = new decimal(rot.Y);
                numericRotZ.Value = new decimal(rot.Z);

                numericScaleX.Value = new decimal(_selectedTransform.Scale.X);
                numericScaleY.Value = new decimal(_selectedTransform.Scale.Y);
                numericScaleZ.Value = new decimal(_selectedTransform.Scale.Z);

                numericScaleX.ValueChanged += NumericScale_ValueChanged;
                numericScaleY.ValueChanged += NumericScale_ValueChanged;
                numericScaleZ.ValueChanged += NumericScale_ValueChanged;

                numericPosX.ValueChanged += NumericPos_ValueChanged;
                numericPosY.ValueChanged += NumericPos_ValueChanged;
                numericPosZ.ValueChanged += NumericPos_ValueChanged;

                numericRotX.ValueChanged += NumericRot_ValueChanged;
                numericRotY.ValueChanged += NumericRot_ValueChanged;
                numericRotZ.ValueChanged += NumericRot_ValueChanged;

                if (_IsoCurveOutputs.TryGetValue(_selectedTransform, out IsoCurveOutput output))
                {
                    UpdateChart(output);
                }

                if (_selectedTransform is IRenderable)
                {
                    var renderable = _selectedTransform as IRenderable;

                    btnColor.Click -= BtnColor_Click;
                    chkCull.CheckedChanged -= ChkCull_CheckedChanged;

                    renderablePanel.Visible = true;
                    btnColor.BackColor = Color.FromArgb
                    (
                        (int)(255 * renderable.Color.W),
                        (int)(255 * renderable.Color.X),
                        (int)(255 * renderable.Color.Y),
                        (int)(255 * renderable.Color.Z)
                    );
                    chkCull.Checked = renderable.EnableCull;

                    btnColor.Click += BtnColor_Click;
                    chkCull.CheckedChanged += ChkCull_CheckedChanged;
                }

                if (_selectedTransform is MeshRenderer)
                {
                    var mesh = _selectedTransform as MeshRenderer;

                    if (mesh.Mesh is Sphere)
                    {
                        var sphere = (mesh.Mesh as Sphere);

                        numericSubdivision.ValueChanged -= NumericSubdivision_ValueChanged;
                        numericSize.ValueChanged -= NumericSize_ValueChanged;

                        spherePanel.Visible = true;

                        numericSize.Value = (decimal)sphere.Size;
                        numericSubdivision.Value = (decimal)sphere.Subdivision;

                        numericSubdivision.ValueChanged += NumericSubdivision_ValueChanged;
                        numericSize.ValueChanged += NumericSize_ValueChanged;
                    }
                }
                else if (_selectedTransform is PointCloudRenderer)
                {
                    var mesh = _selectedTransform as PointCloudRenderer;

                    pointCloudPanel.Visible = true;

                    sliderMax.ValueChanged -= SliderMax_ValueChanged;
                    sliderMin.ValueChanged -= SliderMin_ValueChanged;

                    sliderMax.Value = mesh.Max;
                    sliderMin.Value = mesh.Min;

                    sliderMax.ValueChanged += SliderMax_ValueChanged;
                    sliderMin.ValueChanged += SliderMin_ValueChanged;

                }
                else if (_selectedTransform is VolumeRenderer)
                {
                    var volumeRenderer = _selectedTransform as VolumeRenderer;

                    volumeRendererPanel.Visible = true;

                    intensityMarch.Maximum = volumeRenderer.MaxIntensity;

                    numericMarch.ValueChanged -= NumericMarch_ValueChanged;
                    intensityMarch.ValueChanged -= IntensityMarch_ValueChanged;
                    cmbVolMethod.SelectedValueChanged -= CmbVolMethod_SelectedValueChanged;

                    cmbVolMethod.SelectedIndex = (int)volumeRenderer.Method;
                    intensityMarch.Value = volumeRenderer.Intensity;
                    numericMarch.Value = volumeRenderer.DownSample;

                    numericMarch.ValueChanged += NumericMarch_ValueChanged;
                    intensityMarch.ValueChanged += IntensityMarch_ValueChanged;
                    cmbVolMethod.SelectedValueChanged += CmbVolMethod_SelectedValueChanged; ;

                }
            }
            else
            {
                _SceneGraph.SceneTools.Reset();
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
                HideAllPanels();
            }
        }

        private void SceneGraphTree_LostFocus(object sender, EventArgs e)
        {
            sceneGraphTree.HideSelection = false;
        }

        #endregion

        #region Component Panel

        private void HideAllPanels()
        {

            chartIsoCurve.Hide();
            transformPanel.Visible = false;
            spherePanel.Visible = false;
            pointCloudPanel.Visible = false;
            volumeRendererPanel.Visible = false;
            renderablePanel.Visible = false;
        }

        private void InitTransformPanel()
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
            if (_selectedTransform != null)
            {
                _selectedTransform.Rotation = Quaternion.FromEulerAngles
                (
                    MathHelper.DegreesToRadians((float)numericRotX.Value),
                    MathHelper.DegreesToRadians((float)numericRotY.Value),
                    MathHelper.DegreesToRadians((float)numericRotZ.Value)
                );

                foreach (var child in _selectedTransform.Children)
                {
                    child.Rotation = _selectedTransform.Rotation;
                }

            }
        }

        private void NumericPos_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                _selectedTransform.Position = new Vector3
                (
                    (float)numericPosX.Value,
                    (float)numericPosY.Value,
                    (float)numericPosZ.Value
                );
                _SceneGraph.SceneTools.SetCartesianPosition(_selectedTransform.Position);

                foreach (var child in _selectedTransform.Children)
                {
                    child.Position = _selectedTransform.Position;
                }
            }
        }

        private void NumericScale_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                _selectedTransform.Scale = new Vector3
                (
                    (float)numericScaleX.Value,
                    (float)numericScaleY.Value,
                    (float)numericScaleZ.Value
                );
                foreach (var child in _selectedTransform.Children)
                {
                    child.Scale = _selectedTransform.Scale;
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
            if (_selectedTransform != null)
            {
                var mesh = _selectedTransform as MeshRenderer;
                if (mesh.Mesh is Sphere)
                {
                    var sphere = mesh.Mesh as Sphere;

                    var s = PrimitiveObjectFactory.Sphere(sphere.Size, (int)numericSubdivision.Value, sphere.Type);
                    mesh.SetMesh(s);
                }
            }
        }

        private void NumericSize_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                var mesh = _selectedTransform as MeshRenderer;

                if (mesh.Mesh is Sphere)
                {
                    var sphere = mesh.Mesh as Sphere;

                    var s = PrimitiveObjectFactory.Sphere((float)numericSize.Value, sphere.Subdivision, sphere.Type);
                    mesh.SetMesh(s);
                }
            }
        }

        private void SliderMin_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                var s = sender as TrackBar;
                var mesh = _selectedTransform as PointCloudRenderer;
                if (mesh is PointCloudRenderer)
                {
                    mesh.SetMin(s.Value);
                    sliderMax.Value = Math.Max(s.Value, sliderMax.Value);
                    sliderMinText.Text = s.Value.ToString();
                }
            }
        }

        private void SliderMax_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                var s = sender as TrackBar;
                var mesh = _selectedTransform as PointCloudRenderer;
                if (mesh is PointCloudRenderer)
                {
                    mesh.SetMax(s.Value);
                    sliderMin.Value = Math.Min(s.Value, sliderMin.Value);
                    sliderMaxText.Text = s.Value.ToString();
                }
            }
        }

        private void InitVolumeRendererPanel()
        {
            volumeRendererPanel.Visible = false;

            numericMarch.ValueChanged += NumericMarch_ValueChanged;
            intensityMarch.ValueChanged += IntensityMarch_ValueChanged;
            cmbVolMethod.SelectedValueChanged += CmbVolMethod_SelectedValueChanged;

            btnEdit.Click += BtnEdit_Click;
            btnFinalize.Click += BtnFinalize_Click;

        }

        private void BtnFinalize_Click(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                var volRenderer = (_selectedTransform as VolumeRenderer);

                _frmFinalize = new FrmFinalize(volRenderer);
                _frmFinalize.BtnProcedeClicked += FrmFinalize_BtnProcedeClicked;
                _frmFinalize.ShowDialog();
            }
        }

        private void FrmFinalize_BtnProcedeClicked(Mesh mesh, Vector4 color)
        {
            MeshRenderer renderer = new MeshRenderer(mesh, mesh.Name)
            {
                Color = color,
                EnableCull = false
            };
            renderer.Position += new Vector3(0.0f, 0.0f, 0.4f);
            _SceneGraph.SceneTools.VolumeEditor.IsEnabled = false;
            _editMode = false;
            sceneGraphTree.SelectedNode = null;
            _SceneGraph.AddObject(renderer);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            _editMode = !_editMode;

            if (_editMode)
            {
                _SceneGraph.SceneTools.VolumeEditor.IsEnabled = true;
                Logger.Log("edit mode");
            }
            else
            {
                _SceneGraph.SceneTools.VolumeEditor.IsEnabled = false;
                Logger.Log("not edit mode");
            }
        }

        private void NumericMarch_ValueChanged(object sender, EventArgs e)
        {
            var volRend = (_selectedTransform as VolumeRenderer);

            if (volRend != null)
            {
                volRend.DownSample = (int)numericMarch.Value;
                volRend.Compute();
            }

        }

        private void IntensityMarch_ValueChanged(object sender, EventArgs e)
        {
            var volRend = (_selectedTransform as VolumeRenderer);

            if (volRend != null)
            {
                volRend.Intensity = intensityMarch.Value;
                labelIntensityMarch.Text = intensityMarch.Value.ToString();
                volRend.Compute();
            }
        }

        private void CmbVolMethod_SelectedValueChanged(object sender, EventArgs e)
        {
            var volRend = (_selectedTransform as VolumeRenderer);

            if (volRend != null)
            {
                switch (cmbVolMethod.SelectedIndex)
                {
                    case 0:
                        volRend.Method = eMarchMethod.GpuBoost;
                        break;
                    case 1:
                        volRend.Method = eMarchMethod.GPU;
                        break;
                    case 2:
                        volRend.Method = eMarchMethod.CPU;
                        break;
                }
                volRend.Compute();
            }
        }

        private void InitMeshRendererPanel()
        {
            renderablePanel.Visible = false;

            chkCull.CheckedChanged += ChkCull_CheckedChanged;
            btnColor.Click += BtnColor_Click;
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                var meshRend = _selectedTransform as IRenderable;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    meshRend.Color = new Vector4
                    (
                        colorDialog1.Color.R,
                        colorDialog1.Color.G,
                        colorDialog1.Color.B,
                        colorDialog1.Color.A
                    ) / 255.0f;
                    btnColor.BackColor = colorDialog1.Color;
                }

            }
        }

        private void ChkCull_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedTransform != null)
            {
                var meshRend = _selectedTransform as MeshRenderer;
                meshRend.EnableCull = chkCull.Checked;
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

        private void ApplyEdit()
        {
            if (_editMode && _mouseOnGL)
            {
                var volRend = (_selectedTransform as VolumeRenderer);
                if (volRend != null)
                {
                    if ( MouseState.IsButtonDown(OpenTK.Input.MouseButton.Left))
                    {
                        _direction = -1;
                        if (KeyState.IsKeyDown(OpenTK.Input.Key.LShift))
                        {
                            _direction = 1;
                        }
                        volRend.ComputeVolEditor
                        (
                            _SceneGraph.SceneTools.VolumeEditor.Position,
                            _SceneGraph.SceneTools.VolumeEditor.Scale.X,
                            (float)numericPressure.Value,
                            _direction
                        );
                        volRend.Compute();
                    }
                }
            }
        }

        #endregion

        #region Menu Items

        private void MenuProcessSP_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                var mesh = (_selectedTransform as MeshRenderer).Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.ShortestPath);
                _frmProcess.OnResultReturned += ProcessResultReturned;
                _frmProcess.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a model to process", "No models selected", MessageBoxButtons.OK);
            }
        }

        private void MenuProcessGC_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                var mesh = _selectedTransform as MeshRenderer;

                var a = Algorithm.GaussianCurvature(mesh.Mesh);
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

                mesh.SetColorBuffer(color);

            }
            else
            {
                MessageBox.Show("Please Select a model to process", "No models selected", MessageBoxButtons.OK);
            }
        }

        private void MenuProcessDescriptor_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                var mesh = (_selectedTransform as MeshRenderer).Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.Descriptor);
                _frmProcess.OnResultReturned += ProcessResultReturned;
                _frmProcess.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a model to process", "No models selected", MessageBoxButtons.OK);
            }
        }

        private void MenuProcessParametrization_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                var mesh = (_selectedTransform as MeshRenderer).Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.Parametrization);
                _frmProcess.OnResultReturned += ProcessResultReturned;
                _frmProcess.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a model to process", "No models selected", MessageBoxButtons.OK);
            }
        }

        private void MenuProcessPostProcessing_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                var mesh = (_selectedTransform as MeshRenderer).Mesh;
                _frmProcess = new FrmProcess(mesh, eProcessCoreType.PostProcessing);
                _frmProcess.OnResultReturned += ProcessResultReturned;
                _frmProcess.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a model to process", "No models selected", MessageBoxButtons.OK);
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

        private void MenuOffExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "OFF files (*.off)|*.off|All files (*.*)|*.*";
            dialog.Title = "Save off Files";
            dialog.DefaultExt = "off";
            dialog.FilterIndex = 2;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProcessOutputHandler.SaveOffFile(((sceneGraphTree.SelectedNode as SceneNode).Transform as MeshRenderer).Mesh, dialog.FileName);
            }

        }

        private void MenuSTlBinExport_Click(object sender, EventArgs e)
        {

        }

        private void MenuStlExport_Click(object sender, EventArgs e)
        {
            var meshRend = (_selectedTransform as MeshRenderer);

            if (meshRend != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "STL files (*.stl)|*.stl|All files (*.*)|*.*";
                dialog.Title = "Save STL Files";
                dialog.DefaultExt = "stl";
                dialog.FilterIndex = 2;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ProcessOutputHandler.SaveSTLFile(meshRend.Mesh, dialog.FileName);
                }
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuImport_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog
            {
                ValidateNames = true,
                Filter = "Off files (*.off)|*.off|All Files(*.*)|*.* "
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var v = d.FileName.Substring(d.FileName.Length - 4);
                    if (v.ToLower() == ".off")
                    {
                        var split = d.FileName.Split(new char[] { '\\' });
                        var name = split[split.Length - 1];

                        //_SceneGraph.Clean();
                        var obj = new MeshRenderer(ObjectLoader.LoadOff(d.FileName), name);
                        obj.Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                        obj.Mesh.Name = obj.Name;

                        sceneGraphTree.SelectedNode = null;
                        _SceneGraph.AddObject(obj);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Logger.Log(ex.Message);
                }
            }
            d.Dispose();
        }

        private void MenuImportVol_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog
            {
                ValidateNames = true,
                Filter = "VOL files (*.vol)|*.vol|All Files(*.*)|*.* "
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var v = d.FileName.Substring(d.FileName.Length - 4);
                    if (v.ToLower() == ".vol")
                    {
                        var split = d.FileName.Split(new char[] { '\\' });
                        var name = split[split.Length - 1];

                        var output = ObjectLoader.LoadVol(d.FileName);

                        var volRenderer = new VolumeRenderer(output, name)
                        {
                            Intensity = 60,
                            DownSample = 6,
                            Method = eMarchMethod.GpuBoost,
                            SmoothenRadius = 0.0001f
                        };
                        volRenderer.Compute();
                        _SceneGraph.AddObject(volRenderer);

                        sceneGraphTree.SelectedNode = null;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Logger.Log(ex.Message);
                }
            }
            d.Dispose();
        }

        private void MenuImportVolAsPC_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog
            {
                ValidateNames = true,
                Filter = "VOL files (*.vol)|*.vol|All Files(*.*)|*.* "
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var v = d.FileName.Substring(d.FileName.Length - 4);
                    if (v.ToLower() == ".vol")
                    {
                        var split = d.FileName.Split(new char[] { '\\' });
                        var name = split[split.Length - 1];

                        var mesh = ObjectLoader.LoadVol(d.FileName, out VolOutput output);

                        var pointCloud = new PointCloudRenderer(mesh, output.IntensityMap.Select(x => x.Value).ToList(), output.Spacing)
                        {
                            IsEnabled = true
                        };
                        _SceneGraph.AddObject(pointCloud);

                        sceneGraphTree.SelectedNode = null;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Logger.Log(ex.Message);
                }
            }
            d.Dispose();
        }


        private void MenuImportNifti_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog
            {
                ValidateNames = true,
                Filter = "Nifti files (*.nii)|*.nii|All Files(*.*)|*.* "
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var v = d.FileName.Substring(d.FileName.Length - 4);
                    if (v.ToLower() == ".nii")
                    {
                        var split = d.FileName.Split(new char[] { '\\' });
                        var name = split[split.Length - 1];

                        var output = ObjectLoader.LoadNifti(d.FileName);

                        var volRenderer = new VolumeRenderer(output, name)
                        {
                            Intensity = 60,
                            DownSample = 6,
                            Method = eMarchMethod.GpuBoost,
                            SmoothenRadius = 0.0001f
                        };
                        volRenderer.Compute();
                        _SceneGraph.AddObject(volRenderer);

                        sceneGraphTree.SelectedNode = null;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Logger.Log(ex.Message);
                }

            }
            d.Dispose();
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
            var cube = new PrimitiveRenderer(PrimitiveObjectFactory.Cube(1), "Cube");
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

        private void BtnShowGrid_Click(object sender, EventArgs e)
        {
            _SceneGraph.SceneTools.ShowGrid = btnShowGrid.Checked;
        }

        private void ToolbarShowNormals_Click(object sender, EventArgs e)
        {
            if (_selectedTransform is MeshRenderer)
            {
                (_selectedTransform as MeshRenderer).ShowNormals = !(_selectedTransform as MeshRenderer).ShowNormals;
            }
            else
            {
                MessageBox.Show("Please Select a meshrenderer", "No meshes selected", MessageBoxButtons.OK);
            }
        }

        private void BtnShowHeadLight_Click(object sender, EventArgs e)
        {
            _SceneGraph.ShowHeadLight = btnShowHeadLight.Checked;
        }

        private void BtnShowDirectLight_Click(object sender, EventArgs e)
        {
            _SceneGraph.ShowDirectionalLight = btnShowDirectLight.Checked;
            directLightX.Enabled = btnShowDirectLight.Checked;
            directLightY.Enabled = btnShowDirectLight.Checked;
            directLightZ.Enabled = btnShowDirectLight.Checked;
        }


        private void DirectLightX_ValueChanged(object sender, EventArgs e)
        {
            _SceneGraph.DirectionalLightX = MathHelper.DegreesToRadians(directLightX.Value);
            labelDirLightX.Text = directLightX.Value.ToString();
        }

        private void DirectLightY_ValueChanged(object sender, EventArgs e)
        {
            _SceneGraph.DirectionalLightY = MathHelper.DegreesToRadians(directLightY.Value);
            labelDirLightY.Text = directLightY.Value.ToString();
        }

        private void DirectLightZ_ValueChanged(object sender, EventArgs e)
        {
            _SceneGraph.DirectionalLightZ = MathHelper.DegreesToRadians(directLightZ.Value);
            labelDirLightZ.Text = directLightZ.Value.ToString();
        }

        #endregion



    }

}
