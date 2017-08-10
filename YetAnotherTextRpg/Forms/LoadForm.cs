using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Forms
{
    public class LoadForm : SingleFocusControlFormScreen
    {
        private Listbox<SaveGameListing> _savesBox;
        private readonly SaveManager _saveManager = new SaveManager();

        public override void InstantiateComponents()
        {
            base.InstantiateComponents();

            _savesBox = new Listbox<SaveGameListing>(5, 4);
            _savesBox.Width = Application.Width - 8; ;
            _savesBox.Height = 20;
            Controls.Add(_savesBox);

            Controls.Add(new Label(5, 2) { Text = "Saved games, pick game to load - Press <ESC> to go back", Foreground = ConsoleColor.Gray });

            _savesBox.SelectionChanged += _savesBox_SelectionChanged;
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            _savesBox.Items.AddRange(_saveManager.ListSaves());
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                Application.GoBack();
            }
            else
            {
                base.HandleKeypress(key);
            }
        }

        private void _savesBox_SelectionChanged(object sender, SaveGameListing e)
        {
            Application.GoBack();

            _saveManager.LoadGame(e.Slot);
            Application.SwitchTo<GameScreen>();
        }
    }
}
