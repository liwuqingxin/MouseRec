using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouser.Models
{
    public class Record
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Action> ActionList { get; set; }

        public Record()
        {
            ActionList = new List<Action>();
        }
    }
}
