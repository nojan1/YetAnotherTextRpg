using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using Cuit;
using Cuit.Control.Behaviors;
using YetAnotherTextRpg.Controls;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Game;
using System.Text.RegularExpressions;

namespace YetAnotherTextRpg.Forms
{
    class GameScreen : SingleFocusControlFormScreen
    {
        private Textbox command;
        private OutputBox output;

        private CommandParser _commandParser = new CommandParser();

        public override void InstantiateComponents()
        {
            base.InstantiateComponents();

            command = new Textbox(5, 2);
            command.Width = Console.BufferWidth - 10;
            Controls.Add(command);

            output = new OutputBox(6, 6);
            output.Width = Console.BufferWidth - 12;
            Controls.Add(output);
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    Application.GoBack();
                    break;
                case ConsoleKey.Enter:
                    if (command.Text.Trim() == "inventory")
                    {
                        Application.SwitchTo<InventoryForm>();
                        command.Text = "";
                    }
                    else
                    {
                        if (HandleCommand(command.Text.Trim()))
                        {
                            command.Text = "";
                        }
                    }
                    break;
                default:
                    base.HandleKeypress(key);
                    break;
            }
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var text = GameManager.Instance.ActiveScene.Text;

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
                        var testResult = EmbeddedFunctionsHelper.Conditional(expression);
                        var bodyParts = body.Split(new string[] { "{else}" }, StringSplitOptions.None);

                        if (testResult && bodyParts.Length >= 1)
                        {
                            text = text.Replace(block.Value, bodyParts[0].Trim());
                        }
                        else if (!testResult && bodyParts.Length == 2)
                        {
                            text = text.Replace(block.Value, bodyParts[1].Trim());
                        }else
                        {
                            text = text.Replace(block.Value, "");
                        }
                    }
                }
            }

            output.Text = text;
        }

        private bool HandleCommand(string command)
        {
            string sceneBefore = GameManager.Instance.ActiveScene.Name;

            output.AddOutput(command);

            var result = _commandParser.ProcessCommand(command);
            if (!string.IsNullOrWhiteSpace(result.Output))
            {
                output.AddOutput(result.Output);
            }

            if (result.Succeeded && sceneBefore != GameManager.Instance.ActiveScene.Name)
            {
                UpdateDisplay();
            }

            return result.Succeeded;
        }
    }
}
