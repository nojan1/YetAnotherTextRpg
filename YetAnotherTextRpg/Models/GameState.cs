using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Models
{
    class GameState
    {
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public List<Item> Inventory { get; set; } = new List<Item>();
        public string CurrentScene { get; set; }

        public bool IsPickupPickedUp(string id)
        {
            return Variables.ContainsKey($"PICKUP-{id}");
        }

        public bool HasEnoughMoney(int requiredAmount)
        {
            return GetVariableValue<int>("money") >= requiredAmount;
        }

        public T GetVariableValue<T>(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return (T)Convert.ChangeType(Variables[name], typeof(T));
            }
            else
            {
                return default(T);
            }
        }

        public GameState()
        {
            Inventory.Add(new MoneyItem());
        }
    }
}
