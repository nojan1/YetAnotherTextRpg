using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    class GameState
    {
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public List<Item> Inventory = new List<Item>();
        public string CurrentScene { get; set; }
    }
}
