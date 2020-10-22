using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Standard;

namespace Mouser.Utils
{
    /// <summary>
    /// 屏幕坐标工具。
    /// </summary>
    /// 归一化坐标计算参考 https://stackoverflow.com/questions/28392072/how-to-automate-mouse-actions
    public static class ScreenUtil
    {
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32")]
        internal static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

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
    }
}
