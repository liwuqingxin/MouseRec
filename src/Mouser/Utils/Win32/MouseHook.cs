using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
// ReSharper disable IdentifierTypo
#pragma warning disable 649

namespace Mouser.Utils.Win32
{
    public static class ActionTypes
    {
        public const int Move   = 0;
        public const int Down   = 1;
        public const int Up     = 2;
        public const int Click  = 3;
        public const int DClick = 4;
    }

    /// <summary>
    /// 鼠标全局钩子
    /// </summary>
    public class MouseHook
    {
        // https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondown
        private const int WM_MOUSEMOVE     = 0x200;
        private const int WM_LBUTTONDOWN   = 0x201;
        private const int WM_LBUTTONUP     = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN   = 0x204;
        private const int WM_RBUTTONUP     = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN   = 0x207;
        private const int WM_MBUTTONUP     = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;

        private const int WH_MOUSE_LL = 14; // mouse hook constant

        /// <summary>
        /// 点
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            public int x;
            public int y;
        }

        /// <summary>
        /// 钩子结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class MouseHookStruct
        {
            public POINT pt;
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        /// <summary>
        /// 声明鼠标钩子事件类型
        /// </summary>
        private HookProc _mouseHookProcedure;

        /// <summary>
        /// 鼠标钩子句柄
        /// </summary>
        private static int _hMouseHook = 0;

        /// <summary>
        /// 全局的鼠标事件
        /// </summary>
        public event MouseEventHandler OnMouseActivity;


        /// <summary>
        /// 装置钩子的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 卸下钩子的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 下一个钩挂的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// 钩子回调函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);


        #region .ctor

        /// <summary>
        /// 构造函数
        /// </summary>
        public MouseHook()
        {

        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MouseHook()
        {
            Stop();
        }

        #endregion

        /// <summary>
        /// 启动全局钩子
        /// </summary>
        public void Start()
        {
            // 安装鼠标钩子
            if (_hMouseHook == 0)
            {
                // 生成一个HookProc的实例.
                _mouseHookProcedure = new HookProc(MouseHookProc);

                _hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseHookProcedure, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]), 0);

                //假设装置失败停止钩子
                if (_hMouseHook == 0)
                {
                    Stop();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
        }

        /// <summary>
        /// 停止全局钩子
        /// </summary>
        public void Stop()
        {
            bool retMouse = true;

            if (_hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(_hMouseHook);
                _hMouseHook = 0;
            }

            // 假设卸下钩子失败
            if (!(retMouse))
                throw new Exception("UnhookWindowsHookEx failed.");
        }

        /// <summary>
        /// 鼠标钩子回调函数
        /// </summary>
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 假设正常执行而且用户要监听鼠标的消息
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                MouseButtons button = MouseButtons.None;
                var clickCount = 0;

                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        clickCount = ActionTypes.Down;
                        button     = MouseButtons.Left;
                        break;
                    case WM_LBUTTONUP:
                        clickCount = ActionTypes.Up;
                        button     = MouseButtons.Left;
                        break;
                    case WM_LBUTTONDBLCLK:
                        clickCount = ActionTypes.DClick;
                        button     = MouseButtons.Left;
                        break;

                    case WM_RBUTTONDOWN:
                        clickCount = ActionTypes.Down;
                        button     = MouseButtons.Right;
                        break;
                    case WM_RBUTTONUP:
                        clickCount = ActionTypes.Up;
                        button     = MouseButtons.Right;
                        break;
                    case WM_RBUTTONDBLCLK:
                        clickCount = ActionTypes.DClick;
                        button     = MouseButtons.Right;
                        break;

                    case WM_MBUTTONDOWN:
                        clickCount = ActionTypes.Down;
                        button     = MouseButtons.Middle;
                        break;
                    case WM_MBUTTONUP:
                        clickCount = ActionTypes.Up;
                        button     = MouseButtons.Middle;
                        break;
                    case WM_MBUTTONDBLCLK:
                        clickCount = ActionTypes.DClick;
                        button     = MouseButtons.Middle;
                        break;

                    case WM_MOUSEMOVE:
                        clickCount = ActionTypes.Move;
                        button     = MouseButtons.None;
                        break;
                }

                // 从回调函数中得到鼠标的信息
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                
                // TODO delta 暂未处理
                MouseEventArgs e = new MouseEventArgs(button, clickCount, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y, 0);

                // 假设想要限制鼠标在屏幕中的移动区域能够在此处设置
                // 后期须要考虑实际的x、y的容差
                if (!Screen.PrimaryScreen.Bounds.Contains(e.X, e.Y))
                {
                    //return 1;
                }

                OnMouseActivity(this, e);
            }

            // 启动下一次钩子
            return CallNextHookEx(_hMouseHook, nCode, wParam, lParam);
        }
    }
}
