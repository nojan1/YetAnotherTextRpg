using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    public class SaveGameListing
    {
        public int Slot { get; set; }
        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{Slot} - {Created}";
        }
    }
}
