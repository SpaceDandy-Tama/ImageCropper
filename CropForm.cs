using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArdaCropper
{
    public partial class CropForm : Form
    {
        public Form1 MainForm;
        public Screen screen = null;

        private bool StartDrawing = false;
        private bool EndDrawing = false;

        private Point StartDrawingPoint;

        public CropForm(Form1 mainForm, Screen screen = null)
        {
            InitializeComponent();

            MainForm = mainForm;
            if(screen != null)
                this.screen = screen;
            this.Icon = MainForm.Icon;

            this.BackColor = Color.Black;

            this.Opacity = 0.33f;
            labelResolution.ForeColor = Color.White;
            labelResolution.Text = "";

            this.Activate();
        }

        private void CropForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                StartDrawing = true;
                MainForm.StartPoint = MousePosition;

                StartDrawingPoint = MousePosition;
                pictureBoxArea.Location = new Point(StartDrawingPoint.X - screen.Bounds.Left, StartDrawingPoint.Y - screen.Bounds.Top);
                pictureBoxArea.BackColor = Color.White;
                labelResolution.Location = new Point(StartDrawingPoint.X - screen.Bounds.Left, StartDrawingPoint.Y - screen.Bounds.Top - 20);
            }
        }

        private void CropForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                EndDrawing = true;
                MainForm.EndPoint = MousePosition;

                this.HideAll();
            }
            else if (e.Button == MouseButtons.Right)
                this.DisposeAll();
        }

        private void CropForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (StartDrawing == false)
                return;

            Point startPoint = MainForm.StartPoint;

            int width;
            int height;
            Point pos = StartDrawingPoint;

            if (MousePosition.X > startPoint.X)
                width = MousePosition.X - startPoint.X;
            else if (MousePosition.X < startPoint.X)
            {
                width = startPoint.X - MousePosition.X;
                pos.X -= width;
            }
            else
                width = 0;

            if (MousePosition.Y > startPoint.Y)
                height = MousePosition.Y - startPoint.Y;
            else if (MousePosition.Y < startPoint.Y)
            {
                height = startPoint.Y - MousePosition.Y;
                pos.Y -= height;
            }
            else
                height = 0;

            pictureBoxArea.Location = new Point(pos.X - screen.Bounds.Left, pos.Y - screen.Bounds.Top);

            pictureBoxArea.Size = new Size(width, height);
            labelResolution.Text = pictureBoxArea.Size.Width.ToString() + " x " + pictureBoxArea.Size.Height.ToString();
        }

        private void CropForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.DisposeAll();
            else if (e.KeyCode == Keys.Enter)
            {
                MainForm.StartPoint = new Point(0, 0);
                MainForm.EndPoint = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                StartDrawing = true;
                EndDrawing = true;
                this.HideAll();
            }
        }

        //This event only works when form is shown in linux
        private void CropForm_VisibleChanged(object sender, EventArgs e)
        {
            this.Width = screen != null ? screen.Bounds.Width : 20000;
            this.Height = screen != null ? screen.Bounds.Height : 20000;
            this.Location = screen != null ? screen.Bounds.Location : new Point(-10000, -10000);

#if Windows
            if (StartDrawing && EndDrawing)
            {
                this.DisposeAll();
                MainForm.GetScreenshot();
            }
#endif
        }

        private void HideAll()
        {
            if (MainForm.CropForms.Count == 0)
            {
                this.Hide();
#if !Windows
                this.TriggerScreenshot();
#endif
            }
            else
            {
                foreach (CropForm cropForm in MainForm.CropForms)
                {
                    if (cropForm != null)
                    {
                        cropForm.Hide();
#if !Windows
                        cropForm.TriggerScreenshot();
#endif
                    }
                }
            }
        }

#if !Windows
        private async void TriggerScreenshot()
        {
            if (!StartDrawing || !EndDrawing)
                return;
            
            await Task.Delay(500);

            if (StartDrawing && EndDrawing)
            {
                this.DisposeAll();
                MainForm.GetScreenshot();
            }
        }
#endif

        private void DisposeAll()
        {
            if (MainForm.CropForms.Count == 0)
                this.Dispose();
            else
            {
                foreach (CropForm cropForm in MainForm.CropForms)
                    cropForm?.Dispose();
            }
        }

    }
}
