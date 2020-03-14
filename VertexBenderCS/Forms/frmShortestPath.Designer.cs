namespace VertexBenderCS.Forms
{
    partial class frmShortestPath
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
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMaxVertex = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.numericSource = new System.Windows.Forms.NumericUpDown();
            this.numericTarget = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbMethod
            // 
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            "Array",
            "Minimum Heap",
            "Fibonacci",
            "A*",
            "All"});
            this.cmbMethod.Location = new System.Drawing.Point(93, 35);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(117, 21);
            this.cmbMethod.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label1.Location = new System.Drawing.Point(41, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Method:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label3.Location = new System.Drawing.Point(16, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Vertex Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaxVertex
            // 
            this.labelMaxVertex.AutoSize = true;
            this.labelMaxVertex.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.labelMaxVertex.Location = new System.Drawing.Point(93, 9);
            this.labelMaxVertex.Name = "labelMaxVertex";
            this.labelMaxVertex.Size = new System.Drawing.Size(25, 13);
            this.labelMaxVertex.TabIndex = 6;
            this.labelMaxVertex.Text = "200";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label2.Location = new System.Drawing.Point(26, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Start Index:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label5.Location = new System.Drawing.Point(26, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "End Index:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(66, 123);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // numericSource
            // 
            this.numericSource.Location = new System.Drawing.Point(93, 65);
            this.numericSource.Name = "numericSource";
            this.numericSource.Size = new System.Drawing.Size(117, 20);
            this.numericSource.TabIndex = 10;
            // 
            // numericTarget
            // 
            this.numericTarget.Location = new System.Drawing.Point(93, 89);
            this.numericTarget.Name = "numericTarget";
            this.numericTarget.Size = new System.Drawing.Size(117, 20);
            this.numericTarget.TabIndex = 11;
            // 
            // frmShortestPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(233, 158);
            this.Controls.Add(this.numericTarget);
            this.Controls.Add(this.numericSource);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelMaxVertex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMethod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmShortestPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shortest Path";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTarget)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMaxVertex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NumericUpDown numericSource;
        private System.Windows.Forms.NumericUpDown numericTarget;
    }
}