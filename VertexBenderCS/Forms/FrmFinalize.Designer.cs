namespace VertexBenderCS.Forms
{
    partial class FrmFinalize
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
            this.btnProcede = new System.Windows.Forms.Button();
            this.chkIsland = new System.Windows.Forms.CheckBox();
            this.chkSmoothen = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkFill = new System.Windows.Forms.CheckBox();
            this.numericSmoothen = new System.Windows.Forms.NumericUpDown();
            this.numericFill = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericSmoothen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFill)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProcede
            // 
            this.btnProcede.Location = new System.Drawing.Point(30, 97);
            this.btnProcede.Name = "btnProcede";
            this.btnProcede.Size = new System.Drawing.Size(75, 23);
            this.btnProcede.TabIndex = 0;
            this.btnProcede.Text = "Procede";
            this.btnProcede.UseVisualStyleBackColor = true;
            // 
            // chkIsland
            // 
            this.chkIsland.AutoSize = true;
            this.chkIsland.Location = new System.Drawing.Point(12, 12);
            this.chkIsland.Name = "chkIsland";
            this.chkIsland.Size = new System.Drawing.Size(181, 17);
            this.chkIsland.TabIndex = 1;
            this.chkIsland.Text = "Remove Disconnected Elements";
            this.chkIsland.UseVisualStyleBackColor = true;
            // 
            // chkSmoothen
            // 
            this.chkSmoothen.AutoSize = true;
            this.chkSmoothen.Location = new System.Drawing.Point(12, 35);
            this.chkSmoothen.Name = "chkSmoothen";
            this.chkSmoothen.Size = new System.Drawing.Size(103, 17);
            this.chkSmoothen.TabIndex = 2;
            this.chkSmoothen.Text = "Smoothen Mesh";
            this.chkSmoothen.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(138, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkFill
            // 
            this.chkFill.AutoSize = true;
            this.chkFill.Location = new System.Drawing.Point(12, 58);
            this.chkFill.Name = "chkFill";
            this.chkFill.Size = new System.Drawing.Size(103, 17);
            this.chkFill.TabIndex = 4;
            this.chkFill.Text = "Smoothen Mesh";
            this.chkFill.UseVisualStyleBackColor = true;
            // 
            // numericSmoothen
            // 
            this.numericSmoothen.Location = new System.Drawing.Point(121, 32);
            this.numericSmoothen.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericSmoothen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSmoothen.Name = "numericSmoothen";
            this.numericSmoothen.ReadOnly = true;
            this.numericSmoothen.Size = new System.Drawing.Size(37, 20);
            this.numericSmoothen.TabIndex = 34;
            this.numericSmoothen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericFill
            // 
            this.numericFill.Location = new System.Drawing.Point(121, 58);
            this.numericFill.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericFill.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericFill.Name = "numericFill";
            this.numericFill.ReadOnly = true;
            this.numericFill.Size = new System.Drawing.Size(37, 20);
            this.numericFill.TabIndex = 35;
            this.numericFill.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FrmFinalize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 132);
            this.Controls.Add(this.numericFill);
            this.Controls.Add(this.numericSmoothen);
            this.Controls.Add(this.chkFill);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkSmoothen);
            this.Controls.Add(this.chkIsland);
            this.Controls.Add(this.btnProcede);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmFinalize";
            this.Text = "FinalizeForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericSmoothen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFill)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcede;
        private System.Windows.Forms.CheckBox chkIsland;
        private System.Windows.Forms.CheckBox chkSmoothen;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkFill;
        private System.Windows.Forms.NumericUpDown numericSmoothen;
        private System.Windows.Forms.NumericUpDown numericFill;
    }
}