using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Forms
{
    class InventoryForm : SingleFocusControlFormScreen
    {
        private Listbox<Item> itemsBox;
        private Label itemInfo;

        public override void InstantiateComponents()
        {
            itemsBox = new Listbox<Item>(5, 4);
            itemsBox.Autoselect = false;
            itemsBox.Multiselect = true;
            itemsBox.Width = 30;
            itemsBox.Height = 15;
            Controls.Add(itemsBox);

            itemInfo = new Label(40, 4);
            itemInfo.Width = 40;
            itemInfo.IsMultiline = true;
            Controls.Add(itemInfo);

            Controls.Add(new Label(5, 2) { Text = "Inventory - Press <ESC> to go back", Foreground = ConsoleColor.Gray });

            itemsBox.SelectionChanged += ItemsBox_SelectionChanged;
            itemsBox.PreviewChanged += ItemsBox_PreviewChanged;
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

        private void ItemsBox_PreviewChanged(object sender, Item e)
        {
            itemInfo.Text = $"{e.Name}{Environment.NewLine}------------------{Environment.NewLine}{e.Description}";
        }

        private void ItemsBox_SelectionChanged(object sender, Item e)
        {
            if (e == null)
                return;

            if (!e.CanEquip)
                itemsBox.SetSelection(e, false);

            GameManager.Instance.State.Inventory.ForEach(i => i.Equipped = false);
            foreach (var selected in itemsBox.Selected)
            {
                selected.Equipped = true;
            }
        }

        public override void OnLoaded()
        {
            base.OnLoaded();

            itemsBox.Items.AddRange(GameManager.Instance.State.Inventory);

            GameManager.Instance.State.Inventory
                .Where(i => i.Equipped)
                .ToList()
                .ForEach(i => itemsBox.SetSelection(i, true));
        }
    }
}
