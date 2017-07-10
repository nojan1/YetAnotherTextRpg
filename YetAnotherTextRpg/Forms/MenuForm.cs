using Cuit.Control;
using Cuit.Control.Behaviors;
using Cuit.Screen;
using System;
using System.Collections.Generic;
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

    class MenuForm : FormScreen
    {
        private Listbox<MenuAction> _menuList;

        public override void InstantiateComponents()
        {
            var title = "Welcome to YetAnotherTextRPG";
            var titleLabel = new Label((Application.Width / 2) - (title.Length / 2), 8);
            titleLabel.Text = title;
            titleLabel.Foreground = ConsoleColor.Green;
            Controls.Add(titleLabel);

            _menuList = new Listbox<MenuAction>((Application.Width / 2) - 15, 12);
            _menuList.Width = 30;
            _menuList.Height = 5;
            _menuList.SelectionChanged += _menuList_SelectionChanged;
            Controls.Add(_menuList);
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

                    break;
                case MenuAction.Load:

                    break;
                case MenuAction.Exit:
                    Application.Quit = true;
                    break;
            }

            _menuList.ClearSelection();
        }

        public override void OnLoaded()
        {
            base.OnLoaded();

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
