using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mouser.Models
{
    public class Action
    {
        public TimeSpan Duration { get; set; }
        public Point Point { get; set; }
        public MouseButtons MouseButton { get; set; }
        public int ActionType { get; set; }

    }
}
