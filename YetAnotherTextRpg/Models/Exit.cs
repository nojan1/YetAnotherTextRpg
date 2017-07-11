using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    public class Exit
    {
        public Direction Direction { get; set; }
        public string To { get; set; }
        public string Conditional { get; set; }
    }
}
