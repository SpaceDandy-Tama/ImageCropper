using System;
using System.Windows.Forms;

namespace ArdaCropper.KeyboardHook
{
    /// <summary>
    /// Represents the window that is used internally to get the messages.
    /// </summary>
    public class Window : NativeWindow, IDisposable
    {
        private static int WM_HOTKEY = 0x0312;

        public Window()
        {
            // create the handle for the window.
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// Overridden to get the notifications.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // check if we got a hot key pressed.
            if (m.Msg == WM_HOTKEY)
            {
                // get the keys.
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                // invoke the event to notify the parent.
                if (modifier == ModifierKeys.Shift)
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                else if (modifier == (ModifierKeys.Control | ModifierKeys.Shift))
                    KeyPressedClipboard?.Invoke(this, new KeyPressedEventArgs(modifier, key));
            }
        }

        public delegate void HotkeyEventHandler(object sender, KeyPressedEventArgs args);
        public event HotkeyEventHandler KeyPressed;
        public event HotkeyEventHandler KeyPressedClipboard;

        #region IDisposable Members

        private bool _disposed;
        public void Dispose()
        {
            if (_disposed)
                return;

            this.KeyPressed = null;
            this.KeyPressedClipboard = null;
            this.DestroyHandle();

            _disposed = true;
        }

        #endregion
    }
}
