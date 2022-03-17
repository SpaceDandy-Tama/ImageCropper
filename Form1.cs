using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
#if Windows
using Microsoft.Win32;
using ArdaCropper.KeyboardHook;
#endif

namespace ArdaCropper
{
    public partial class Form1 : Form
    {
#if Windows
        private HotkeyListener Hook;
#endif

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
            [DataMember]
            public bool EnableHotkeys = true;
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
                try
                {
                    using (FileStream fileStream = File.OpenRead(AppSettingPath))
                    {
                        Settings = (AppSetting)Ser.ReadObject(fileStream);

                        return true;
                    }
                }
                catch
                {
                    File.Delete(AppSettingPath);
                }
            }
            return false;
        }
        #endregion

        public AppSetting Settings;
        public string shortcutPath;

        public string AppSettingPath = "ArdaCropperSettings.json";
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

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fileName = AppSettingPath;
            AppSettingPath = Path.Combine(appDataPath, "ArdaSuite");
            if (!Directory.Exists(AppSettingPath))
                Directory.CreateDirectory(AppSettingPath);
            AppSettingPath = Path.Combine(AppSettingPath, fileName);

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
            checkBoxHotkey.Check(Settings.EnableHotkeys);
            checkBoxRegistry.Check(RegisterInStartup(Settings.StartupViaRegistry));
            checkBoxStartMenu.Check(ShortcutInStartup(Settings.StartupViaStartMenu));

#if !Windows
            checkBoxRegistry.Enabled = false;
            checkBoxStartMenu.Enabled = false;
            checkBoxHotkey.Enabled = false;
            this.Height = 200;
#endif

            //Start Mizimized
            DelayedMinimize(); //Fixed setting window not being minimized upon startup

#if Windows
            if (Settings.EnableHotkeys)
            {
                EnableHotkeys();
            }
#endif
        }

#if Windows
        private void EnableHotkeys()
        {
            DisableHotkeys();

            Hook = new HotkeyListener();
            Hook.RegisterHotKey(KeyboardHook.ModifierKeys.Shift, Keys.PrintScreen, Hook_KeyPressed);
            Hook.RegisterHotKey(KeyboardHook.ModifierKeys.Control | KeyboardHook.ModifierKeys.Shift, Keys.PrintScreen, Hook_KeyPressedClipboard);
        }
        private void DisableHotkeys()
        {
            Hook?.Dispose();
            Hook = null;
        }

        private void Hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (isCropping)
                return;

            StartCropping(false);
        }
        private void Hook_KeyPressedClipboard(object sender, KeyPressedEventArgs e)
        {
            if (isCropping)
                return;

            StartCropping(true);
        }
#endif

        public async void DelayedMinimize()
		{
            await System.Threading.Tasks.Task.Delay(1);
            Minimize();
            this.ShowInTaskbar = true;
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

#if !Windows
        Gdk.Pixbuf pixBufScreenshot;
#endif

        public void GetScreenshot(bool toClipboard)
        {
            isCropping = false;

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

            if (toClipboard)
            {
#if Windows
                Clipboard.SetImage(bmpScreenshot);
#else
                MemoryStream myMemoryStream = new MemoryStream();
                bmpScreenshot.Save(myMemoryStream, ImageFormat.Bmp);
                myMemoryStream.Position = 0;
                pixBufScreenshot = new Gdk.Pixbuf(myMemoryStream);

                Gtk.Clipboard clipboard = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));
                clipboard.Image = pixBufScreenshot;
#endif
            }
            else
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

//#if DEBUG
//                using (StreamWriter sw = new StreamWriter("log.log"))
//                {
//                    sw.WriteLine(saveDir);
//                }
//#endif

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
            if (isCropping || e.Button != MouseButtons.Left)
                return;

            StartCropping(Settings.CopyToClipboard);

        }
		private void StartCropping(bool toClipboard)
		{
            isCropping = true;

            CropForms.Clear();

            foreach (Screen screen in Screen.AllScreens)
            {
                CropForm tempCropForm = new CropForm(this, toClipboard, screen);
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
            Settings.EnableHotkeys = checkBoxHotkey.Checked;
            Settings.StartupViaRegistry = RegisterInStartup(checkBoxRegistry.Checked);
            Settings.StartupViaStartMenu = ShortcutInStartup(checkBoxStartMenu.Checked);

            SaveAppSetting();

            Minimize();

#if Windows
            DisableHotkeys();
            if (Settings.EnableHotkeys)
                EnableHotkeys();
#endif
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
