using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Managers;

namespace YetAnotherTextRpg.Models
{
    public class Item
    {
        public string Id { get; set; }
        public virtual string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Equipped { get; set; }
        public bool CanEquip { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class MoneyItem : Item
    {
        public override string Name { get => $"Money: {Amount}"; }

        public int Amount
        {
            get
            {
                if (GameManager.Instance.State.Variables.ContainsKey("money"))
                {
                    return Convert.ToInt32(GameManager.Instance.State.Variables["money"]);
                }
                else
                {
                    return 0;
                }
            }
        }

        public MoneyItem()
        {
            CanEquip = false;
            Description = "Money money. We all need it to buy stuff";
            ImagePath = "money.asc";
        }
    }
}
