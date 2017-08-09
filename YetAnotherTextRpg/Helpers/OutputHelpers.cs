using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using YetAnotherTextRpg.Game;

namespace YetAnotherTextRpg.Helpers
{
    public static class OutputHelpers
    {
        public static string ProcessOutput(string text)
        {
            var blocks = Regex.Matches(text, @"{(.*?) (.*?)}(([^(\r\n|\r|\n)]*(\r\n|\r|\n)+)+){\/\1}", RegexOptions.Multiline);
            foreach (Match block in blocks)
            {
                if (block.Groups.Count >= 4)
                {
                    var blockType = block.Groups[1].Value.ToLower();
                    var expression = block.Groups[2].Value;
                    var body = block.Groups[3].Value;

                    if (blockType == "if")
                    {
                        var testResult = EmbeddedFunctionsHelper.Conditional(expression).Success;
                        var bodyParts = body.Split(new string[] { "{else}" }, StringSplitOptions.None);

                        if (testResult && bodyParts.Length >= 1)
                        {
                            text = text.Replace(block.Value, bodyParts[0].Trim());
                        }
                        else if (!testResult && bodyParts.Length == 2)
                        {
                            text = text.Replace(block.Value, bodyParts[1].Trim());
                        }
                        else
                        {
                            text = text.Replace(block.Value, "");
                        }
                    }
                }
            }

            return text;
        }
    }
}
