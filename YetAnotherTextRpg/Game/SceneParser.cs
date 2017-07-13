using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Game
{
    public static class SceneParser
    {
        private const string SCENE_FOLDER = "Resources/Scenes";

        public static Scene GetScene(string name)
        {
            var filename = Path.Combine(SCENE_FOLDER, $"{name}.xml");

            var root = new XmlRootAttribute("Scene");
            var serializer = new XmlSerializer(typeof(Scene), root);

            using(var stream = File.OpenRead(filename))
            {
                Scene scene = (Scene)serializer.Deserialize(stream);
                scene.Name = name;

                scene.Text = scene.Text.Replace("\n", Environment.NewLine); //TODO: Remove me please

                foreach(var pickup in scene.Pickups)
                {
                    if (!string.IsNullOrEmpty(pickup.ItemId))
                    {
                        pickup.Item = ItemParser.ParseItem(pickup.ItemId);
                    }
                }

                return scene;
            }
        }
    }
}
