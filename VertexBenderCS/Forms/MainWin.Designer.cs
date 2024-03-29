﻿namespace VertexBenderCS.Forms
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
            this.GLControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 24, 8, 8));
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
            this.toolbarShowNormals = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnShowGrid = new System.Windows.Forms.ToolStripButton();
            this.btnShowHeadLight = new System.Windows.Forms.ToolStripButton();
            this.btnShowDirectLight = new System.Windows.Forms.ToolStripButton();
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
            this.menuImportNifti = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportVolAsPC = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportPTS = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIsoCurveExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOffExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStlExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSTlBinExport = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuProcessPostProcessing = new System.Windows.Forms.ToolStripMenuItem();
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
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.sliderPointCloud = new System.Windows.Forms.TrackBar();
            this.pointCloudPanel = new System.Windows.Forms.Panel();
            this.chkIsCuberille = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericPCDownSample = new System.Windows.Forms.NumericUpDown();
            this.sliderMinText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericMarch = new System.Windows.Forms.NumericUpDown();
            this.volumeRendererPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.numericPressure = new System.Windows.Forms.NumericUpDown();
            this.btnFinalize = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cmbVolMethod = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelIntensityMarch = new System.Windows.Forms.Label();
            this.intensityMarch = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.renderablePanel = new System.Windows.Forms.Panel();
            this.btnColor = new System.Windows.Forms.Button();
            this.chkCull = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.directLightX = new System.Windows.Forms.TrackBar();
            this.directLightY = new System.Windows.Forms.TrackBar();
            this.directLightZ = new System.Windows.Forms.TrackBar();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelDirLightY = new System.Windows.Forms.Label();
            this.labelDirLightZ = new System.Windows.Forms.Label();
            this.labelDirLightX = new System.Windows.Forms.Label();
            this.menuImportSamsung = new System.Windows.Forms.ToolStripMenuItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.sliderPointCloud)).BeginInit();
            this.pointCloudPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPCDownSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMarch)).BeginInit();
            this.volumeRendererPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intensityMarch)).BeginInit();
            this.panel1.SuspendLayout();
            this.renderablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directLightX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directLightY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directLightZ)).BeginInit();
            this.SuspendLayout();
            // 
            // GLControl
            // 
            this.GLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLControl.BackColor = System.Drawing.Color.Black;
            this.GLControl.Location = new System.Drawing.Point(259, 97);
            this.GLControl.Name = "GLControl";
            this.GLControl.Size = new System.Drawing.Size(1035, 641);
            this.GLControl.TabIndex = 29;
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
            this.toolbarShowNormals,
            this.toolStripSeparator4,
            this.btnShowGrid,
            this.btnShowHeadLight,
            this.btnShowDirectLight});
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
            this.toolbarShaded.Checked = true;
            this.toolbarShaded.CheckOnClick = true;
            this.toolbarShaded.CheckState = System.Windows.Forms.CheckState.Checked;
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
            this.toolbarPoint.ToolTipText = "Points";
            // 
            // toolbarShowNormals
            // 
            this.toolbarShowNormals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarShowNormals.Image = global::VertexBenderCS.Resources.normal;
            this.toolbarShowNormals.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarShowNormals.Name = "toolbarShowNormals";
            this.toolbarShowNormals.Size = new System.Drawing.Size(52, 67);
            this.toolbarShowNormals.Text = "btnShowNormals";
            this.toolbarShowNormals.ToolTipText = "Normals";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 70);
            // 
            // btnShowGrid
            // 
            this.btnShowGrid.Checked = true;
            this.btnShowGrid.CheckOnClick = true;
            this.btnShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnShowGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowGrid.Image = global::VertexBenderCS.Resources.grid;
            this.btnShowGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowGrid.Name = "btnShowGrid";
            this.btnShowGrid.Size = new System.Drawing.Size(52, 67);
            this.btnShowGrid.Text = "Show Grid";
            this.btnShowGrid.ToolTipText = "Grid";
            // 
            // btnShowHeadLight
            // 
            this.btnShowHeadLight.CheckOnClick = true;
            this.btnShowHeadLight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowHeadLight.Image = global::VertexBenderCS.Resources.showLight;
            this.btnShowHeadLight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowHeadLight.Name = "btnShowHeadLight";
            this.btnShowHeadLight.Size = new System.Drawing.Size(52, 67);
            this.btnShowHeadLight.Text = "Toggle Lights";
            this.btnShowHeadLight.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnShowHeadLight.ToolTipText = "Head Light";
            // 
            // btnShowDirectLight
            // 
            this.btnShowDirectLight.Checked = true;
            this.btnShowDirectLight.CheckOnClick = true;
            this.btnShowDirectLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnShowDirectLight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowDirectLight.Image = global::VertexBenderCS.Resources.showGizmo;
            this.btnShowDirectLight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowDirectLight.Name = "btnShowDirectLight";
            this.btnShowDirectLight.Size = new System.Drawing.Size(52, 67);
            this.btnShowDirectLight.Text = "Toggle Indicators";
            this.btnShowDirectLight.ToolTipText = "Directional Light";
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
            this.menuImportNifti,
            this.menuImportVolAsPC,
            this.menuImportSamsung,
            this.menuImportPTS});
            this.menuImport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImport.Image = ((System.Drawing.Image)(resources.GetObject("menuImport.Image")));
            this.menuImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuImport.Name = "menuImport";
            this.menuImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.menuImport.Size = new System.Drawing.Size(180, 22);
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
            // menuImportNifti
            // 
            this.menuImportNifti.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportNifti.Name = "menuImportNifti";
            this.menuImportNifti.Size = new System.Drawing.Size(199, 22);
            this.menuImportNifti.Text = "Nifti (.nii)";
            // 
            // menuImportVolAsPC
            // 
            this.menuImportVolAsPC.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportVolAsPC.Name = "menuImportVolAsPC";
            this.menuImportVolAsPC.Size = new System.Drawing.Size(199, 22);
            this.menuImportVolAsPC.Text = "Point Cloud(.vol, .nii)";
            // 
            // menuImportPTS
            // 
            this.menuImportPTS.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportPTS.Name = "menuImportPTS";
            this.menuImportPTS.Size = new System.Drawing.Size(199, 22);
            this.menuImportPTS.Text = "Laser Scan(.pts)";
            // 
            // menuExport
            // 
            this.menuExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuIsoCurveExport,
            this.menuOffExport,
            this.menuStlExport,
            this.menuSTlBinExport});
            this.menuExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuExport.Image = ((System.Drawing.Image)(resources.GetObject("menuExport.Image")));
            this.menuExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuExport.Name = "menuExport";
            this.menuExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuExport.Size = new System.Drawing.Size(180, 22);
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
            // menuStlExport
            // 
            this.menuStlExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuStlExport.Name = "menuStlExport";
            this.menuStlExport.Size = new System.Drawing.Size(165, 22);
            this.menuStlExport.Text = "STL";
            // 
            // menuSTlBinExport
            // 
            this.menuSTlBinExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuSTlBinExport.Name = "menuSTlBinExport";
            this.menuSTlBinExport.Size = new System.Drawing.Size(165, 22);
            this.menuSTlBinExport.Text = "STL(Binary)";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.toolStripSeparator.ForeColor = System.Drawing.SystemColors.Desktop;
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // menuSave
            // 
            this.menuSave.Enabled = false;
            this.menuSave.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuSave.Image = ((System.Drawing.Image)(resources.GetObject("menuSave.Image")));
            this.menuSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(180, 22);
            this.menuSave.Text = "&Save";
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Enabled = false;
            this.menuSaveAs.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAs.Text = "Save &As";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.toolStripSeparator2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // menuExit
            // 
            this.menuExit.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(180, 22);
            this.menuExit.Text = "E&xit";
            // 
            // processMenu
            // 
            this.processMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProcessSP,
            this.menuProcessGC,
            this.menuProcessDescriptor,
            this.menuProcessParametrization,
            this.menuProcessPostProcessing});
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
            // menuProcessPostProcessing
            // 
            this.menuProcessPostProcessing.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessPostProcessing.Name = "menuProcessPostProcessing";
            this.menuProcessPostProcessing.Size = new System.Drawing.Size(244, 22);
            this.menuProcessPostProcessing.Text = "Post-Processing";
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
            this.Status});
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
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(118, 20);
            this.Status.Text = "toolStripStatusLabel1";
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
            this.transformPanel.Location = new System.Drawing.Point(0, 0);
            this.transformPanel.Name = "transformPanel";
            this.transformPanel.Size = new System.Drawing.Size(226, 142);
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
            this.spherePanel.Location = new System.Drawing.Point(0, 238);
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
            // sliderPointCloud
            // 
            this.sliderPointCloud.Location = new System.Drawing.Point(59, 21);
            this.sliderPointCloud.Maximum = 255;
            this.sliderPointCloud.Name = "sliderPointCloud";
            this.sliderPointCloud.Size = new System.Drawing.Size(167, 45);
            this.sliderPointCloud.TabIndex = 23;
            // 
            // pointCloudPanel
            // 
            this.pointCloudPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.pointCloudPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pointCloudPanel.Controls.Add(this.chkIsCuberille);
            this.pointCloudPanel.Controls.Add(this.label1);
            this.pointCloudPanel.Controls.Add(this.numericPCDownSample);
            this.pointCloudPanel.Controls.Add(this.sliderMinText);
            this.pointCloudPanel.Controls.Add(this.sliderPointCloud);
            this.pointCloudPanel.Controls.Add(this.label2);
            this.pointCloudPanel.Controls.Add(this.label3);
            this.pointCloudPanel.Location = new System.Drawing.Point(3, 238);
            this.pointCloudPanel.Name = "pointCloudPanel";
            this.pointCloudPanel.Size = new System.Drawing.Size(223, 117);
            this.pointCloudPanel.TabIndex = 24;
            this.pointCloudPanel.Visible = false;
            // 
            // chkIsCuberille
            // 
            this.chkIsCuberille.AutoSize = true;
            this.chkIsCuberille.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsCuberille.Location = new System.Drawing.Point(18, 84);
            this.chkIsCuberille.Name = "chkIsCuberille";
            this.chkIsCuberille.Size = new System.Drawing.Size(83, 19);
            this.chkIsCuberille.TabIndex = 37;
            this.chkIsCuberille.Text = "Is Cuberille";
            this.chkIsCuberille.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 29;
            this.label1.Text = "down sample";
            // 
            // numericPCDownSample
            // 
            this.numericPCDownSample.Location = new System.Drawing.Point(89, 54);
            this.numericPCDownSample.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericPCDownSample.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPCDownSample.Name = "numericPCDownSample";
            this.numericPCDownSample.ReadOnly = true;
            this.numericPCDownSample.Size = new System.Drawing.Size(120, 24);
            this.numericPCDownSample.TabIndex = 28;
            this.numericPCDownSample.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
            // numericMarch
            // 
            this.numericMarch.Location = new System.Drawing.Point(85, 54);
            this.numericMarch.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericMarch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericMarch.Name = "numericMarch";
            this.numericMarch.ReadOnly = true;
            this.numericMarch.Size = new System.Drawing.Size(120, 24);
            this.numericMarch.TabIndex = 26;
            this.numericMarch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // volumeRendererPanel
            // 
            this.volumeRendererPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.volumeRendererPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.volumeRendererPanel.Controls.Add(this.label12);
            this.volumeRendererPanel.Controls.Add(this.numericPressure);
            this.volumeRendererPanel.Controls.Add(this.btnFinalize);
            this.volumeRendererPanel.Controls.Add(this.btnEdit);
            this.volumeRendererPanel.Controls.Add(this.cmbVolMethod);
            this.volumeRendererPanel.Controls.Add(this.label9);
            this.volumeRendererPanel.Controls.Add(this.label4);
            this.volumeRendererPanel.Controls.Add(this.labelIntensityMarch);
            this.volumeRendererPanel.Controls.Add(this.numericMarch);
            this.volumeRendererPanel.Controls.Add(this.intensityMarch);
            this.volumeRendererPanel.Controls.Add(this.label10);
            this.volumeRendererPanel.Controls.Add(this.label11);
            this.volumeRendererPanel.Location = new System.Drawing.Point(0, 238);
            this.volumeRendererPanel.Name = "volumeRendererPanel";
            this.volumeRendererPanel.Size = new System.Drawing.Size(226, 187);
            this.volumeRendererPanel.TabIndex = 27;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 152);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 15);
            this.label12.TabIndex = 35;
            this.label12.Text = "Pressure";
            // 
            // numericPressure
            // 
            this.numericPressure.Location = new System.Drawing.Point(71, 150);
            this.numericPressure.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericPressure.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPressure.Name = "numericPressure";
            this.numericPressure.ReadOnly = true;
            this.numericPressure.Size = new System.Drawing.Size(37, 24);
            this.numericPressure.TabIndex = 33;
            this.numericPressure.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnFinalize
            // 
            this.btnFinalize.ForeColor = System.Drawing.Color.Black;
            this.btnFinalize.Location = new System.Drawing.Point(115, 116);
            this.btnFinalize.Name = "btnFinalize";
            this.btnFinalize.Size = new System.Drawing.Size(102, 23);
            this.btnFinalize.TabIndex = 31;
            this.btnFinalize.Text = "Finalize";
            this.btnFinalize.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Location = new System.Drawing.Point(8, 116);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(102, 23);
            this.btnEdit.TabIndex = 30;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // cmbVolMethod
            // 
            this.cmbVolMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVolMethod.FormattingEnabled = true;
            this.cmbVolMethod.Items.AddRange(new object[] {
            "GPU Boost",
            "GPU",
            "CPU"});
            this.cmbVolMethod.Location = new System.Drawing.Point(84, 84);
            this.cmbVolMethod.Name = "cmbVolMethod";
            this.cmbVolMethod.Size = new System.Drawing.Size(121, 23);
            this.cmbVolMethod.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 15);
            this.label9.TabIndex = 28;
            this.label9.Text = "Method";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "down sample";
            // 
            // labelIntensityMarch
            // 
            this.labelIntensityMarch.AutoSize = true;
            this.labelIntensityMarch.Location = new System.Drawing.Point(38, 23);
            this.labelIntensityMarch.Name = "labelIntensityMarch";
            this.labelIntensityMarch.Size = new System.Drawing.Size(13, 15);
            this.labelIntensityMarch.TabIndex = 25;
            this.labelIntensityMarch.Text = "0";
            // 
            // intensityMarch
            // 
            this.intensityMarch.Location = new System.Drawing.Point(59, 21);
            this.intensityMarch.Maximum = 5000;
            this.intensityMarch.Name = "intensityMarch";
            this.intensityMarch.Size = new System.Drawing.Size(167, 45);
            this.intensityMarch.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 15);
            this.label10.TabIndex = 2;
            this.label10.Text = "Min:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Volume";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.renderablePanel);
            this.panel1.Controls.Add(this.volumeRendererPanel);
            this.panel1.Controls.Add(this.pointCloudPanel);
            this.panel1.Controls.Add(this.transformPanel);
            this.panel1.Controls.Add(this.spherePanel);
            this.panel1.Location = new System.Drawing.Point(3, 439);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 299);
            this.panel1.TabIndex = 28;
            // 
            // renderablePanel
            // 
            this.renderablePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.renderablePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.renderablePanel.Controls.Add(this.btnColor);
            this.renderablePanel.Controls.Add(this.chkCull);
            this.renderablePanel.Controls.Add(this.label8);
            this.renderablePanel.Controls.Add(this.numericUpDown1);
            this.renderablePanel.Controls.Add(this.label17);
            this.renderablePanel.Location = new System.Drawing.Point(0, 148);
            this.renderablePanel.Name = "renderablePanel";
            this.renderablePanel.Size = new System.Drawing.Size(226, 84);
            this.renderablePanel.TabIndex = 37;
            // 
            // btnColor
            // 
            this.btnColor.Location = new System.Drawing.Point(109, 27);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(75, 23);
            this.btnColor.TabIndex = 39;
            this.btnColor.Text = "Color";
            this.btnColor.UseVisualStyleBackColor = true;
            // 
            // chkCull
            // 
            this.chkCull.AutoSize = true;
            this.chkCull.Location = new System.Drawing.Point(8, 24);
            this.chkCull.Name = "chkCull";
            this.chkCull.Size = new System.Drawing.Size(46, 19);
            this.chkCull.TabIndex = 36;
            this.chkCull.Text = "Cull";
            this.chkCull.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 15);
            this.label8.TabIndex = 35;
            this.label8.Text = "Pressure";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(71, 150);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.ReadOnly = true;
            this.numericUpDown1.Size = new System.Drawing.Size(37, 24);
            this.numericUpDown1.TabIndex = 33;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(66, 15);
            this.label17.TabIndex = 0;
            this.label17.Text = "Renderable";
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // directLightX
            // 
            this.directLightX.AutoSize = false;
            this.directLightX.Location = new System.Drawing.Point(480, 27);
            this.directLightX.Maximum = 180;
            this.directLightX.Minimum = -179;
            this.directLightX.Name = "directLightX";
            this.directLightX.Size = new System.Drawing.Size(167, 35);
            this.directLightX.TabIndex = 30;
            this.directLightX.TickStyle = System.Windows.Forms.TickStyle.None;
            this.directLightX.Value = 180;
            // 
            // directLightY
            // 
            this.directLightY.AutoSize = false;
            this.directLightY.Location = new System.Drawing.Point(480, 47);
            this.directLightY.Maximum = 180;
            this.directLightY.Minimum = -179;
            this.directLightY.Name = "directLightY";
            this.directLightY.Size = new System.Drawing.Size(167, 24);
            this.directLightY.TabIndex = 31;
            this.directLightY.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // directLightZ
            // 
            this.directLightZ.AutoSize = false;
            this.directLightZ.Location = new System.Drawing.Point(480, 68);
            this.directLightZ.Maximum = 180;
            this.directLightZ.Minimum = -179;
            this.directLightZ.Name = "directLightZ";
            this.directLightZ.Size = new System.Drawing.Size(167, 23);
            this.directLightZ.TabIndex = 32;
            this.directLightZ.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(435, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 15);
            this.label13.TabIndex = 34;
            this.label13.Text = "X:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(435, 69);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 15);
            this.label14.TabIndex = 35;
            this.label14.Text = "Z:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(435, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 15);
            this.label15.TabIndex = 36;
            this.label15.Text = "Y:";
            // 
            // labelDirLightY
            // 
            this.labelDirLightY.AutoSize = true;
            this.labelDirLightY.Location = new System.Drawing.Point(453, 48);
            this.labelDirLightY.Name = "labelDirLightY";
            this.labelDirLightY.Size = new System.Drawing.Size(0, 15);
            this.labelDirLightY.TabIndex = 39;
            // 
            // labelDirLightZ
            // 
            this.labelDirLightZ.AutoSize = true;
            this.labelDirLightZ.Location = new System.Drawing.Point(453, 69);
            this.labelDirLightZ.Name = "labelDirLightZ";
            this.labelDirLightZ.Size = new System.Drawing.Size(0, 15);
            this.labelDirLightZ.TabIndex = 38;
            // 
            // labelDirLightX
            // 
            this.labelDirLightX.AutoSize = true;
            this.labelDirLightX.Location = new System.Drawing.Point(453, 29);
            this.labelDirLightX.Name = "labelDirLightX";
            this.labelDirLightX.Size = new System.Drawing.Size(0, 15);
            this.labelDirLightX.TabIndex = 37;
            // 
            // menuImportSamsung
            // 
            this.menuImportSamsung.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportSamsung.Name = "menuImportSamsung";
            this.menuImportSamsung.Size = new System.Drawing.Size(199, 22);
            this.menuImportSamsung.Text = "Samsung";
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1298, 905);
            this.Controls.Add(this.labelDirLightY);
            this.Controls.Add(this.labelDirLightZ);
            this.Controls.Add(this.labelDirLightX);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.directLightZ);
            this.Controls.Add(this.directLightY);
            this.Controls.Add(this.directLightX);
            this.Controls.Add(this.GLControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.chartIsoCurve);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.sceneGraphTree);
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
            ((System.ComponentModel.ISupportInitialize)(this.sliderPointCloud)).EndInit();
            this.pointCloudPanel.ResumeLayout(false);
            this.pointCloudPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPCDownSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMarch)).EndInit();
            this.volumeRendererPanel.ResumeLayout(false);
            this.volumeRendererPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intensityMarch)).EndInit();
            this.panel1.ResumeLayout(false);
            this.renderablePanel.ResumeLayout(false);
            this.renderablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directLightX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directLightY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directLightZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.ToolStripMenuItem menuImportNifti;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnShowGrid;
        private System.Windows.Forms.ToolStripButton btnShowHeadLight;
        private System.Windows.Forms.ToolStripButton btnShowDirectLight;
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
        private System.Windows.Forms.TrackBar sliderPointCloud;
        private System.Windows.Forms.Panel pointCloudPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label sliderMinText;
        private System.Windows.Forms.ToolStripMenuItem menuOffExport;
        private System.Windows.Forms.NumericUpDown numericMarch;
        private System.Windows.Forms.Panel volumeRendererPanel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelIntensityMarch;
        private System.Windows.Forms.TrackBar intensityMarch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbVolMethod;
        private System.Windows.Forms.Button btnFinalize;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numericPressure;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem menuStlExport;
        private System.Windows.Forms.ToolStripMenuItem menuSTlBinExport;
        private OpenTK.GLControl GLControl;
        private System.Windows.Forms.ToolStripButton toolbarShowNormals;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel renderablePanel;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.CheckBox chkCull;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TrackBar directLightX;
        private System.Windows.Forms.TrackBar directLightY;
        private System.Windows.Forms.TrackBar directLightZ;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelDirLightY;
        private System.Windows.Forms.Label labelDirLightZ;
        private System.Windows.Forms.Label labelDirLightX;
        private System.Windows.Forms.ToolStripMenuItem menuImportVolAsPC;
        private System.Windows.Forms.ToolStripMenuItem menuProcessPostProcessing;
        private System.Windows.Forms.CheckBox chkIsCuberille;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericPCDownSample;
        private System.Windows.Forms.ToolStripMenuItem menuImportPTS;
        private System.Windows.Forms.ToolStripMenuItem menuImportSamsung;
    }
}

