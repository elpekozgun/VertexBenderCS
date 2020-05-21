namespace VertexBenderCS.Forms
{
    partial class MainWin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.GLControl = new OpenTK.GLControl();
            this.sceneGraphTree = new System.Windows.Forms.TreeView();
            this.Log = new System.Windows.Forms.TextBox();
            this.chartIsoCurve = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.processWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolbarProjectionMode = new System.Windows.Forms.ToolStripButton();
            this.toolbarShaded = new System.Windows.Forms.ToolStripButton();
            this.toolbarWireframe = new System.Windows.Forms.ToolStripButton();
            this.toolbarPoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolbarIsBlinn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportOff = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportObj = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportDae = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportVol = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportDcm = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIsoCurveExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOffExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.processMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessSP = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessGC = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessDescriptor = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessParametrization = new System.Windows.Forms.ToolStripMenuItem();
            this.addMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddCube = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddTetrahedron = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddPyramid = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddBottomlessPyramid = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddSphereTetra = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddSphereCube = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddSphereIcosahedron = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.transformPanel = new System.Windows.Forms.Panel();
            this.numericScaleZ = new System.Windows.Forms.NumericUpDown();
            this.numericScaleY = new System.Windows.Forms.NumericUpDown();
            this.numericScaleX = new System.Windows.Forms.NumericUpDown();
            this.numericRotZ = new System.Windows.Forms.NumericUpDown();
            this.numericRotY = new System.Windows.Forms.NumericUpDown();
            this.numericRotX = new System.Windows.Forms.NumericUpDown();
            this.numericPosZ = new System.Windows.Forms.NumericUpDown();
            this.numericPosY = new System.Windows.Forms.NumericUpDown();
            this.labelZPos = new System.Windows.Forms.Label();
            this.labelYPos = new System.Windows.Forms.Label();
            this.labelXpos = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.labelRotation = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.numericPosX = new System.Windows.Forms.NumericUpDown();
            this.labelTransform = new System.Windows.Forms.Label();
            this.spherePanel = new System.Windows.Forms.Panel();
            this.numericSubdivision = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericSize = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.sliderMin = new System.Windows.Forms.TrackBar();
            this.pointCloudPanel = new System.Windows.Forms.Panel();
            this.sliderMaxText = new System.Windows.Forms.Label();
            this.sliderMinText = new System.Windows.Forms.Label();
            this.sliderMax = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IntensityMarch = new System.Windows.Forms.TrackBar();
            this.numericMarch = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.chartIsoCurve)).BeginInit();
            this.toolBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.transformPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRotZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPosX)).BeginInit();
            this.spherePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSubdivision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderMin)).BeginInit();
            this.pointCloudPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntensityMarch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMarch)).BeginInit();
            this.SuspendLayout();
            // 
            // GLControl
            // 
            this.GLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLControl.BackColor = System.Drawing.Color.White;
            this.GLControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GLControl.Location = new System.Drawing.Point(255, 97);
            this.GLControl.MinimumSize = new System.Drawing.Size(466, 461);
            this.GLControl.Name = "GLControl";
            this.GLControl.Size = new System.Drawing.Size(1039, 644);
            this.GLControl.TabIndex = 0;
            this.GLControl.VSync = false;
            // 
            // sceneGraphTree
            // 
            this.sceneGraphTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sceneGraphTree.Font = new System.Drawing.Font("Ebrima", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sceneGraphTree.HotTracking = true;
            this.sceneGraphTree.LineColor = System.Drawing.Color.MediumPurple;
            this.sceneGraphTree.Location = new System.Drawing.Point(3, 97);
            this.sceneGraphTree.Name = "sceneGraphTree";
            this.sceneGraphTree.Size = new System.Drawing.Size(250, 336);
            this.sceneGraphTree.TabIndex = 4;
            // 
            // Log
            // 
            this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Log.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Log.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Log.Location = new System.Drawing.Point(3, 744);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            this.Log.Size = new System.Drawing.Size(1291, 135);
            this.Log.TabIndex = 5;
            // 
            // chartIsoCurve
            // 
            this.chartIsoCurve.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.chartIsoCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chartIsoCurve.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.chartIsoCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartIsoCurve.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            this.chartIsoCurve.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartIsoCurve.BorderlineColor = System.Drawing.Color.MediumAquamarine;
            this.chartIsoCurve.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartIsoCurve.BorderlineWidth = 2;
            this.chartIsoCurve.BorderSkin.BackColor = System.Drawing.Color.DimGray;
            this.chartIsoCurve.BorderSkin.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Center;
            this.chartIsoCurve.BorderSkin.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartIsoCurve.BorderSkin.BorderWidth = 20;
            this.chartIsoCurve.BorderSkin.PageColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.LabelStyle.Interval = 0D;
            chartArea1.AxisY.LabelStyle.IntervalOffset = 0D;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.Name = "isoCurve-Distances";
            this.chartIsoCurve.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            legend1.ForeColor = System.Drawing.Color.Gainsboro;
            legend1.InterlacedRowsColor = System.Drawing.Color.White;
            legend1.Name = "Legend1";
            legend1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chartIsoCurve.Legends.Add(legend1);
            this.chartIsoCurve.Location = new System.Drawing.Point(920, 102);
            this.chartIsoCurve.Margin = new System.Windows.Forms.Padding(0);
            this.chartIsoCurve.Name = "chartIsoCurve";
            this.chartIsoCurve.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            this.chartIsoCurve.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series1.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Center;
            series1.BackSecondaryColor = System.Drawing.Color.White;
            series1.ChartArea = "isoCurve-Distances";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.DodgerBlue;
            series1.CustomProperties = "PointWidth=0.3";
            series1.EmptyPointStyle.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series1.EmptyPointStyle.Color = System.Drawing.Color.White;
            series1.IsVisibleInLegend = false;
            series1.LabelBackColor = System.Drawing.Color.White;
            series1.LabelBorderColor = System.Drawing.Color.White;
            series1.LabelForeColor = System.Drawing.Color.Gainsboro;
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.White;
            series1.MarkerColor = System.Drawing.Color.White;
            series1.MarkerSize = 3;
            series1.Name = "IsoCurve";
            series1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chartIsoCurve.Series.Add(series1);
            this.chartIsoCurve.Size = new System.Drawing.Size(370, 230);
            this.chartIsoCurve.TabIndex = 15;
            this.chartIsoCurve.Text = "chart1";
            this.chartIsoCurve.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 70);
            // 
            // toolBar
            // 
            this.toolBar.AllowItemReorder = true;
            this.toolBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.toolBar.Font = new System.Drawing.Font("Consolas", 9F);
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbarProjectionMode,
            this.toolStripSeparator1,
            this.toolbarShaded,
            this.toolbarWireframe,
            this.toolbarPoint,
            this.toolStripSeparator4,
            this.toolStripButton2,
            this.toolbarIsBlinn,
            this.toolStripButton7});
            this.toolBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.MinimumSize = new System.Drawing.Size(1231, 70);
            this.toolBar.Name = "toolBar";
            this.toolBar.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.toolBar.Size = new System.Drawing.Size(1298, 70);
            this.toolBar.TabIndex = 17;
            this.toolBar.Text = "toolStrip1";
            // 
            // toolbarProjectionMode
            // 
            this.toolbarProjectionMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarProjectionMode.Image = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.toolbarProjectionMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarProjectionMode.Name = "toolbarProjectionMode";
            this.toolbarProjectionMode.Size = new System.Drawing.Size(52, 67);
            this.toolbarProjectionMode.Text = "Projection Mode";
            // 
            // toolbarShaded
            // 
            this.toolbarShaded.CheckOnClick = true;
            this.toolbarShaded.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarShaded.Image = global::VertexBenderCS.Resources.Shaded;
            this.toolbarShaded.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarShaded.Name = "toolbarShaded";
            this.toolbarShaded.Size = new System.Drawing.Size(52, 67);
            this.toolbarShaded.Text = "Shaded";
            // 
            // toolbarWireframe
            // 
            this.toolbarWireframe.CheckOnClick = true;
            this.toolbarWireframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarWireframe.Image = global::VertexBenderCS.Resources.Wireframe;
            this.toolbarWireframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarWireframe.Name = "toolbarWireframe";
            this.toolbarWireframe.Size = new System.Drawing.Size(52, 67);
            this.toolbarWireframe.Text = "Wireframe";
            // 
            // toolbarPoint
            // 
            this.toolbarPoint.CheckOnClick = true;
            this.toolbarPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarPoint.Image = global::VertexBenderCS.Resources.showPointCloud;
            this.toolbarPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarPoint.Name = "toolbarPoint";
            this.toolbarPoint.Size = new System.Drawing.Size(52, 67);
            this.toolbarPoint.Text = "Point Cloud";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 70);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.CheckOnClick = true;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::VertexBenderCS.Resources.grid;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton2.Text = "Show Grid";
            // 
            // toolbarIsBlinn
            // 
            this.toolbarIsBlinn.CheckOnClick = true;
            this.toolbarIsBlinn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarIsBlinn.Image = global::VertexBenderCS.Resources.showLight;
            this.toolbarIsBlinn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarIsBlinn.Name = "toolbarIsBlinn";
            this.toolbarIsBlinn.Size = new System.Drawing.Size(52, 67);
            this.toolbarIsBlinn.Text = "Toggle Lights";
            this.toolbarIsBlinn.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.CheckOnClick = true;
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::VertexBenderCS.Resources.showGizmo;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton7.Text = "Toggle Indicators";
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(1055, 48);
            // 
            // mainMenu
            // 
            this.mainMenu.Font = new System.Drawing.Font("Ebrima", 8.25F);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.processMenu,
            this.addMenu,
            this.editMenu,
            this.aboutMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainMenu.Size = new System.Drawing.Size(1298, 24);
            this.mainMenu.TabIndex = 19;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuImport,
            this.menuExport,
            this.toolStripSeparator,
            this.menuSave,
            this.menuSaveAs,
            this.toolStripSeparator2,
            this.menuExit});
            this.fileMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.fileMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // menuImport
            // 
            this.menuImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuImportOff,
            this.menuImportObj,
            this.menuImportDae,
            this.menuImportVol,
            this.menuImportDcm});
            this.menuImport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImport.Image = ((System.Drawing.Image)(resources.GetObject("menuImport.Image")));
            this.menuImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuImport.Name = "menuImport";
            this.menuImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.menuImport.Size = new System.Drawing.Size(149, 22);
            this.menuImport.Text = "&Import";
            // 
            // menuImportOff
            // 
            this.menuImportOff.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportOff.Name = "menuImportOff";
            this.menuImportOff.Size = new System.Drawing.Size(199, 22);
            this.menuImportOff.Text = "Object File Format (.off)";
            // 
            // menuImportObj
            // 
            this.menuImportObj.Enabled = false;
            this.menuImportObj.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportObj.Name = "menuImportObj";
            this.menuImportObj.Size = new System.Drawing.Size(199, 22);
            this.menuImportObj.Text = "Wavefront (.obj)";
            // 
            // menuImportDae
            // 
            this.menuImportDae.Enabled = false;
            this.menuImportDae.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportDae.Name = "menuImportDae";
            this.menuImportDae.Size = new System.Drawing.Size(199, 22);
            this.menuImportDae.Text = "Collada (.dae)";
            // 
            // menuImportVol
            // 
            this.menuImportVol.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportVol.Name = "menuImportVol";
            this.menuImportVol.Size = new System.Drawing.Size(199, 22);
            this.menuImportVol.Text = "Volume (.vol)";
            // 
            // menuImportDcm
            // 
            this.menuImportDcm.Enabled = false;
            this.menuImportDcm.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportDcm.Name = "menuImportDcm";
            this.menuImportDcm.Size = new System.Drawing.Size(199, 22);
            this.menuImportDcm.Text = "Dicom (.dcm)";
            // 
            // menuExport
            // 
            this.menuExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuIsoCurveExport,
            this.menuOffExport});
            this.menuExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuExport.Image = ((System.Drawing.Image)(resources.GetObject("menuExport.Image")));
            this.menuExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuExport.Name = "menuExport";
            this.menuExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuExport.Size = new System.Drawing.Size(149, 22);
            this.menuExport.Text = "&Export";
            // 
            // menuIsoCurveExport
            // 
            this.menuIsoCurveExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuIsoCurveExport.Name = "menuIsoCurveExport";
            this.menuIsoCurveExport.Size = new System.Drawing.Size(165, 22);
            this.menuIsoCurveExport.Text = "Iso-Curve Output";
            // 
            // menuOffExport
            // 
            this.menuOffExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuOffExport.Name = "menuOffExport";
            this.menuOffExport.Size = new System.Drawing.Size(165, 22);
            this.menuOffExport.Text = "Off file";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.toolStripSeparator.ForeColor = System.Drawing.SystemColors.Desktop;
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(146, 6);
            // 
            // menuSave
            // 
            this.menuSave.Enabled = false;
            this.menuSave.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuSave.Image = ((System.Drawing.Image)(resources.GetObject("menuSave.Image")));
            this.menuSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(149, 22);
            this.menuSave.Text = "&Save";
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Enabled = false;
            this.menuSaveAs.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(149, 22);
            this.menuSaveAs.Text = "Save &As";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.toolStripSeparator2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // menuExit
            // 
            this.menuExit.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(149, 22);
            this.menuExit.Text = "E&xit";
            // 
            // processMenu
            // 
            this.processMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProcessSP,
            this.menuProcessGC,
            this.menuProcessDescriptor,
            this.menuProcessParametrization});
            this.processMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.processMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.processMenu.Name = "processMenu";
            this.processMenu.Size = new System.Drawing.Size(59, 20);
            this.processMenu.Text = "&Process";
            // 
            // menuProcessSP
            // 
            this.menuProcessSP.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessSP.Name = "menuProcessSP";
            this.menuProcessSP.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.menuProcessSP.Size = new System.Drawing.Size(244, 22);
            this.menuProcessSP.Text = "&Shortest Path";
            // 
            // menuProcessGC
            // 
            this.menuProcessGC.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessGC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuProcessGC.Name = "menuProcessGC";
            this.menuProcessGC.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.menuProcessGC.Size = new System.Drawing.Size(244, 22);
            this.menuProcessGC.Text = "&Gaussian Curvature";
            // 
            // menuProcessDescriptor
            // 
            this.menuProcessDescriptor.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessDescriptor.Name = "menuProcessDescriptor";
            this.menuProcessDescriptor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.menuProcessDescriptor.Size = new System.Drawing.Size(244, 22);
            this.menuProcessDescriptor.Text = "Sampling and &Descriptors";
            // 
            // menuProcessParametrization
            // 
            this.menuProcessParametrization.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessParametrization.Name = "menuProcessParametrization";
            this.menuProcessParametrization.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.menuProcessParametrization.Size = new System.Drawing.Size(244, 22);
            this.menuProcessParametrization.Text = "&Parametrization";
            // 
            // addMenu
            // 
            this.addMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddCube,
            this.menuAddTetrahedron,
            this.menuAddPyramid,
            this.menuAddBottomlessPyramid,
            this.menuAddSphereTetra,
            this.menuAddSphereCube,
            this.menuAddSphereIcosahedron});
            this.addMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.addMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.addMenu.Name = "addMenu";
            this.addMenu.Size = new System.Drawing.Size(41, 20);
            this.addMenu.Text = "&Add";
            // 
            // menuAddCube
            // 
            this.menuAddCube.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddCube.Name = "menuAddCube";
            this.menuAddCube.Size = new System.Drawing.Size(183, 22);
            this.menuAddCube.Text = "Cube";
            // 
            // menuAddTetrahedron
            // 
            this.menuAddTetrahedron.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddTetrahedron.Name = "menuAddTetrahedron";
            this.menuAddTetrahedron.Size = new System.Drawing.Size(183, 22);
            this.menuAddTetrahedron.Text = "Tetrahedron";
            // 
            // menuAddPyramid
            // 
            this.menuAddPyramid.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddPyramid.Name = "menuAddPyramid";
            this.menuAddPyramid.Size = new System.Drawing.Size(183, 22);
            this.menuAddPyramid.Text = "Pyramid";
            // 
            // menuAddBottomlessPyramid
            // 
            this.menuAddBottomlessPyramid.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddBottomlessPyramid.Name = "menuAddBottomlessPyramid";
            this.menuAddBottomlessPyramid.Size = new System.Drawing.Size(183, 22);
            this.menuAddBottomlessPyramid.Text = "Bottomless Pyramid";
            // 
            // menuAddSphereTetra
            // 
            this.menuAddSphereTetra.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddSphereTetra.Name = "menuAddSphereTetra";
            this.menuAddSphereTetra.Size = new System.Drawing.Size(183, 22);
            this.menuAddSphereTetra.Text = "Sphere (tetrahedron)";
            // 
            // menuAddSphereCube
            // 
            this.menuAddSphereCube.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddSphereCube.Name = "menuAddSphereCube";
            this.menuAddSphereCube.Size = new System.Drawing.Size(183, 22);
            this.menuAddSphereCube.Text = "Sphere (cube)";
            // 
            // menuAddSphereIcosahedron
            // 
            this.menuAddSphereIcosahedron.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuAddSphereIcosahedron.Name = "menuAddSphereIcosahedron";
            this.menuAddSphereIcosahedron.Size = new System.Drawing.Size(183, 22);
            this.menuAddSphereIcosahedron.Text = "Sphere icosahedron)";
            // 
            // editMenu
            // 
            this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripSeparator3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripSeparator6,
            this.toolStripMenuItem7});
            this.editMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.editMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(39, 20);
            this.editMenu.Text = "&Edit";
            this.editMenu.Visible = false;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItem2.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem2.Text = "&Undo";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItem3.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem3.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(140, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.toolStripMenuItem4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem4.Image")));
            this.toolStripMenuItem4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItem4.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem4.Text = "Cu&t";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItem5.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem5.Text = "&Copy";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.toolStripMenuItem6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem6.Image")));
            this.toolStripMenuItem6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItem6.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem6.Text = "&Paste";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(140, 6);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem7.Text = "Select &All";
            // 
            // aboutMenu
            // 
            this.aboutMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.aboutMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.aboutMenu.Name = "aboutMenu";
            this.aboutMenu.Size = new System.Drawing.Size(52, 20);
            this.aboutMenu.Text = "Abou&t";
            // 
            // statusBar
            // 
            this.statusBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusBar.Location = new System.Drawing.Point(0, 880);
            this.statusBar.MaximumSize = new System.Drawing.Size(0, 25);
            this.statusBar.MinimumSize = new System.Drawing.Size(0, 25);
            this.statusBar.Name = "statusBar";
            this.statusBar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusBar.Size = new System.Drawing.Size(1298, 25);
            this.statusBar.TabIndex = 20;
            this.statusBar.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // transformPanel
            // 
            this.transformPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.transformPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.transformPanel.Controls.Add(this.numericScaleZ);
            this.transformPanel.Controls.Add(this.numericScaleY);
            this.transformPanel.Controls.Add(this.numericScaleX);
            this.transformPanel.Controls.Add(this.numericRotZ);
            this.transformPanel.Controls.Add(this.numericRotY);
            this.transformPanel.Controls.Add(this.numericRotX);
            this.transformPanel.Controls.Add(this.numericPosZ);
            this.transformPanel.Controls.Add(this.numericPosY);
            this.transformPanel.Controls.Add(this.labelZPos);
            this.transformPanel.Controls.Add(this.labelYPos);
            this.transformPanel.Controls.Add(this.labelXpos);
            this.transformPanel.Controls.Add(this.labelScale);
            this.transformPanel.Controls.Add(this.labelRotation);
            this.transformPanel.Controls.Add(this.labelPosition);
            this.transformPanel.Controls.Add(this.numericPosX);
            this.transformPanel.Controls.Add(this.labelTransform);
            this.transformPanel.Location = new System.Drawing.Point(15, 444);
            this.transformPanel.Name = "transformPanel";
            this.transformPanel.Size = new System.Drawing.Size(226, 155);
            this.transformPanel.TabIndex = 21;
            // 
            // numericScaleZ
            // 
            this.numericScaleZ.DecimalPlaces = 3;
            this.numericScaleZ.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericScaleZ.Location = new System.Drawing.Point(170, 105);
            this.numericScaleZ.Margin = new System.Windows.Forms.Padding(0);
            this.numericScaleZ.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericScaleZ.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericScaleZ.Name = "numericScaleZ";
            this.numericScaleZ.Size = new System.Drawing.Size(50, 22);
            this.numericScaleZ.TabIndex = 15;
            this.numericScaleZ.Tag = "scaleZ";
            // 
            // numericScaleY
            // 
            this.numericScaleY.DecimalPlaces = 3;
            this.numericScaleY.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericScaleY.Location = new System.Drawing.Point(115, 105);
            this.numericScaleY.Margin = new System.Windows.Forms.Padding(0);
            this.numericScaleY.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericScaleY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericScaleY.Name = "numericScaleY";
            this.numericScaleY.Size = new System.Drawing.Size(50, 22);
            this.numericScaleY.TabIndex = 14;
            this.numericScaleY.Tag = "scaleY";
            // 
            // numericScaleX
            // 
            this.numericScaleX.DecimalPlaces = 3;
            this.numericScaleX.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericScaleX.Location = new System.Drawing.Point(60, 105);
            this.numericScaleX.Margin = new System.Windows.Forms.Padding(0);
            this.numericScaleX.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericScaleX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericScaleX.Name = "numericScaleX";
            this.numericScaleX.Size = new System.Drawing.Size(50, 22);
            this.numericScaleX.TabIndex = 13;
            this.numericScaleX.Tag = "scaleX";
            // 
            // numericRotZ
            // 
            this.numericRotZ.DecimalPlaces = 3;
            this.numericRotZ.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericRotZ.Location = new System.Drawing.Point(170, 75);
            this.numericRotZ.Margin = new System.Windows.Forms.Padding(0);
            this.numericRotZ.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericRotZ.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericRotZ.Name = "numericRotZ";
            this.numericRotZ.Size = new System.Drawing.Size(50, 22);
            this.numericRotZ.TabIndex = 12;
            this.numericRotZ.Tag = "rotZ";
            // 
            // numericRotY
            // 
            this.numericRotY.DecimalPlaces = 3;
            this.numericRotY.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericRotY.Location = new System.Drawing.Point(115, 75);
            this.numericRotY.Margin = new System.Windows.Forms.Padding(0);
            this.numericRotY.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericRotY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericRotY.Name = "numericRotY";
            this.numericRotY.Size = new System.Drawing.Size(50, 22);
            this.numericRotY.TabIndex = 11;
            this.numericRotY.Tag = "rotY";
            // 
            // numericRotX
            // 
            this.numericRotX.DecimalPlaces = 3;
            this.numericRotX.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericRotX.Location = new System.Drawing.Point(60, 75);
            this.numericRotX.Margin = new System.Windows.Forms.Padding(0);
            this.numericRotX.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericRotX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericRotX.Name = "numericRotX";
            this.numericRotX.Size = new System.Drawing.Size(50, 22);
            this.numericRotX.TabIndex = 10;
            this.numericRotX.Tag = "rotX";
            // 
            // numericPosZ
            // 
            this.numericPosZ.DecimalPlaces = 3;
            this.numericPosZ.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericPosZ.Location = new System.Drawing.Point(170, 45);
            this.numericPosZ.Margin = new System.Windows.Forms.Padding(0);
            this.numericPosZ.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericPosZ.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericPosZ.Name = "numericPosZ";
            this.numericPosZ.Size = new System.Drawing.Size(50, 22);
            this.numericPosZ.TabIndex = 9;
            this.numericPosZ.Tag = "posZ";
            // 
            // numericPosY
            // 
            this.numericPosY.DecimalPlaces = 3;
            this.numericPosY.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericPosY.Location = new System.Drawing.Point(115, 45);
            this.numericPosY.Margin = new System.Windows.Forms.Padding(0);
            this.numericPosY.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericPosY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericPosY.Name = "numericPosY";
            this.numericPosY.Size = new System.Drawing.Size(50, 22);
            this.numericPosY.TabIndex = 8;
            this.numericPosY.Tag = "posY";
            // 
            // labelZPos
            // 
            this.labelZPos.AutoSize = true;
            this.labelZPos.Location = new System.Drawing.Point(167, 25);
            this.labelZPos.Name = "labelZPos";
            this.labelZPos.Size = new System.Drawing.Size(17, 15);
            this.labelZPos.TabIndex = 7;
            this.labelZPos.Text = "Z:";
            // 
            // labelYPos
            // 
            this.labelYPos.AutoSize = true;
            this.labelYPos.Location = new System.Drawing.Point(112, 25);
            this.labelYPos.Name = "labelYPos";
            this.labelYPos.Size = new System.Drawing.Size(17, 15);
            this.labelYPos.TabIndex = 6;
            this.labelYPos.Text = "Y:";
            // 
            // labelXpos
            // 
            this.labelXpos.AutoSize = true;
            this.labelXpos.Location = new System.Drawing.Point(57, 25);
            this.labelXpos.Name = "labelXpos";
            this.labelXpos.Size = new System.Drawing.Size(17, 15);
            this.labelXpos.TabIndex = 5;
            this.labelXpos.Text = "X:";
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(20, 106);
            this.labelScale.Name = "labelScale";
            this.labelScale.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelScale.Size = new System.Drawing.Size(37, 15);
            this.labelScale.TabIndex = 4;
            this.labelScale.Text = "Scale:";
            // 
            // labelRotation
            // 
            this.labelRotation.AutoSize = true;
            this.labelRotation.Location = new System.Drawing.Point(2, 75);
            this.labelRotation.Name = "labelRotation";
            this.labelRotation.Size = new System.Drawing.Size(55, 15);
            this.labelRotation.TabIndex = 3;
            this.labelRotation.Text = "Rotation:";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(4, 46);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(53, 15);
            this.labelPosition.TabIndex = 2;
            this.labelPosition.Text = "Position:";
            // 
            // numericPosX
            // 
            this.numericPosX.DecimalPlaces = 3;
            this.numericPosX.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericPosX.Location = new System.Drawing.Point(60, 45);
            this.numericPosX.Margin = new System.Windows.Forms.Padding(0);
            this.numericPosX.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericPosX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.numericPosX.Name = "numericPosX";
            this.numericPosX.Size = new System.Drawing.Size(50, 22);
            this.numericPosX.TabIndex = 1;
            this.numericPosX.Tag = "posX";
            // 
            // labelTransform
            // 
            this.labelTransform.AutoSize = true;
            this.labelTransform.Location = new System.Drawing.Point(3, 3);
            this.labelTransform.Name = "labelTransform";
            this.labelTransform.Size = new System.Drawing.Size(38, 15);
            this.labelTransform.TabIndex = 0;
            this.labelTransform.Text = "Name";
            // 
            // spherePanel
            // 
            this.spherePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.spherePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spherePanel.Controls.Add(this.numericSubdivision);
            this.spherePanel.Controls.Add(this.label5);
            this.spherePanel.Controls.Add(this.label6);
            this.spherePanel.Controls.Add(this.numericSize);
            this.spherePanel.Controls.Add(this.label7);
            this.spherePanel.Location = new System.Drawing.Point(15, 605);
            this.spherePanel.Name = "spherePanel";
            this.spherePanel.Size = new System.Drawing.Size(226, 93);
            this.spherePanel.TabIndex = 22;
            this.spherePanel.Visible = false;
            // 
            // numericSubdivision
            // 
            this.numericSubdivision.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericSubdivision.Location = new System.Drawing.Point(90, 55);
            this.numericSubdivision.Margin = new System.Windows.Forms.Padding(0);
            this.numericSubdivision.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericSubdivision.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSubdivision.Name = "numericSubdivision";
            this.numericSubdivision.Size = new System.Drawing.Size(50, 22);
            this.numericSubdivision.TabIndex = 10;
            this.numericSubdivision.Tag = "rotX";
            this.numericSubdivision.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Subdivision:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Size:";
            // 
            // numericSize
            // 
            this.numericSize.DecimalPlaces = 3;
            this.numericSize.Font = new System.Drawing.Font("Ebrima", 8F);
            this.numericSize.Location = new System.Drawing.Point(90, 24);
            this.numericSize.Margin = new System.Windows.Forms.Padding(0);
            this.numericSize.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericSize.Name = "numericSize";
            this.numericSize.Size = new System.Drawing.Size(50, 22);
            this.numericSize.TabIndex = 1;
            this.numericSize.Tag = "posX";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Sphere";
            // 
            // sliderMin
            // 
            this.sliderMin.Location = new System.Drawing.Point(59, 21);
            this.sliderMin.Maximum = 255;
            this.sliderMin.Name = "sliderMin";
            this.sliderMin.Size = new System.Drawing.Size(167, 45);
            this.sliderMin.TabIndex = 23;
            // 
            // pointCloudPanel
            // 
            this.pointCloudPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.pointCloudPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pointCloudPanel.Controls.Add(this.sliderMaxText);
            this.pointCloudPanel.Controls.Add(this.sliderMinText);
            this.pointCloudPanel.Controls.Add(this.sliderMax);
            this.pointCloudPanel.Controls.Add(this.label1);
            this.pointCloudPanel.Controls.Add(this.sliderMin);
            this.pointCloudPanel.Controls.Add(this.label2);
            this.pointCloudPanel.Controls.Add(this.label3);
            this.pointCloudPanel.Location = new System.Drawing.Point(15, 605);
            this.pointCloudPanel.Name = "pointCloudPanel";
            this.pointCloudPanel.Size = new System.Drawing.Size(226, 93);
            this.pointCloudPanel.TabIndex = 24;
            this.pointCloudPanel.Visible = false;
            // 
            // sliderMaxText
            // 
            this.sliderMaxText.AutoSize = true;
            this.sliderMaxText.Location = new System.Drawing.Point(36, 56);
            this.sliderMaxText.Name = "sliderMaxText";
            this.sliderMaxText.Size = new System.Drawing.Size(25, 15);
            this.sliderMaxText.TabIndex = 26;
            this.sliderMaxText.Text = "255";
            // 
            // sliderMinText
            // 
            this.sliderMinText.AutoSize = true;
            this.sliderMinText.Location = new System.Drawing.Point(38, 23);
            this.sliderMinText.Name = "sliderMinText";
            this.sliderMinText.Size = new System.Drawing.Size(13, 15);
            this.sliderMinText.TabIndex = 25;
            this.sliderMinText.Text = "0";
            // 
            // sliderMax
            // 
            this.sliderMax.Location = new System.Drawing.Point(59, 54);
            this.sliderMax.Maximum = 255;
            this.sliderMax.Name = "sliderMax";
            this.sliderMax.Size = new System.Drawing.Size(167, 45);
            this.sliderMax.TabIndex = 24;
            this.sliderMax.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Max:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Min:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Point Cloud";
            // 
            // IntensityMarch
            // 
            this.IntensityMarch.Location = new System.Drawing.Point(15, 696);
            this.IntensityMarch.Maximum = 255;
            this.IntensityMarch.Name = "IntensityMarch";
            this.IntensityMarch.Size = new System.Drawing.Size(167, 45);
            this.IntensityMarch.TabIndex = 25;
            this.IntensityMarch.TabStop = false;
            this.IntensityMarch.Value = 60;
            // 
            // numericMarch
            // 
            this.numericMarch.Location = new System.Drawing.Point(25, 730);
            this.numericMarch.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericMarch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericMarch.Name = "numericMarch";
            this.numericMarch.Size = new System.Drawing.Size(120, 24);
            this.numericMarch.TabIndex = 26;
            this.numericMarch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1298, 905);
            this.Controls.Add(this.numericMarch);
            this.Controls.Add(this.IntensityMarch);
            this.Controls.Add(this.pointCloudPanel);
            this.Controls.Add(this.spherePanel);
            this.Controls.Add(this.transformPanel);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.chartIsoCurve);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.sceneGraphTree);
            this.Controls.Add(this.GLControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Ebrima", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(931, 686);
            this.Name = "MainWin";
            this.Text = "Vertex Bender Framework";
            ((System.ComponentModel.ISupportInitialize)(this.chartIsoCurve)).EndInit();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.transformPanel.ResumeLayout(false);
            this.transformPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRotZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPosX)).EndInit();
            this.spherePanel.ResumeLayout(false);
            this.spherePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSubdivision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderMin)).EndInit();
            this.pointCloudPanel.ResumeLayout(false);
            this.pointCloudPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntensityMarch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMarch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl GLControl;
        private System.Windows.Forms.TreeView sceneGraphTree;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartIsoCurve;
        private System.ComponentModel.BackgroundWorker processWorker;
        private System.Windows.Forms.ToolStripButton toolbarProjectionMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripButton toolbarShaded;
        private System.Windows.Forms.ToolStripButton toolbarWireframe;
        private System.Windows.Forms.ToolStripButton toolbarPoint;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem menuImport;
        private System.Windows.Forms.ToolStripMenuItem menuExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem processMenu;
        private System.Windows.Forms.ToolStripMenuItem menuProcessSP;
        private System.Windows.Forms.ToolStripMenuItem menuProcessGC;
        private System.Windows.Forms.ToolStripMenuItem menuProcessParametrization;
        private System.Windows.Forms.ToolStripMenuItem aboutMenu;
        private System.Windows.Forms.ToolStripMenuItem editMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem menuImportOff;
        private System.Windows.Forms.ToolStripMenuItem menuImportObj;
        private System.Windows.Forms.ToolStripMenuItem menuImportDae;
        private System.Windows.Forms.ToolStripMenuItem menuImportVol;
        private System.Windows.Forms.ToolStripMenuItem menuImportDcm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolbarIsBlinn;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.Panel transformPanel;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Label labelRotation;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.NumericUpDown numericPosX;
        private System.Windows.Forms.Label labelTransform;
        private System.Windows.Forms.NumericUpDown numericScaleZ;
        private System.Windows.Forms.NumericUpDown numericScaleY;
        private System.Windows.Forms.NumericUpDown numericScaleX;
        private System.Windows.Forms.NumericUpDown numericRotZ;
        private System.Windows.Forms.NumericUpDown numericRotY;
        private System.Windows.Forms.NumericUpDown numericRotX;
        private System.Windows.Forms.NumericUpDown numericPosZ;
        private System.Windows.Forms.NumericUpDown numericPosY;
        private System.Windows.Forms.Label labelZPos;
        private System.Windows.Forms.Label labelYPos;
        private System.Windows.Forms.Label labelXpos;
        private System.Windows.Forms.ToolStripMenuItem menuIsoCurveExport;
        private System.Windows.Forms.ToolStripMenuItem menuProcessDescriptor;
        private System.Windows.Forms.ToolStripMenuItem addMenu;
        private System.Windows.Forms.ToolStripMenuItem menuAddCube;
        private System.Windows.Forms.ToolStripMenuItem menuAddTetrahedron;
        private System.Windows.Forms.ToolStripMenuItem menuAddPyramid;
        private System.Windows.Forms.ToolStripMenuItem menuAddBottomlessPyramid;
        private System.Windows.Forms.ToolStripMenuItem menuAddSphereTetra;
        private System.Windows.Forms.ToolStripMenuItem menuAddSphereIcosahedron;
        private System.Windows.Forms.Panel spherePanel;
        private System.Windows.Forms.NumericUpDown numericSubdivision;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem menuAddSphereCube;
        private System.Windows.Forms.TrackBar sliderMin;
        private System.Windows.Forms.Panel pointCloudPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar sliderMax;
        private System.Windows.Forms.Label sliderMaxText;
        private System.Windows.Forms.Label sliderMinText;
        private System.Windows.Forms.ToolStripMenuItem menuOffExport;
        private System.Windows.Forms.TrackBar IntensityMarch;
        private System.Windows.Forms.NumericUpDown numericMarch;
    }
}

