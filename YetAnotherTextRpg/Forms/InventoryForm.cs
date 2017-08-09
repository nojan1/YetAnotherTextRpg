using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YetAnotherTextRpg.Game;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Forms
{
    class InventoryForm : SingleFocusControlFormScreen
    {
        private Listbox<Item> itemsBox;
        private Label itemInfo;
        private Image itemImage;

        public override void InstantiateComponents()
        {
            itemsBox = new Listbox<Item>(5, 4);
            itemsBox.Autoselect = false;
            itemsBox.Multiselect = true;
            itemsBox.Width = 30;
            itemsBox.Height = 20;
            Controls.Add(itemsBox);

            itemInfo = new Label(40, 15);
            itemInfo.Width = 40;
            itemInfo.IsMultiline = true;
            Controls.Add(itemInfo);

            Controls.Add(new Label(5, 2) { Text = "Inventory - Press <ESC> to go back", Foreground = ConsoleColor.Gray });

            itemImage = new Image(40, 4);
            itemImage.Border = Cuit.Helpers.RectangleDrawStyle.Single;
            itemImage.BorderColor = ConsoleColor.White;
            itemImage.IsVisible = false;

            Controls.Add(itemImage);

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
            UpdateItemDisplay(e);
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

            UpdateItemDisplay(e);
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

        private void UpdateItemDisplay(Item i)
        {
            itemInfo.Text = $"{i.Name}{(i.Equipped ? " (Equipped)" : "")}{Environment.NewLine}------------------{Environment.NewLine}{i.Description}";

            itemImage.IsVisible = true;
            itemImage.SetImageFromFile(ItemParser.GetItemPath(i));
        }
    }
}
