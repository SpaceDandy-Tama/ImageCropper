namespace ArdaCropper
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonDone = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxClip = new System.Windows.Forms.CheckBox();
            this.checkBoxSave = new System.Windows.Forms.CheckBox();
            this.comboBoxSaveDir = new System.Windows.Forms.ComboBox();
            this.checkBoxRegistry = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMenu = new System.Windows.Forms.CheckBox();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.labelFormat = new System.Windows.Forms.Label();
            this.checkBoxHotkey = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(13, 153);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(190, 23);
            this.buttonDone.TabIndex = 0;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Double click this icon to capture and crop part of your screen into your clipboar" +
    "d.";
            this.notifyIcon1.BalloonTipTitle = "Proper Cropper Mate";
            this.notifyIcon1.Text = "ArdaCropper";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // checkBoxClip
            // 
            this.checkBoxClip.AutoSize = true;
            this.checkBoxClip.Location = new System.Drawing.Point(13, 13);
            this.checkBoxClip.Name = "checkBoxClip";
            this.checkBoxClip.Size = new System.Drawing.Size(109, 17);
            this.checkBoxClip.TabIndex = 1;
            this.checkBoxClip.Text = "Copy to Clipboard";
            this.checkBoxClip.UseVisualStyleBackColor = true;
            // 
            // checkBoxSave
            // 
            this.checkBoxSave.AutoSize = true;
            this.checkBoxSave.Location = new System.Drawing.Point(13, 36);
            this.checkBoxSave.Name = "checkBoxSave";
            this.checkBoxSave.Size = new System.Drawing.Size(63, 17);
            this.checkBoxSave.TabIndex = 1;
            this.checkBoxSave.Text = "Save to";
            this.checkBoxSave.UseVisualStyleBackColor = true;
            // 
            // comboBoxSaveDir
            // 
            this.comboBoxSaveDir.FormattingEnabled = true;
            this.comboBoxSaveDir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxSaveDir.Location = new System.Drawing.Point(82, 34);
            this.comboBoxSaveDir.Name = "comboBoxSaveDir";
            this.comboBoxSaveDir.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSaveDir.TabIndex = 2;
            // 
            // checkBoxRegistry
            // 
            this.checkBoxRegistry.AutoSize = true;
            this.checkBoxRegistry.Location = new System.Drawing.Point(13, 107);
            this.checkBoxRegistry.Name = "checkBoxRegistry";
            this.checkBoxRegistry.Size = new System.Drawing.Size(118, 17);
            this.checkBoxRegistry.TabIndex = 1;
            this.checkBoxRegistry.Text = "Startup via Registry";
            this.checkBoxRegistry.UseVisualStyleBackColor = true;
            this.checkBoxRegistry.Click += new System.EventHandler(this.checkBoxRegistry_Click);
            // 
            // checkBoxStartMenu
            // 
            this.checkBoxStartMenu.AutoSize = true;
            this.checkBoxStartMenu.Location = new System.Drawing.Point(13, 130);
            this.checkBoxStartMenu.Name = "checkBoxStartMenu";
            this.checkBoxStartMenu.Size = new System.Drawing.Size(132, 17);
            this.checkBoxStartMenu.TabIndex = 1;
            this.checkBoxStartMenu.Text = "Startup via Start Menu";
            this.checkBoxStartMenu.UseVisualStyleBackColor = true;
            this.checkBoxStartMenu.Click += new System.EventHandler(this.checkBoxStartMenu_Click);
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Items.AddRange(new object[] {
            ".png",
            ".bmp",
            ".jpg"});
            this.comboBoxFormat.Location = new System.Drawing.Point(82, 55);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFormat.TabIndex = 2;
            // 
            // labelFormat
            // 
            this.labelFormat.AutoSize = true;
            this.labelFormat.Location = new System.Drawing.Point(13, 60);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(70, 13);
            this.labelFormat.TabIndex = 3;
            this.labelFormat.Text = "Save Format:";
            // 
            // checkBoxHotkey
            // 
            this.checkBoxHotkey.AutoSize = true;
            this.checkBoxHotkey.Location = new System.Drawing.Point(13, 84);
            this.checkBoxHotkey.Name = "checkBoxHotkey";
            this.checkBoxHotkey.Size = new System.Drawing.Size(101, 17);
            this.checkBoxHotkey.TabIndex = 4;
            this.checkBoxHotkey.Text = "Enable Hotkeys";
            this.checkBoxHotkey.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 185);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxHotkey);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.comboBoxFormat);
            this.Controls.Add(this.comboBoxSaveDir);
            this.Controls.Add(this.checkBoxStartMenu);
            this.Controls.Add(this.checkBoxRegistry);
            this.Controls.Add(this.checkBoxSave);
            this.Controls.Add(this.checkBoxClip);
            this.Controls.Add(this.buttonDone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ArdaCropper Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox checkBoxClip;
        private System.Windows.Forms.CheckBox checkBoxSave;
        private System.Windows.Forms.ComboBox comboBoxSaveDir;
        private System.Windows.Forms.CheckBox checkBoxRegistry;
        private System.Windows.Forms.CheckBox checkBoxStartMenu;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.CheckBox checkBoxHotkey;
    }
}

