﻿namespace VertexBenderCS.Forms
{
    partial class FrmProcess
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
            this.tabProcess = new System.Windows.Forms.TabControl();
            this.tabSPH = new System.Windows.Forms.TabPage();
            this.tabGm = new System.Windows.Forms.TabPage();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tabDescriptor = new System.Windows.Forms.TabPage();
            this.numericStartIndex = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.radioISO = new System.Windows.Forms.RadioButton();
            this.radioAGD = new System.Windows.Forms.RadioButton();
            this.radioFPS = new System.Windows.Forms.RadioButton();
            this.labelSampleCount = new System.Windows.Forms.Label();
            this.numericSampleCount = new System.Windows.Forms.NumericUpDown();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.numericSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTarget)).BeginInit();
            this.tabProcess.SuspendLayout();
            this.tabSPH.SuspendLayout();
            this.tabGm.SuspendLayout();
            this.imagePanel.SuspendLayout();
            this.tabDescriptor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericStartIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleCount)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbMethod
            // 
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            "Array",
            "Minimum Heap",
            "Fibonacci",
            "A*",
            "All"});
            this.cmbMethod.Location = new System.Drawing.Point(141, 52);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(80, 23);
            this.cmbMethod.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label1.Location = new System.Drawing.Point(76, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Method:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Ebrima", 9F);
            this.label3.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label3.Location = new System.Drawing.Point(7, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Vertex Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaxVertex
            // 
            this.labelMaxVertex.AutoSize = true;
            this.labelMaxVertex.Font = new System.Drawing.Font("Ebrima", 9F);
            this.labelMaxVertex.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.labelMaxVertex.Location = new System.Drawing.Point(88, 7);
            this.labelMaxVertex.Name = "labelMaxVertex";
            this.labelMaxVertex.Size = new System.Drawing.Size(25, 15);
            this.labelMaxVertex.TabIndex = 6;
            this.labelMaxVertex.Text = "200";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label2.Location = new System.Drawing.Point(63, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Start Index:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label5.Location = new System.Drawing.Point(67, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "End Index:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(183, 343);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(96, 26);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // numericSource
            // 
            this.numericSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericSource.Location = new System.Drawing.Point(141, 89);
            this.numericSource.Name = "numericSource";
            this.numericSource.Size = new System.Drawing.Size(80, 24);
            this.numericSource.TabIndex = 10;
            // 
            // numericTarget
            // 
            this.numericTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericTarget.Location = new System.Drawing.Point(141, 129);
            this.numericTarget.Name = "numericTarget";
            this.numericTarget.Size = new System.Drawing.Size(80, 24);
            this.numericTarget.TabIndex = 11;
            // 
            // tabProcess
            // 
            this.tabProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabProcess.Controls.Add(this.tabSPH);
            this.tabProcess.Controls.Add(this.tabGm);
            this.tabProcess.Controls.Add(this.tabDescriptor);
            this.tabProcess.Font = new System.Drawing.Font("Ebrima", 9F);
            this.tabProcess.Location = new System.Drawing.Point(-2, 33);
            this.tabProcess.Name = "tabProcess";
            this.tabProcess.SelectedIndex = 0;
            this.tabProcess.Size = new System.Drawing.Size(285, 305);
            this.tabProcess.TabIndex = 12;
            // 
            // tabSPH
            // 
            this.tabSPH.Controls.Add(this.label1);
            this.tabSPH.Controls.Add(this.numericTarget);
            this.tabSPH.Controls.Add(this.label2);
            this.tabSPH.Controls.Add(this.cmbMethod);
            this.tabSPH.Controls.Add(this.label5);
            this.tabSPH.Controls.Add(this.numericSource);
            this.tabSPH.Location = new System.Drawing.Point(4, 24);
            this.tabSPH.Name = "tabSPH";
            this.tabSPH.Padding = new System.Windows.Forms.Padding(3);
            this.tabSPH.Size = new System.Drawing.Size(277, 277);
            this.tabSPH.TabIndex = 0;
            this.tabSPH.Text = "Shortest Path";
            this.tabSPH.UseVisualStyleBackColor = true;
            // 
            // tabGm
            // 
            this.tabGm.Controls.Add(this.imagePanel);
            this.tabGm.Location = new System.Drawing.Point(4, 24);
            this.tabGm.Name = "tabGm";
            this.tabGm.Padding = new System.Windows.Forms.Padding(3);
            this.tabGm.Size = new System.Drawing.Size(277, 277);
            this.tabGm.TabIndex = 1;
            this.tabGm.Text = "Geodesic Matrix";
            this.tabGm.UseVisualStyleBackColor = true;
            // 
            // imagePanel
            // 
            this.imagePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imagePanel.Controls.Add(this.label4);
            this.imagePanel.Location = new System.Drawing.Point(3, 4);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(270, 270);
            this.imagePanel.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Output will be drawn here";
            // 
            // tabDescriptor
            // 
            this.tabDescriptor.Controls.Add(this.numericStartIndex);
            this.tabDescriptor.Controls.Add(this.label7);
            this.tabDescriptor.Controls.Add(this.radioISO);
            this.tabDescriptor.Controls.Add(this.radioAGD);
            this.tabDescriptor.Controls.Add(this.radioFPS);
            this.tabDescriptor.Controls.Add(this.labelSampleCount);
            this.tabDescriptor.Controls.Add(this.numericSampleCount);
            this.tabDescriptor.Location = new System.Drawing.Point(4, 24);
            this.tabDescriptor.Name = "tabDescriptor";
            this.tabDescriptor.Size = new System.Drawing.Size(277, 277);
            this.tabDescriptor.TabIndex = 2;
            this.tabDescriptor.Text = "Descriptor";
            this.tabDescriptor.UseVisualStyleBackColor = true;
            // 
            // numericStartIndex
            // 
            this.numericStartIndex.Location = new System.Drawing.Point(176, 197);
            this.numericStartIndex.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericStartIndex.Name = "numericStartIndex";
            this.numericStartIndex.Size = new System.Drawing.Size(55, 24);
            this.numericStartIndex.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.label7.Location = new System.Drawing.Point(106, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Start Index:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioISO
            // 
            this.radioISO.AutoSize = true;
            this.radioISO.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.radioISO.Location = new System.Drawing.Point(61, 120);
            this.radioISO.Name = "radioISO";
            this.radioISO.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioISO.Size = new System.Drawing.Size(128, 19);
            this.radioISO.TabIndex = 15;
            this.radioISO.Text = "Iso-Curve Signature";
            this.radioISO.UseVisualStyleBackColor = true;
            // 
            // radioAGD
            // 
            this.radioAGD.AutoSize = true;
            this.radioAGD.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.radioAGD.Location = new System.Drawing.Point(22, 80);
            this.radioAGD.Name = "radioAGD";
            this.radioAGD.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioAGD.Size = new System.Drawing.Size(167, 19);
            this.radioAGD.TabIndex = 14;
            this.radioAGD.Text = "Average Geodesic Distance";
            this.radioAGD.UseVisualStyleBackColor = true;
            // 
            // radioFPS
            // 
            this.radioFPS.AutoSize = true;
            this.radioFPS.Checked = true;
            this.radioFPS.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.radioFPS.Location = new System.Drawing.Point(39, 40);
            this.radioFPS.Name = "radioFPS";
            this.radioFPS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioFPS.Size = new System.Drawing.Size(150, 19);
            this.radioFPS.TabIndex = 13;
            this.radioFPS.TabStop = true;
            this.radioFPS.Text = "Farthest Point Sampling";
            this.radioFPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioFPS.UseVisualStyleBackColor = true;
            // 
            // labelSampleCount
            // 
            this.labelSampleCount.AutoSize = true;
            this.labelSampleCount.ForeColor = System.Drawing.Color.MediumAquamarine;
            this.labelSampleCount.Location = new System.Drawing.Point(89, 159);
            this.labelSampleCount.Name = "labelSampleCount";
            this.labelSampleCount.Size = new System.Drawing.Size(83, 15);
            this.labelSampleCount.TabIndex = 11;
            this.labelSampleCount.Text = "Sample Count:";
            this.labelSampleCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericSampleCount
            // 
            this.numericSampleCount.Location = new System.Drawing.Point(176, 156);
            this.numericSampleCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericSampleCount.Name = "numericSampleCount";
            this.numericSampleCount.Size = new System.Drawing.Size(55, 24);
            this.numericSampleCount.TabIndex = 12;
            this.numericSampleCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(5, 344);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(175, 24);
            this.progressBar1.TabIndex = 13;
            // 
            // FrmProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(282, 379);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tabProcess);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelMaxVertex);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Ebrima", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Process";
            ((System.ComponentModel.ISupportInitialize)(this.numericSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTarget)).EndInit();
            this.tabProcess.ResumeLayout(false);
            this.tabSPH.ResumeLayout(false);
            this.tabSPH.PerformLayout();
            this.tabGm.ResumeLayout(false);
            this.imagePanel.ResumeLayout(false);
            this.imagePanel.PerformLayout();
            this.tabDescriptor.ResumeLayout(false);
            this.tabDescriptor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericStartIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleCount)).EndInit();
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
        private System.Windows.Forms.TabControl tabProcess;
        private System.Windows.Forms.TabPage tabSPH;
        private System.Windows.Forms.TabPage tabGm;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TabPage tabDescriptor;
        private System.Windows.Forms.RadioButton radioISO;
        private System.Windows.Forms.RadioButton radioAGD;
        private System.Windows.Forms.RadioButton radioFPS;
        private System.Windows.Forms.Label labelSampleCount;
        private System.Windows.Forms.NumericUpDown numericSampleCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericStartIndex;
        private System.Windows.Forms.Label label7;
    }
}