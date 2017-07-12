using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Game
{
    public static class ItemParser
    {
        private const string ITEM_FOLDER = "Resources/Items";

        public static Item ParseItem(string itemId)
        {
            var filename = Path.Combine(ITEM_FOLDER, $"{itemId}.json");

            var itemJson = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<Item>(itemJson);
        }
    }
}
