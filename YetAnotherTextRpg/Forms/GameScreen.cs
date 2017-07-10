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
    class GameScreen : IScreen, ILoaded
    {
        public CuitApplication Application { get; set; }
        public event EventHandler Loaded = delegate { };

        private readonly Textbox command;
        private readonly OutputBox output;

        public GameScreen()
        {
            command = new Textbox(5, 2);
            command.Width = Console.BufferWidth - 10;

            output = new OutputBox(6, 6);
            output.Width = Console.BufferWidth - 12;
        }

        public void HandleKeypress(ConsoleKeyInfo key)
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
                    command.HandleKeypress(key);
                    break;
            }
        }

        public void Update(Screenbuffer buffer, bool force)
        {
            if(command.IsDirty || force) command.Draw(buffer);
            if(output.IsDirty || force) output.Draw(buffer);

            command.IsDirty = false; output.IsDirty = false;
        }

        public void OnLoaded()
        {
            Loaded(this, new EventArgs());

            command.OnGotFocus();
        }

        private bool HandleCommand(string command)
        {
            output.AddOutput(command);

            return true;
        }
    }
}
