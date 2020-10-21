using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mouser.Utils;
using Point = System.Drawing.Point;

namespace Mouser
{
    /// <summary>
    /// MaskLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MaskLayer : Window
    {
        private readonly double _sx;
        private readonly double _sy;
        private readonly DoubleAnimation _animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)), FillBehavior.HoldEnd)
        {
            EasingFunction = new QuinticEase()
            {
                EasingMode = EasingMode.EaseOut
            }
        };

        public MaskLayer()
        {
            InitializeComponent();

            Util.SystemDpi(out var x, out var y);
            _sy = Util.Scaling(y);
            _sx = Util.Scaling(x);
        }

        /// <summary>
        /// 执行点击效果
        /// </summary>
        /// <param name="element"></param>
        /// <param name="position"></param>
        public void ShowClick(FrameworkElement element, Point position)
        {
            var top  = position.Y / _sy - element.Height / 2 - SystemParameters.VirtualScreenTop;
            var left = position.X / _sx - element.Width / 2 - SystemParameters.VirtualScreenLeft;
            Canvas.SetTop(element, top);
            Canvas.SetLeft(element, left);

            element.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, _animation);
            element.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _animation);
        }
    }
}
