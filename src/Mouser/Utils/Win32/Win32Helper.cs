using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Standard;
using Point = System.Drawing.Point;

namespace Mouser.Utils.Win32
{
    public static class Win32Helper
    {
        #region 屏幕坐标计算

        // 归一化坐标计算参考 https://stackoverflow.com/questions/28392072/how-to-automate-mouse-actions

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32")]
        private static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

        /// <summary>
        /// Convert px to dx. If px is invalid, return null.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point? TransformPxToDx(Point point)
        {
            GetClientRect(GetDesktopWindow(), out RECT rect);

            var x = (int)((double)65536 / rect.Right * point.X);
            var y = (int)((double)65536 / rect.Bottom * point.Y);
            return new Point(x, y);
        }

        #endregion




        #region 屏幕透明

        ///win32 api
        private const int WS_EX_TRANSPARENT = 0x20;

        private const int GWL_EXSTYLE = (-20);

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 设置窗体透明可穿透
        /// </summary>
        /// <param name="win"></param>
        public static void SetWindowTransparent(Window win)
        {
            IntPtr hwnd = new WindowInteropHelper(win).Handle;
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        /// <summary>
        /// 设置窗体透明可穿透
        /// </summary>
        /// <param name="handle"></param>
        public static void SetVisualTransparent(IntPtr handle)
        {
            var extendedStyle = GetWindowLong(handle, GWL_EXSTYLE);
            SetWindowLong(handle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        #endregion
    }
}
