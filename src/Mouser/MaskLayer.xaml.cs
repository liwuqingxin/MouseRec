using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private readonly DoubleAnimation _animation = new DoubleAnimation(0.3, 1, new Duration(TimeSpan.FromMilliseconds(500)), FillBehavior.HoldEnd)
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
            Top = position.Y / _sy - this.Height / 2;
            Left = position.X / _sx - this.Width / 2;

            element.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, _animation);
            element.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _animation);
        }

        private void ClickIndicator_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("");
        }
    }
}
