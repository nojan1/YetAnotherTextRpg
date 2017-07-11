using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Equipped { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
