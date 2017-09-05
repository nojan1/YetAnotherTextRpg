using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Game
{
    public static class EncounterParser
    {
        private const string ENCOUNTER_FOLDER = "Resources/Encounters";

        public static Encounter GetEncounter(string name)
        {
            var filename = Path.Combine(ENCOUNTER_FOLDER, $"{name}.xml");

            var root = new XmlRootAttribute("Encounter");
            var serializer = new XmlSerializer(typeof(Encounter), root);

            using (var stream = File.OpenRead(filename))
            {
                Encounter encounter = (Encounter)serializer.Deserialize(stream);

                foreach(var enemy in encounter.Enemies)
                {
                    enemy.ImagePath = Path.Combine(ENCOUNTER_FOLDER, "Images", enemy.ImagePath);
                }

                return encounter;
            }
        }
    }
}
