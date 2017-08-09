﻿using System;
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

        public bool Add(string itemId)
        {
            var item = ItemParser.ParseItem(itemId);
            GameManager.Instance.State.Inventory.Add(item);

            return true;
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
            GameManager.Instance.State.Variables[key] = value;
            return true;
        }

        public bool IsSet(string key)
        {
            return Get(key) != null;
        }
    }

    public class PickupsParameterHelper
    {
        public bool InScene(string id)
        {
            return GameManager.Instance.ActiveScene.Pickups.Any(p => p.ItemId == id) && 
                   !GameManager.Instance.State.IsPickupPickedUp(id);
        }
    }

    public class OutputParameterHelper
    {
        public string Output { get; private set; }

        public bool Set(string output)
        {
            Output = output;
            return true;
        }
    }

    public class SceneParameterHelper
    {
        public bool SwitchTo(string sceneName)
        {
            GameManager.Instance.SwitchScene(sceneName);
            return true;
        }
    }
}
