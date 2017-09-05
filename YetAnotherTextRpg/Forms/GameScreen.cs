using Cuit.Control;
using Cuit.Screen;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Cuit;
using Cuit.Control.Behaviors;
using YetAnotherTextRpg.Controls;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Game;
using System.Text.RegularExpressions;
using YetAnotherTextRpg.Helpers;

namespace YetAnotherTextRpg.Forms
{
    class GameScreen : SingleFocusControlFormScreen
    {
        private Textbox command;
        private OutputBox output;

        private readonly CommandParser _commandParser = new CommandParser();
        private readonly List<string> _history = new List<string>();
        private int _historyPosition = -1;

        public override void InstantiateComponents()
        {
            base.InstantiateComponents();

            command = new Textbox(5, 2);
            command.Width = Console.BufferWidth - 10;
            RegisterControl(command);

            output = new OutputBox(6, 6);
            output.Width = Console.BufferWidth - 12;
            RegisterControl(output);
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    Application.GoBack();
                    break;

                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                    if (_history.Any())
                    {
                        if(_historyPosition == -1)
                            _history.Add(command.Text.Trim());

                        _historyPosition += key.Key == ConsoleKey.UpArrow ? 1 : -1;

                        if (_historyPosition < 0)
                            _historyPosition = _history.Count - 1;
                        else if (_historyPosition > _history.Count - 1)
                            _historyPosition = 0;

                        command.Text = _history[_historyPosition];
                    }

                    break;
                case ConsoleKey.Enter:
                    if(_history.Any())
                        _history.Remove(_history.Last());

                    _historyPosition = -1;
                    _history.Add(command.Text.Trim());

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

        public void AppendOutput(string outputText)
        {
            output.AddOutput(outputText);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            ////////
            //TODO: Kick the devs to fix Cuit....
            Console.Clear();
            output.IsDirty = true; command.IsDirty = true;
            ////////

            output.Text = OutputHelpers.ProcessOutput(GameManager.Instance.ActiveScene.Text);
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
