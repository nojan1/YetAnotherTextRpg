using Cuit.Control;
using Cuit.Control.Behaviors;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YetAnotherTextRpg.Managers;

namespace YetAnotherTextRpg.Forms
{
    enum MenuAction
    {
        New,
        Resume,
        Save,
        Load,
        Exit
    }

    class MenuForm : SingleFocusControlFormScreen
    {
        private SaveManager _saveManager = new SaveManager();
        private Listbox<MenuAction> _menuList;

        public override void InstantiateComponents()
        {
            _menuList = new Listbox<MenuAction>((Application.Width / 2) - 15, 11);
            _menuList.Width = 30;
            //_menuList.Height = 8;
            _menuList.SelectionChanged += _menuList_SelectionChanged;
            Controls.Add(_menuList);

            var title = "Welcome to YetAnotherTextRPG";
            var titleLabel = new Label((Application.Width / 2) - (title.Length / 2), 8);
            titleLabel.Text = title;
            titleLabel.Foreground = ConsoleColor.Green;
            Controls.Add(titleLabel);
        }

        private void _menuList_SelectionChanged(object sender, MenuAction e)
        {
            switch (e)
            {
                case MenuAction.New:
                case MenuAction.Resume:
                    if(e == MenuAction.New)
                    {
                        GameManager.Instance.NewGame();
                    }

                    Application.SwitchTo<GameScreen>();

                    break;
                case MenuAction.Save:
                    var saves = _saveManager.ListSaves();
                    _saveManager.SaveGame(saves.Any() ? saves.Max(s => s.Slot) + 1 : 1);
                    Application.SwitchTo<GameScreen>();
                    break;
                case MenuAction.Load:
                    Application.SwitchTo<LoadForm>();
                    break;
                case MenuAction.Exit:
                    Application.Quit = true;
                    break;
            }

            _menuList.ClearSelection();
        }

        public override void OnGotFocus()
        {
            base.OnGotFocus();

            _menuList.Items.Clear();
            if (GameManager.Instance.HasLoadedState)
            {
                _menuList.Items.Add(MenuAction.Resume);
                _menuList.Items.Add(MenuAction.Save);
            }
            else
            {
                _menuList.Items.Add(MenuAction.New);
            }

            _menuList.Items.Add(MenuAction.Load);
            _menuList.Items.Add(MenuAction.Exit);

        }
    }
}
