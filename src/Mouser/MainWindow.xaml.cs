using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using Mouser.Models;
using Mouser.Utils;
using Mouser.Utils.Win32;
using Action = Mouser.Models.Action;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Mouser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MaskLayer _mask = new MaskLayer() { Topmost = true };
        private readonly MouseHook _mouseHook = new MouseHook();
        private Record _currentRecord;



        #region .ctor

        public MainWindow()
        {
            InitializeComponent();

            _mouseHook.OnMouseActivity += OnMouseActivity;
            this.Closing += OnClosing;

            _mask.Show();
        }

        private void OnClosing(object sender, CancelEventArgs args)
        {
            _mouseHook.Stop();
            _mask.Close();
            _stopPlaying = true;
        }

        #endregion



        #region 全局快捷键

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var handle = new WindowInteropHelper(this).Handle;
            HotKeyManager.RegisterHotKey(handle, HotKeyManager.WM_HOTKEY_START, HotKeyManager.MOD_ALT, HotKeyManager.VK_F11);
            HotKeyManager.RegisterHotKey(handle, HotKeyManager.WM_HOTKEY_STOP,  HotKeyManager.MOD_ALT, HotKeyManager.VK_F12);

            // HwndSource source = HwndSource.FromHwnd(handle);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            Debug.WriteLine($"hwnd:{hwnd},msg:{msg},wParam:{wParam},lParam{lParam}:,handle:{handle}");

            if (msg == HotKeyManager.WM_HOTKEY)
            {
                switch (wParam.ToInt32())
                {
                    case HotKeyManager.WM_HOTKEY_START:

                        break;
                    case HotKeyManager.WM_HOTKEY_STOP:
                        StopPlaying();
                        break;
                }
            }
            
            return IntPtr.Zero;
        }

        #endregion



        #region 录制

        private DateTime _last;
        private DateTime _lastExceptMove;

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            _currentRecord = new Record()
            {
                BeginTime = _last = DateTime.Now,
            };
            _mouseHook.Start();
        }

        private void BtnEnd_OnClick(object sender, RoutedEventArgs e)
        {
            _mouseHook.Stop();
            var json = JsonUtil.SerializeByNsj(_currentRecord);
            File.WriteAllText($"./{DateTime.Now:yyyy_MM_dd HHmmss}_record.json", json);
        }

        private void OnMouseActivity(object sender, MouseEventArgs args)
        {
            Action action = new Action()
            {
                MouseButton = args.Button,
                Point       = args.Location,
                ActionType =  args.Clicks,
                Duration    = DateTime.Now - _last,
            };

            if (action.ActionType == ActionTypes.Move && action.Duration.TotalMilliseconds < 50)
            {
                return;
            }

            _last = DateTime.Now;
            _currentRecord.ActionList.Add(action);
            TbkInfo.Text = $"{_lastExceptMove = DateTime.Now:HH:mm:ss fff} {args.Clicks}, {args.X}, {args.Y}";
        }

        #endregion



        #region 播放

        private bool _stopPlaying = false;
        
        private async void BtnPlay_OnClick(object sender, RoutedEventArgs e)
        {
            _stopPlaying  = false;
            _mask.Topmost = false;
            _mask.Topmost = true;

            while (_stopPlaying == false)
            {
                foreach (var t in _currentRecord.ActionList)
                {
                    if (_stopPlaying) return;
                    
                    await Util.DelayAsync((int)t.Duration.TotalMilliseconds);
                    
                    if (_stopPlaying) return;

                    // 模拟移动
                    MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_ABSOLUTE | MouseSimulator.MOUSEEVENTF_MOVE, t.Point.X * 65536 / 3840, t.Point.Y * 65536 / 2160, 0, 0);
                    
                    switch (t.ActionType)
                    {
                        case ActionTypes.Up:
                            if (t.MouseButton == MouseButtons.Left)
                            {
                                MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                _mask.ShowClick(_mask.EffectLeftUp, t.Point);
                            }
                            else if (t.MouseButton == MouseButtons.Right)
                            {
                                MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                                _mask.ShowClick(_mask.EffectLeftUp, t.Point);
                            }
                            else if (t.MouseButton == MouseButtons.Middle)
                            {
                                MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
                                _mask.ShowClick(_mask.EffectLeftUp, t.Point);
                            }
                            break;
                        case ActionTypes.Down:
                            if (t.MouseButton == MouseButtons.Left)
                            {
                                MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                _mask.ShowClick(_mask.EffectLeftDown, t.Point);
                            }
                            else if (t.MouseButton == MouseButtons.Right)
                            {
                                MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                                _mask.ShowClick(_mask.EffectLeftDown, t.Point);
                            }
                            else if (t.MouseButton == MouseButtons.Middle)
                            {
                                MouseSimulator.mouse_event(MouseSimulator.MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
                                _mask.ShowClick(_mask.EffectLeftDown, t.Point);
                            }
                            break;
                        case ActionTypes.Click:
                        case ActionTypes.DClick:
                            // TODO 暂未处理
                            break;
                    }
                }
            }
        }

        private void StopPlaying()
        {
            _stopPlaying = true;
            Canvas.SetLeft(_mask.EffectLeftUp, -10000);
            Canvas.SetLeft(_mask.EffectLeftDown, -10000);
        }

        #endregion
    }
}
