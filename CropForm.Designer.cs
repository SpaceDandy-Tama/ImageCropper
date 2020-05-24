using System;
using System.Windows.Forms;

namespace ArdaCropper
{
    partial class CropForm
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
            this.labelResolution = new System.Windows.Forms.Label();
            this.pictureBoxArea = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArea)).BeginInit();
            this.SuspendLayout();
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelResolution.Location = new System.Drawing.Point(227, 182);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(51, 20);
            this.labelResolution.TabIndex = 1;
            this.labelResolution.Text = "label1";
            // 
            // pictureBoxArea
            // 
            this.pictureBoxArea.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxArea.Location = new System.Drawing.Point(231, 205);
            this.pictureBoxArea.Name = "pictureBoxArea";
            this.pictureBoxArea.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxArea.TabIndex = 0;
            this.pictureBoxArea.TabStop = false;
            // 
            // CropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelResolution);
            this.Controls.Add(this.pictureBoxArea);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CropForm";
            this.Text = "ArdaCropper";
            this.VisibleChanged += new System.EventHandler(this.CropForm_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CropForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CropForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CropForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CropForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxArea;
        private System.Windows.Forms.Label labelResolution;
    }
}