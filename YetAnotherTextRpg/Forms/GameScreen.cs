using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using Cuit;
using Cuit.Control.Behaviors;
using YetAnotherTextRpg.Controls;

namespace YetAnotherTextRpg.Forms
{
    class GameScreen : SingleFocusControlFormScreen
    {
        private Textbox command;
        private OutputBox output;

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
                    if (HandleCommand(command.Text.Trim()))
                    {
                        command.Text = "";
                    }

                    break;
                case ConsoleKey.I:
                    Application.SwitchTo<InventoryForm>();
                    break;
                default:
                    base.HandleKeypress(key);
                    break;
            }
        }

        private bool HandleCommand(string command)
        {
            output.AddOutput(command);

            return true;
        }
    }
}
