namespace TriangleVisualizer
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.canvas = new TriangleVisualizer.DoubleBufferedPanel();
            this.accordion1 = new Opulos.Core.UI.Accordion();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(5, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.canvas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.accordion1);
            this.splitContainer1.Panel2MinSize = 250;
            this.splitContainer1.Size = new System.Drawing.Size(744, 453);
            this.splitContainer1.SplitterDistance = 399;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.TabStop = false;
            this.splitContainer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_MouseUp);
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(395, 449);
            this.canvas.TabIndex = 0;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // accordion1
            // 
            this.accordion1.AllowMouseResize = true;
            this.accordion1.AutoScroll = true;
            this.accordion1.AutoSize = true;
            this.accordion1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.accordion1.BackColor = System.Drawing.Color.White;
            this.accordion1.CheckBoxFactory = null;
            this.accordion1.CheckBoxMargin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.accordion1.ContentBackColor = null;
            this.accordion1.ContentMargin = null;
            this.accordion1.ContentPadding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.accordion1.ControlBackColor = null;
            this.accordion1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accordion1.DownArrow = null;
            this.accordion1.FillHeight = true;
            this.accordion1.FillLastOpened = false;
            this.accordion1.FillModeGrowOnly = false;
            this.accordion1.FillResetOnCollapse = false;
            this.accordion1.FillWidth = true;
            this.accordion1.GrabCursor = System.Windows.Forms.Cursors.SizeNS;
            this.accordion1.GrabRequiresPositiveFillWeight = true;
            this.accordion1.GrabWidth = 6;
            this.accordion1.GrowAndShrink = true;
            this.accordion1.Insets = new System.Windows.Forms.Padding(0);
            this.accordion1.Location = new System.Drawing.Point(0, 0);
            this.accordion1.Name = "accordion1";
            this.accordion1.OpenOnAdd = false;
            this.accordion1.OpenOneOnly = false;
            this.accordion1.ShowToolMenu = false;
            this.accordion1.ShowToolMenuOnHoverWhenClosed = false;
            this.accordion1.ShowToolMenuOnRightClick = true;
            this.accordion1.ShowToolMenuRequiresPositiveFillWeight = false;
            this.accordion1.Size = new System.Drawing.Size(337, 449);
            this.accordion1.TabIndex = 1;
            this.accordion1.UpArrow = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 463);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel canvas;
        private Opulos.Core.UI.Accordion accordion1;
        private System.Windows.Forms.SplitContainer splitContainer1;


    }
}

