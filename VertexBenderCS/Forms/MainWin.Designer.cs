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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Scene");
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.GLControl = new OpenTK.GLControl();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Log = new System.Windows.Forms.TextBox();
            this.chartIsoCurve = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.processWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
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
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.processMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessSP = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessGC = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessDescriptor = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.chartIsoCurve)).BeginInit();
            this.toolBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
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
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Font = new System.Drawing.Font("Ebrima", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.HotTracking = true;
            this.treeView1.LineColor = System.Drawing.Color.MediumPurple;
            this.treeView1.Location = new System.Drawing.Point(3, 97);
            this.treeView1.Name = "treeView1";
            treeNode1.Checked = true;
            treeNode1.Name = "Scene";
            treeNode1.NodeFont = new System.Drawing.Font("Ebrima", 8.25F);
            treeNode1.Text = "Scene";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.Size = new System.Drawing.Size(250, 336);
            this.treeView1.TabIndex = 4;
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
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
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
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton6,
            this.toolStripButton5,
            this.toolStripButton4,
            this.toolStripSeparator4,
            this.toolStripButton2,
            this.toolStripButton3,
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
            // toolStripButton1
            // 
            this.toolStripButton1.CheckOnClick = true;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton1.Text = "Projection Mode";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.CheckOnClick = true;
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::VertexBenderCS.Resources.Shaded;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton6.Text = "Shaded";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.CheckOnClick = true;
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::VertexBenderCS.Resources.Wireframe;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton5.Text = "Wireframe";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.CheckOnClick = true;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::VertexBenderCS.Resources.showPointCloud;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton4.Text = "Point Cloud";
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
            this.aboutMenu,
            this.editMenu});
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
            this.menuImportObj.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportObj.Name = "menuImportObj";
            this.menuImportObj.Size = new System.Drawing.Size(199, 22);
            this.menuImportObj.Text = "Wavefront (.obj)";
            // 
            // menuImportDae
            // 
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
            this.menuImportDcm.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuImportDcm.Name = "menuImportDcm";
            this.menuImportDcm.Size = new System.Drawing.Size(199, 22);
            this.menuImportDcm.Text = "Dicom (.dcm)";
            // 
            // menuExport
            // 
            this.menuExport.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuExport.Image = ((System.Drawing.Image)(resources.GetObject("menuExport.Image")));
            this.menuExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuExport.Name = "menuExport";
            this.menuExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuExport.Size = new System.Drawing.Size(149, 22);
            this.menuExport.Text = "&Export";
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
            this.menuProcessDescriptor});
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
            this.menuProcessSP.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.menuProcessSP.Size = new System.Drawing.Size(215, 22);
            this.menuProcessSP.Text = "&Shortest Path";
            // 
            // menuProcessGC
            // 
            this.menuProcessGC.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessGC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuProcessGC.Name = "menuProcessGC";
            this.menuProcessGC.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuProcessGC.Size = new System.Drawing.Size(215, 22);
            this.menuProcessGC.Text = "&Gaussian Curvature";
            // 
            // menuProcessDescriptor
            // 
            this.menuProcessDescriptor.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessDescriptor.Name = "menuProcessDescriptor";
            this.menuProcessDescriptor.Size = new System.Drawing.Size(215, 22);
            this.menuProcessDescriptor.Text = "Sampling and &Descriptors";
            // 
            // aboutMenu
            // 
            this.aboutMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.aboutMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.aboutMenu.Name = "aboutMenu";
            this.aboutMenu.Size = new System.Drawing.Size(52, 20);
            this.aboutMenu.Text = "&About";
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
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 70);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.CheckOnClick = true;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::VertexBenderCS.Resources.showLight;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(52, 67);
            this.toolStripButton3.Text = "Toggle Lights";
            this.toolStripButton3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1298, 905);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.chartIsoCurve);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.treeView1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl GLControl;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartIsoCurve;
        private System.ComponentModel.BackgroundWorker processWorker;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
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
        private System.Windows.Forms.ToolStripMenuItem menuProcessDescriptor;
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
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
    }
}

