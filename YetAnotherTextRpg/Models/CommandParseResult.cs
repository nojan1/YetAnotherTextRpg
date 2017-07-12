using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    public class CommandParseResult
    {
        public bool Succeeded { get; set; }
        public string Output { get; set; } = "";
    }
}
