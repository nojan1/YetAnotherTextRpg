using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Equipped { get; set; }
        public bool CanEquip { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
