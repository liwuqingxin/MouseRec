using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;


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

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string OpenFile(out string fileName)
        {
            fileName = null;

            var openFile = new OpenFileDialog { Filter = "Files (*.json)|*.json|All Files (*.*)|*.*" };

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    fileName = openFile.FileName;
                    return File.ReadAllText(openFile.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return null;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="json"></param>
        public static void SaveFileAs(string json)
        {
            var saveFile = new SaveFileDialog { Filter = "Files (*.json)|*.json|All Files (*.*)|*.*" };
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFile.FileName, json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
