using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Win32;

namespace ArdaCropper
{
    public partial class Form1 : Form
    {

        private ContextMenu contextMenu1;
        private MenuItem menuItemExit;
        private MenuItem menuItemSettings;

        #region App Settings

        [DataContract]
        public class AppSetting
        {
            [DataMember]
            public bool CopyToClipboard = true;
            [DataMember]
            public bool SaveToDisk = false;
            [DataMember]
            public int SaveDirIndex = 0;
            [DataMember]
            public string SaveDirCustom = "none";
            [DataMember]
            public int SaveFormatIndex = 0;
            [DataMember]
            public bool StartupViaRegistry = false;
            [DataMember]
            public bool StartupViaStartMenu = false;
        }

        private void SaveAppSetting()
        {
            MemoryStream memoryStream = new MemoryStream();
            Ser.WriteObject(memoryStream, Settings);
            using (FileStream fileStream = File.Create(AppSettingPath))
            {
                memoryStream.Position = 0;
                memoryStream.CopyTo(fileStream);
            }
        }

        private bool LoadAppSetting()
        {
            Ser = new DataContractJsonSerializer(typeof(AppSetting));
            if (File.Exists(AppSettingPath))
            {
                using (FileStream fileStream = File.OpenRead(AppSettingPath))
                {
                    Settings = (AppSetting)Ser.ReadObject(fileStream);
                    return true;
                }
            }
            return false;
        }
        #endregion

        public AppSetting Settings;
        public string shortcutPath;

        public string AppSettingPath = "AppSetting.json";
        public DataContractJsonSerializer Ser;

        public bool isCropping = false;
        public Point StartPoint;
        public Point EndPoint;

        public List<CropForm> CropForms = new List<CropForm>();

        public Form1()
        {
            //Don't touch this
            InitializeComponent();

            //Setup
            notifyIcon1.Icon = this.Icon;

            contextMenu1 = new ContextMenu();

            menuItemSettings = new MenuItem();
            menuItemExit = new MenuItem();

            contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItemSettings, menuItemExit });

            menuItemSettings.Index = 0;
            menuItemSettings.Text = "S&ettings";
            menuItemSettings.Click += new EventHandler(menuItemSettings_Click);

            menuItemExit.Index = 1;
            menuItemExit.Text = "E&xit";
            menuItemExit.Click += new EventHandler(menuItemExit_Click);

            notifyIcon1.ContextMenu = contextMenu1;

            shortcutPath = Path.Combine(Application.StartupPath, "ArdaCropper.lnk");

            comboBoxSaveDir.Items.Add("MyPictures");
            comboBoxSaveDir.Items.Add("Desktop");
            //comboBoxSaveDir.Items.Add("Custom");

            AppSettingPath = Path.Combine(Application.StartupPath, AppSettingPath);

            if (!LoadAppSetting())
            {
                Settings = new AppSetting();
                if (Environment.OSVersion.Version.ToString().StartsWith("10"))
                    Settings.StartupViaRegistry = true;
                else
                    Settings.StartupViaStartMenu = true;
                SaveAppSetting();
            }

            checkBoxClip.Check(Settings.CopyToClipboard);
            checkBoxSave.Check(Settings.SaveToDisk);
            comboBoxSaveDir.SelectedIndex = Settings.SaveDirIndex;
            comboBoxFormat.SelectedIndex = Settings.SaveFormatIndex;
            checkBoxRegistry.Check(RegisterInStartup(Settings.StartupViaRegistry));
            checkBoxStartMenu.Check(ShortcutInStartup(Settings.StartupViaStartMenu));

#if !Windows
            checkBoxRegistry.Enabled = false;
            checkBoxStartMenu.Enabled = false;
            this.Height = 200;
#endif

