namespace VertexBenderCS
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.GLControl = new OpenTK.GLControl();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenu = new System.Windows.Forms.MenuItem();
            this.menuImport = new System.Windows.Forms.MenuItem();
            this.menuExport = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.editMenu = new System.Windows.Forms.MenuItem();
            this.aboutMenu = new System.Windows.Forms.MenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Log = new System.Windows.Forms.TextBox();
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.MaskedTextBox();
            this.txtTarget = new System.Windows.Forms.MaskedTextBox();
            this.btnDijkstra = new System.Windows.Forms.Button();
            this.btnGeodesicMatrix = new System.Windows.Forms.Button();
            this.btnFPS = new System.Windows.Forms.Button();
            this.btnGauss = new System.Windows.Forms.Button();
            this.btnAverageGeo = new System.Windows.Forms.Button();
            this.btnIsoCurve = new System.Windows.Forms.Button();
            this.chartIsoCurve = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartIsoCurve)).BeginInit();
            this.SuspendLayout();
            // 
            // GLControl
            // 
            this.GLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.GLControl.Location = new System.Drawing.Point(221, 35);
            this.GLControl.MinimumSize = new System.Drawing.Size(400, 400);
            this.GLControl.Name = "GLControl";
            this.GLControl.Size = new System.Drawing.Size(834, 622);
            this.GLControl.TabIndex = 0;
            this.GLControl.VSync = false;
            // 
            // statusBar
            // 
            this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.statusBar.Location = new System.Drawing.Point(0, 775);
            this.statusBar.MinimumSize = new System.Drawing.Size(0, 22);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1055, 22);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusBar1";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenu,
            this.editMenu,
            this.aboutMenu});
            // 
            // fileMenu
            // 
            this.fileMenu.Index = 0;
            this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuImport,
            this.menuExport,
            this.menuItem1,
            this.menuExit});
            this.fileMenu.Text = "File";
            // 
            // menuImport
            // 
            this.menuImport.Index = 0;
            this.menuImport.Text = "Import";
            // 
            // menuExport
            // 
            this.menuExport.Index = 1;
            this.menuExport.Text = "Export";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 3;
            this.menuExit.Text = "Exit";
            // 
            // editMenu
            // 
            this.editMenu.Index = 1;
            this.editMenu.Text = "Edit";
            // 
            // aboutMenu
            // 
            this.aboutMenu.Index = 2;
            this.aboutMenu.Text = "About";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 183);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(221, 310);
            this.treeView1.TabIndex = 4;
            // 
            // Log
            // 
            this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.Log.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Log.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Log.Location = new System.Drawing.Point(0, 663);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(1055, 113);
            this.Log.TabIndex = 5;
            // 
            // toolBar
            // 
            this.toolBar.Divider = false;
            this.toolBar.DropDownArrows = true;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(1055, 40);
            this.toolBar.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.button1.BackgroundImage = global::VertexBenderCS.Properties.Resources.Perspective2;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.ForeColor = System.Drawing.Color.Transparent;
            this.button1.Location = new System.Drawing.Point(4, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 36);
            this.button1.TabIndex = 6;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(70, 12);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(36, 20);
            this.txtSource.TabIndex = 7;
            this.txtSource.Text = "0";
            this.txtSource.ValidatingType = typeof(int);
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(112, 12);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(41, 20);
            this.txtTarget.TabIndex = 8;
            this.txtTarget.Text = "1";
            this.txtTarget.ValidatingType = typeof(int);
            // 
            // btnDijkstra
            // 
            this.btnDijkstra.Location = new System.Drawing.Point(159, 10);
            this.btnDijkstra.Name = "btnDijkstra";
            this.btnDijkstra.Size = new System.Drawing.Size(75, 23);
            this.btnDijkstra.TabIndex = 9;
            this.btnDijkstra.Text = "Dijkstra";
            this.btnDijkstra.UseVisualStyleBackColor = true;
            // 
            // btnGeodesicMatrix
            // 
            this.btnGeodesicMatrix.Location = new System.Drawing.Point(251, 1);
            this.btnGeodesicMatrix.Name = "btnGeodesicMatrix";
            this.btnGeodesicMatrix.Size = new System.Drawing.Size(75, 34);
            this.btnGeodesicMatrix.TabIndex = 10;
            this.btnGeodesicMatrix.Text = "Geodesic Matrix";
            this.btnGeodesicMatrix.UseVisualStyleBackColor = true;
            // 
            // btnFPS
            // 
            this.btnFPS.Location = new System.Drawing.Point(332, 0);
            this.btnFPS.Name = "btnFPS";
            this.btnFPS.Size = new System.Drawing.Size(82, 34);
            this.btnFPS.TabIndex = 11;
            this.btnFPS.Text = "Fathest Point Sample";
            this.btnFPS.UseVisualStyleBackColor = true;
            // 
            // btnGauss
            // 
            this.btnGauss.Location = new System.Drawing.Point(420, 0);
            this.btnGauss.Name = "btnGauss";
            this.btnGauss.Size = new System.Drawing.Size(75, 34);
            this.btnGauss.TabIndex = 12;
            this.btnGauss.Text = "Gaussian Curvature";
            this.btnGauss.UseVisualStyleBackColor = true;
            // 
            // btnAverageGeo
            // 
            this.btnAverageGeo.Location = new System.Drawing.Point(501, 0);
            this.btnAverageGeo.Name = "btnAverageGeo";
            this.btnAverageGeo.Size = new System.Drawing.Size(104, 34);
            this.btnAverageGeo.TabIndex = 13;
            this.btnAverageGeo.Text = "Average Geodesic Distance";
            this.btnAverageGeo.UseVisualStyleBackColor = true;
            // 
            // btnIsoCurve
            // 
            this.btnIsoCurve.Location = new System.Drawing.Point(611, 0);
            this.btnIsoCurve.Name = "btnIsoCurve";
            this.btnIsoCurve.Size = new System.Drawing.Size(104, 34);
            this.btnIsoCurve.TabIndex = 14;
            this.btnIsoCurve.Text = "Iso-Curve Signature";
            this.btnIsoCurve.UseVisualStyleBackColor = true;
            // 
            // chartIsoCurve
            // 
            this.chartIsoCurve.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.chartIsoCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartIsoCurve.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            this.chartIsoCurve.BorderSkin.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Center;
            chartArea1.Name = "isoCurve-Distances";
            this.chartIsoCurve.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartIsoCurve.Legends.Add(legend1);
            this.chartIsoCurve.Location = new System.Drawing.Point(728, 53);
            this.chartIsoCurve.Margin = new System.Windows.Forms.Padding(0);
            this.chartIsoCurve.Name = "chartIsoCurve";
            this.chartIsoCurve.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series1.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Center;
            series1.ChartArea = "isoCurve-Distances";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series1.CustomProperties = "PointWidth=0.3";
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.MarkerSize = 3;
            series1.Name = "IsoCurve";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chartIsoCurve.Series.Add(series1);
            this.chartIsoCurve.Size = new System.Drawing.Size(316, 200);
            this.chartIsoCurve.TabIndex = 15;
            this.chartIsoCurve.Text = "chart1";
            this.chartIsoCurve.Visible = false;
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1055, 797);
            this.Controls.Add(this.chartIsoCurve);
            this.Controls.Add(this.btnIsoCurve);
            this.Controls.Add(this.btnAverageGeo);
            this.Controls.Add(this.btnGauss);
            this.Controls.Add(this.btnFPS);
            this.Controls.Add(this.btnGeodesicMatrix);
            this.Controls.Add(this.btnDijkstra);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.GLControl);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Red;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainWin";
            this.Text = "Vertex Bender Framework";
            ((System.ComponentModel.ISupportInitialize)(this.chartIsoCurve)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl GLControl;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.MenuItem fileMenu;
        private System.Windows.Forms.MenuItem editMenu;
        private System.Windows.Forms.MenuItem aboutMenu;
        public System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuItem menuImport;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.MenuItem menuExport;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.ToolBar toolBar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MaskedTextBox txtSource;
        private System.Windows.Forms.MaskedTextBox txtTarget;
        private System.Windows.Forms.Button btnDijkstra;
        private System.Windows.Forms.Button btnGeodesicMatrix;
        private System.Windows.Forms.Button btnFPS;
        private System.Windows.Forms.Button btnGauss;
        private System.Windows.Forms.Button btnAverageGeo;
        private System.Windows.Forms.Button btnIsoCurve;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartIsoCurve;
    }
}

