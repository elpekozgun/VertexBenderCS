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
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Scene");
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.GLControl = new OpenTK.GLControl();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Log = new System.Windows.Forms.TextBox();
            this.chartIsoCurve = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.processWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBar = new System.Windows.Forms.ToolStrip();
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
            this.menuProcessGM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessFPS = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessGC = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessAGD = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessIso = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
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
            this.GLControl.Location = new System.Drawing.Point(219, 66);
            this.GLControl.MinimumSize = new System.Drawing.Size(400, 400);
            this.GLControl.Name = "GLControl";
            this.GLControl.Size = new System.Drawing.Size(891, 577);
            this.GLControl.TabIndex = 0;
            this.GLControl.VSync = false;
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Font = new System.Drawing.Font("Ebrima", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.HotTracking = true;
            this.treeView1.LineColor = System.Drawing.Color.MediumPurple;
            this.treeView1.Location = new System.Drawing.Point(3, 66);
            this.treeView1.Name = "treeView1";
            treeNode4.Checked = true;
            treeNode4.Name = "Scene";
            treeNode4.NodeFont = new System.Drawing.Font("Ebrima", 8.25F);
            treeNode4.Text = "Scene";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.treeView1.Size = new System.Drawing.Size(215, 310);
            this.treeView1.TabIndex = 4;
            // 
            // Log
            // 
            this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Log.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Log.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Log.Location = new System.Drawing.Point(3, 645);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(1107, 117);
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
            chartArea4.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gainsboro;
            chartArea4.AxisX.LabelStyle.Interval = 0D;
            chartArea4.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea4.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea4.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Gainsboro;
            chartArea4.AxisY.LabelStyle.Interval = 0D;
            chartArea4.AxisY.LabelStyle.IntervalOffset = 0D;
            chartArea4.BackColor = System.Drawing.Color.White;
            chartArea4.Name = "isoCurve-Distances";
            this.chartIsoCurve.ChartAreas.Add(chartArea4);
            legend4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            legend4.ForeColor = System.Drawing.Color.Gainsboro;
            legend4.InterlacedRowsColor = System.Drawing.Color.White;
            legend4.Name = "Legend1";
            legend4.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chartIsoCurve.Legends.Add(legend4);
            this.chartIsoCurve.Location = new System.Drawing.Point(791, 68);
            this.chartIsoCurve.Margin = new System.Windows.Forms.Padding(0);
            this.chartIsoCurve.Name = "chartIsoCurve";
            this.chartIsoCurve.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            this.chartIsoCurve.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series4.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Center;
            series4.BackSecondaryColor = System.Drawing.Color.White;
            series4.ChartArea = "isoCurve-Distances";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series4.Color = System.Drawing.Color.DodgerBlue;
            series4.CustomProperties = "PointWidth=0.3";
            series4.EmptyPointStyle.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series4.EmptyPointStyle.Color = System.Drawing.Color.White;
            series4.IsVisibleInLegend = false;
            series4.LabelBackColor = System.Drawing.Color.White;
            series4.LabelBorderColor = System.Drawing.Color.White;
            series4.LabelForeColor = System.Drawing.Color.Gainsboro;
            series4.Legend = "Legend1";
            series4.MarkerBorderColor = System.Drawing.Color.White;
            series4.MarkerColor = System.Drawing.Color.White;
            series4.MarkerSize = 3;
            series4.Name = "IsoCurve";
            series4.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chartIsoCurve.Series.Add(series4);
            this.chartIsoCurve.Size = new System.Drawing.Size(317, 199);
            this.chartIsoCurve.TabIndex = 15;
            this.chartIsoCurve.Text = "chart1";
            this.chartIsoCurve.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 382);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(199, 17);
            this.progressBar.TabIndex = 16;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 37);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // toolBar
            // 
            this.toolBar.AllowItemReorder = true;
            this.toolBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.toolBar.Font = new System.Drawing.Font("Consolas", 9F);
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton6,
            this.toolStripButton5,
            this.toolStripButton4});
            this.toolBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.MinimumSize = new System.Drawing.Size(1055, 40);
            this.toolBar.Name = "toolBar";
            this.toolBar.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.toolBar.Size = new System.Drawing.Size(1113, 40);
            this.toolBar.TabIndex = 17;
            this.toolBar.Text = "toolStrip1";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(29, 37);
            this.toolStripButton6.Text = "toolStripButton2";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(29, 37);
            this.toolStripButton5.Text = "asd";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(29, 37);
            this.toolStripButton4.Text = "toolStripButton2";
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
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainMenu.Size = new System.Drawing.Size(1113, 24);
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
            this.menuProcessGM,
            this.menuProcessFPS,
            this.menuProcessGC,
            this.menuProcessAGD,
            this.menuProcessIso});
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
            this.menuProcessSP.Size = new System.Drawing.Size(256, 22);
            this.menuProcessSP.Text = "&Shortest Path";
            // 
            // menuProcessGM
            // 
            this.menuProcessGM.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessGM.Name = "menuProcessGM";
            this.menuProcessGM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.menuProcessGM.Size = new System.Drawing.Size(256, 22);
            this.menuProcessGM.Text = "&Geodesic Matrix";
            // 
            // menuProcessFPS
            // 
            this.menuProcessFPS.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessFPS.Image = ((System.Drawing.Image)(resources.GetObject("menuProcessFPS.Image")));
            this.menuProcessFPS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuProcessFPS.Name = "menuProcessFPS";
            this.menuProcessFPS.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menuProcessFPS.Size = new System.Drawing.Size(256, 22);
            this.menuProcessFPS.Text = "&Farthest Point Sampling";
            // 
            // menuProcessGC
            // 
            this.menuProcessGC.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessGC.Image = ((System.Drawing.Image)(resources.GetObject("menuProcessGC.Image")));
            this.menuProcessGC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuProcessGC.Name = "menuProcessGC";
            this.menuProcessGC.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuProcessGC.Size = new System.Drawing.Size(256, 22);
            this.menuProcessGC.Text = "&Gaussian Curvature";
            // 
            // menuProcessAGD
            // 
            this.menuProcessAGD.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessAGD.Image = ((System.Drawing.Image)(resources.GetObject("menuProcessAGD.Image")));
            this.menuProcessAGD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuProcessAGD.Name = "menuProcessAGD";
            this.menuProcessAGD.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.menuProcessAGD.Size = new System.Drawing.Size(256, 22);
            this.menuProcessAGD.Text = "&Average Geodesic Distance";
            // 
            // menuProcessIso
            // 
            this.menuProcessIso.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.menuProcessIso.Name = "menuProcessIso";
            this.menuProcessIso.Size = new System.Drawing.Size(256, 22);
            this.menuProcessIso.Text = "&Iso Curve Signatures";
            // 
            // aboutMenu
            // 
            this.aboutMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem});
            this.aboutMenu.Font = new System.Drawing.Font("Ebrima", 9F);
            this.aboutMenu.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.aboutMenu.Name = "aboutMenu";
            this.aboutMenu.Size = new System.Drawing.Size(52, 20);
            this.aboutMenu.Text = "&About";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
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
            this.statusProgress});
            this.statusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusBar.Location = new System.Drawing.Point(0, 762);
            this.statusBar.MaximumSize = new System.Drawing.Size(0, 22);
            this.statusBar.MinimumSize = new System.Drawing.Size(0, 22);
            this.statusBar.Name = "statusBar";
            this.statusBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusBar.Size = new System.Drawing.Size(1113, 22);
            this.statusBar.TabIndex = 20;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusProgress
            // 
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusProgress.Size = new System.Drawing.Size(200, 16);
            this.statusProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1113, 784);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.chartIsoCurve);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.GLControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Ebrima", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
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
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartIsoCurve;
        private System.Windows.Forms.ProgressBar progressBar;
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
        private System.Windows.Forms.ToolStripMenuItem menuProcessGM;
        private System.Windows.Forms.ToolStripMenuItem menuProcessFPS;
        private System.Windows.Forms.ToolStripMenuItem menuProcessGC;
        private System.Windows.Forms.ToolStripMenuItem menuProcessAGD;
        private System.Windows.Forms.ToolStripMenuItem menuProcessIso;
        private System.Windows.Forms.ToolStripMenuItem aboutMenu;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
    }
}

