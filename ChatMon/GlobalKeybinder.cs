using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ChatMon
{
    class GlobalKeybinder
    {
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int MYACTION_HOTKEY_ID = 1;

        static HwndSource source;

        public static void Register(Window window, int modifier, int key)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);
            UnregisterHotKey(handle, MYACTION_HOTKEY_ID);
            if(key != 0) { 
                RegisterHotKey(handle, MYACTION_HOTKEY_ID, modifier, key);
            }
        }

        public delegate void OnShutUpDelegate();
        public static event OnShutUpDelegate OnShutUp;

        public static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    OnShutUp?.Invoke();
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
