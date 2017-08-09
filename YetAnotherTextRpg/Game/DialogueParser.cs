using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace YetAnotherTextRpg.Game
{
    public static class DialogueParser
    {
        private const string DIALOGUE_FOLDER = "Resources/Dialogues";

        public static Dialogue GetDialogue(string name)
        {
            var filename = Path.Combine(DIALOGUE_FOLDER, $"{name}.xml");

            var root = new XmlRootAttribute("Dialogue");
            var serializer = new XmlSerializer(typeof(Dialogue), root);

            using (var stream = File.OpenRead(filename))
            {
                Dialogue dialogue = (Dialogue)serializer.Deserialize(stream);

                foreach (var screen in dialogue.Screen)
                {
                    screen.Says = screen.Says.Replace("\n", Environment.NewLine);
                }

                return dialogue;
            }
        }
    }
}
