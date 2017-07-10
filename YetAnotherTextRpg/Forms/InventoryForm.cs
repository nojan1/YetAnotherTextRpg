using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Forms
{
    class InventoryForm : FormScreen
    {
        private Listbox<Item> itemsBox;
        private Label itemInfo;

        public override void InstantiateComponents()
        {
            itemsBox = new Listbox<Item>(5, 2);
            itemsBox.Autoselect = true;
            itemsBox.Multiselect = false;
            itemsBox.Width = 30;
            itemsBox.Height = 15;
            Controls.Add(itemsBox);

            itemInfo = new Label(40, 2);
            itemInfo.Width = 40;
            itemInfo.IsMultiline = true;
            Controls.Add(itemInfo);

            itemsBox.SelectionChanged += ItemsBox_SelectionChanged;
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

        private void ItemsBox_SelectionChanged(object sender, Item e)
        {
            itemInfo.Text = $"{e.Name}{Environment.NewLine}------------------{Environment.NewLine}{e.Description}";
        }

        public override void OnLoaded()
        {
            base.OnLoaded();

            itemsBox.Items.AddRange(GameManager.Instance.State.Inventory);
        }
    }
}
