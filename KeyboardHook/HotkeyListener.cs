using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageCropper.KeyboardHook
{
	public class HotkeyListener : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private Window _window = new Window();
        private int _currentId;

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key, Window.HotkeyEventHandler callback)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");

            if (modifier == ModifierKeys.Shift)
                _window.KeyPressed += callback;
            else if (modifier == (ModifierKeys.Control | ModifierKeys.Shift))
                _window.KeyPressedClipboard += callback;
        }

#region IDisposable Members

        private bool _disposed;
        public void Dispose()
        {
            if (_disposed)
                return;

            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();

            _disposed = true;
        }

#endregion
    }
}