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

        public int Money
        {
            get => GetVariableValue<int>("money");
            set => Variables["money"] = value.ToString();
        }

        public int MaxHealth
        {
            get => GetVariableValue<int>("maxhealth");
            set => Variables["maxhealth"] = value.ToString();
        }

        public int Health
        {
            get => GetVariableValue<int>("health");
            set => Variables["health"] = value.ToString();
        }

        public bool IsPickupPickedUp(string id)
        {
            return Variables.ContainsKey($"PICKUP-{id}");
        }

        public bool HasEnoughMoney(int requiredAmount)
        {
            return Money >= requiredAmount;
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

        public CombatProfile GetPlayerCombatProfile()
        {
            return new CombatProfile
            {
                Armor = GetVariableValue<int>("armor"),
                Attack = GetVariableValue<int>("attack"),
                Defense = GetVariableValue<int>("defense"),
                Strength = GetVariableValue<int>("strength"),
            };
        }
    }
}
