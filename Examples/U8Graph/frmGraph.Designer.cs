﻿namespace U8Graph
{
    partial class frmGraph
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
            this.SuspendLayout();
            // 
            // frmGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(718, 702);
            this.Name = "frmGraph";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Graph View";
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.frmGraph_Scroll);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

