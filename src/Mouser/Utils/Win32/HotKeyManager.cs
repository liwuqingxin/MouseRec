using System;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace Mouser.Utils.Win32
{
    /// <summary>
    /// 热键管理器。
    /// </summary>
    /// https://www.cnblogs.com/atskyline/archive/2012/09/20/2694878.html
    /// https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-registerhotkey?redirectedfrom=MSDN
    public static class HotKeyManager
    {
        // The keys that must be pressed in combination with the key specified by
        // the uVirtKey parameter in order to generate the WM_HOTKEY message.
        // The fsModifiers parameter can be a combination of the following values.
        public const int MOD_ALT      = 0x0001;
        public const int MOD_CONTROL  = 0x0002;
        public const int MOD_NOREPEAT = 0x4000;
        public const int MOD_SHIFT    = 0x0004;
        public const int MOD_WIN      = 0x0008;

        // Id of our hot keys.
        public const int WM_HOTKEY_START = 0x0001;
        public const int WM_HOTKEY_STOP  = 0x0002;
        public const int WM_HOTKEY_RECORD_START = 0x0003;
        public const int WM_HOTKEY_RECORD_STOP = 0x0004;

        // virtual-key
        // https://docs.microsoft.com/zh-cn/windows/win32/inputdev/virtual-key-codes?redirectedfrom=MSDN
        public const int VK_F9  = 0x78;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x7A;
        public const int VK_F12 = 0x7B;

        // hot-key message key
        public const int WM_HOTKEY = 0x0312;

        /// <summary>
        /// 注册热键
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        /// <summary>
        /// 注销热键
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
