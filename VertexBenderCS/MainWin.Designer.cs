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
            this.txtSource.Location = new System.Drawing.Point(70, 1);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(36, 20);
            this.txtSource.TabIndex = 7;
            this.txtSource.ValidatingType = typeof(int);
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(112, 1);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(41, 20);
            this.txtTarget.TabIndex = 8;
            this.txtTarget.ValidatingType = typeof(int);
            // 
            // btnDijkstra
            // 
            this.btnDijkstra.Location = new System.Drawing.Point(159, -1);
            this.btnDijkstra.Name = "btnDijkstra";
            this.btnDijkstra.Size = new System.Drawing.Size(75, 23);
            this.btnDijkstra.TabIndex = 9;
            this.btnDijkstra.Text = "button2";
            this.btnDijkstra.UseVisualStyleBackColor = true;
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1055, 797);
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
    }
}