            //Start Mizimized
            Minimize();
            this.ShowInTaskbar = false;
            notifyIcon1.ShowBalloonTip(2000);
        }

        public void Minimize()
        {
            this.Hide();
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            //Don't need to do this, it results in a bug anyway.
            //this.ShowInTaskbar = false;
        }

        public void Normalize()
        {
            this.Show();
            notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        public void GetScreenshot()
        {
            if (EndPoint.X < StartPoint.X)
            {
                Point temp = EndPoint;
                EndPoint.X = StartPoint.X;
                StartPoint.X = temp.X;
            }
            else if(EndPoint.X == StartPoint.X)
                return;

            if (EndPoint.Y < StartPoint.Y)
            {
                Point temp = EndPoint;
                EndPoint.Y = StartPoint.Y;
                StartPoint.Y = temp.Y;
            }
            else if (EndPoint.Y == StartPoint.Y)
                return;

            Size resolution = new Size(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);

            Bitmap bmpScreenshot = new Bitmap(resolution.Width, resolution.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(StartPoint.X, StartPoint.Y, 0, 0, resolution, CopyPixelOperation.SourceCopy);

            if(Settings.CopyToClipboard)
                Clipboard.SetImage(bmpScreenshot);

            if (Settings.SaveToDisk)
            {
                string saveDir = "";
                if (Settings.SaveDirIndex == 0)
                    saveDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                else if (Settings.SaveDirIndex == 1)
                    saveDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                else if (Settings.SaveDirIndex == 2)
                    saveDir = Settings.SaveDirCustom;

                string fileName = "Screencrop";
                if (resolution.Width == Screen.PrimaryScreen.Bounds.Width && resolution.Height == Screen.PrimaryScreen.Bounds.Height)
                    fileName = "Screenshot";

                int count = 0;
                string[] files = Directory.GetFiles(saveDir);
                foreach(string file in files)
                {
                    string temp = Path.GetFileName(file);
                    if (temp.StartsWith(fileName, StringComparison.Ordinal))
                        count++;
                }

                saveDir = Path.Combine(saveDir, fileName + count.ToString());

                ImageFormat imageFormat;
                if (Settings.SaveFormatIndex == 0)
                {
                    imageFormat = ImageFormat.Png;
                    saveDir += ".png";
                }
                else if (Settings.SaveFormatIndex == 1)
                {
                    imageFormat = ImageFormat.Bmp;
                    saveDir += ".bmp";
                }
                else
                {
                    imageFormat = ImageFormat.Jpeg;
                    saveDir += ".jpg";
                }

#if DEBUG
                using (StreamWriter sw = new StreamWriter("log.log"))
                {
                    sw.WriteLine(saveDir);
                }
#endif

                bmpScreenshot.Save(saveDir, imageFormat);
            }

            bmpScreenshot.Dispose();
            gfxScreenshot.Dispose();
        }

        private bool RegisterInStartup(bool isChecked)
        {
#if !Windows
            return false;
#else
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                if(registryKey.GetValue("ArdaCropper") == null)
                    registryKey.SetValue("ArdaCropper", Application.ExecutablePath);
            }
            else
            {
                if (registryKey.GetValue("ArdaCropper") != null)
                    registryKey.DeleteValue("ArdaCropper");
            }

            return isChecked;
#endif
        }

        public bool ShortcutInStartup(bool isChecked)
        {
#if !Windows
            return false;
#else
            string startupDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (isChecked)
            {
                //if(!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\ArdaCropper.lnk"))
                    File.Copy(shortcutPath, Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\ArdaCropper.lnk", true);
            }
            else
            {
                string[] filesUnderStartup = Directory.GetFiles(startupDir);
                foreach (string fileName in filesUnderStartup)
                {
                    string[] temp = fileName.Split('\\');
                    if (temp[temp.Length-1].StartsWith("ArdaCropper", StringComparison.Ordinal))
                    {
                        File.Delete(fileName);
                        break;
                    }
                }
            }

            return isChecked;
#endif
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            //GetScreenshot();

            isCropping = true;

            CropForms.Clear();

            foreach (Screen screen in Screen.AllScreens)
            {
                CropForm tempCropForm = new CropForm(this, screen);
                tempCropForm.Show();
                CropForms.Add(tempCropForm);
            }

        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            Normalize();
        }

        private void menuItemExit_Click(object Sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Settings.CopyToClipboard = checkBoxClip.Checked;
            Settings.SaveToDisk = checkBoxSave.Checked;
            Settings.SaveDirIndex = comboBoxSaveDir.SelectedIndex;
            Settings.SaveDirCustom = "none";
            Settings.SaveFormatIndex = comboBoxFormat.SelectedIndex;
            Settings.StartupViaRegistry = RegisterInStartup(checkBoxRegistry.Checked);
            Settings.StartupViaStartMenu = ShortcutInStartup(checkBoxStartMenu.Checked);

            SaveAppSetting();

            Minimize();
        }

        private void checkBoxRegistry_Click(object sender, EventArgs e)
        {
            if (checkBoxRegistry.CheckState == CheckState.Checked)
                checkBoxStartMenu.Check(false);
        }

        private void checkBoxStartMenu_Click(object sender, EventArgs e)
        {
            if (checkBoxStartMenu.CheckState == CheckState.Checked)
            {
                checkBoxRegistry.Check(false);

                if (Environment.OSVersion.Version.ToString().StartsWith("10"))
                    MessageBox.Show("Sorry, but this feature does not work in Windows 10. You'd probably be better off using registry startup.");
            }
        }
    }

}
