// ReSharper disable IdentifierTypo
namespace Mouser.Utils.Win32
{
    public static class MouseSimulator
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_MOVE       = 0x0001;
        public const int MOUSEEVENTF_LEFTDOWN   = 0x0002;
        public const int MOUSEEVENTF_LEFTUP     = 0x0004;
        public const int MOUSEEVENTF_RIGHTDOWN  = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP    = 0x0010;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const int MOUSEEVENTF_MIDDLEUP   = 0x0040;
        public const int MOUSEEVENTF_ABSOLUTE   = 0x8000;
    }
}
