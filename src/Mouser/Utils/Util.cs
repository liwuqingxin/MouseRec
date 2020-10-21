using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouser.Utils
{
    public static class Util
    {
        /// <summary>
        /// 异步延时
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static async Task DelayAsync(int ms)
        {
            await Task.Delay(ms);
        }

        /// <summary>
        /// 获取当前系统的dpi数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SystemDpi(out int x, out int y)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                x = (int)g.DpiX;
                y = (int)g.DpiY;
                g.Dispose();
            }
        }

        /// <summary>
        /// 根据当前系统dpi数值匹配 当前系统的桌面缩放比例
        /// </summary>
        /// <param name="DpiIndex"></param>
        /// <returns></returns>
        public static double Scaling(int DpiIndex)
        {
            switch (DpiIndex)
            {
                case 96:  return 1;
                case 120: return 1.25;
                case 144: return 1.5;
                case 168: return 1.75;
                case 192: return 2;
            }
            return 1;
        }
    }
}
