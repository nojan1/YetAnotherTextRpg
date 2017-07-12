using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Game
{
    public static class SceneParser
    {
        private const string SCENE_FOLDER = "Resources/Scenes";

        private static readonly Dictionary<string, Action<string, Scene>> _sectionParsingFunctions = new Dictionary<string, Action<string, Scene>>
        {
            { "TEXT",  ParseText},
            { "ITEMS",  ParseItems},
            { "EXITS",  ParseExits},
        };

        public static Scene GetScene(string name)
        {
            var filename = Path.Combine(SCENE_FOLDER, $"{name}.scene");

            var scene = new Scene() { Name = name };
            var sectionNameRegex = new Regex(@"\[(.*?)\]");
            string currentSection = null;
            var sectionContent = new StringBuilder();

            foreach(var line in File.ReadAllLines(filename))
            {
                var sectionNameMatch = sectionNameRegex.Match(line);
                if (sectionNameMatch.Success)
                {
                    if (!string.IsNullOrEmpty(currentSection))
                    {
                        if (_sectionParsingFunctions.ContainsKey(currentSection))
                        {
                            _sectionParsingFunctions[currentSection](sectionContent.ToString(), scene);
                        }

                        sectionContent.Clear();
                    }

                    currentSection = sectionNameMatch.Groups[1].Value;
                }
                else if(!string.IsNullOrEmpty(currentSection))
                {
                    sectionContent.AppendLine(line);
                }
            }

            return scene;
        }

        private static void ParseText(string content, Scene scene)
        {
            scene.Text = content;
        }

        private static void ParseItems(string content, Scene scene)
        {

        }

        private static void ParseExits(string content, Scene scene)
        {
            var exitLines = Regex.Split(content, @"\((.*)\)").Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

            foreach(var line in exitLines)
            {
                var parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                if (parts.Length < 2)
                    continue;

                if (!Enum.TryParse<Direction>(parts[0], out Direction direction))
                    continue;

                scene.Exits.Add(new Exit
                {
                    Direction = direction,
                    To = parts[1],
                    Conditional = parts.Length == 3 ? parts[2] : null
                });
            }
        }
    }
}
