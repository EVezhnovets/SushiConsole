using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsole.Models
{
    public class OrderEventsArgs : EventArgs
    {
        public string Message { get; set; }
        public bool BoolArg { get; set; }

        public OrderEventsArgs(string message, bool boolArg)
        {
            Message = message;
            BoolArg = boolArg;
        }
    }
}
