using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    public class CommandParseResult
    {
        public bool Continue { get; set; }
        public bool Succeeded { get; set; }
        public string Output { get; set; } = "";
    }
}
