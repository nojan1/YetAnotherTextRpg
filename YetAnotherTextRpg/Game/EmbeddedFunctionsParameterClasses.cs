using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YetAnotherTextRpg.Managers;

namespace YetAnotherTextRpg.Game
{
    public class InventoryParameterHelper
    {
        public bool Contains(string id)
        {
            return GameManager.Instance.State.Inventory.Any(i => i.Id == id);
        }
    }

    public class VariablesParameterHelper
    {
        public string Get(string key)
        {
            if (GameManager.Instance.State.Variables.ContainsKey(key))
            {
                return GameManager.Instance.State.Variables[key];
            }
            else
            {
                return null;
            }
        }

        public void Set(string key, string value)
        {
            GameManager.Instance.State.Variables[key] = value;
        }
    }

    public class PickupsParameterHelper
    {
        public bool InScene(string id)
        {
            return GameManager.Instance.State.IsPickupPickedUp(id);
        }
    }
}
