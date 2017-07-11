using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    public class Scene
    {
        public string Name { get; set; }
        public ICollection<Exit> Exits { get; set; } = new List<Exit>();
        public string Text { get; set; }


    }
}
