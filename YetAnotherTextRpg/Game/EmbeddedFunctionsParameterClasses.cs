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

        public bool Set(string key, string value)
        {
            var existedBefore = GameManager.Instance.State.Variables.ContainsKey(key);

            GameManager.Instance.State.Variables[key] = value;

            return existedBefore;
        }
    }

    public class PickupsParameterHelper
    {
        public bool InScene(string id)
        {
            return GameManager.Instance.State.IsPickupPickedUp(id);
        }
    }

    public class OutputParameterHelper
    {
        public string Output { get; private set; }

        public bool Set(string output)
        {
            Output = output;

            return output != null;
        }
    }
}
