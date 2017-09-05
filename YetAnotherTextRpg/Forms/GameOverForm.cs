using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Helpers;
using YetAnotherTextRpg.Managers;

namespace YetAnotherTextRpg.Forms
{
    public class GameOverForm : SingleFocusControlFormScreen
    {
        public override void InstantiateComponents()
        {
            base.InstantiateComponents();

            var continueButton = new Button((Application.Width / 2) - 5, 10);
            continueButton.Text = "Continue";
            continueButton.Click += ContinueButton_Click;
            RegisterControl(continueButton);

            var label = Application.CreateCenteredLabel(5, "You died! Succccckkkerr!!!!!");
            RegisterControl(label);
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            GameManager.Instance.ClearState();
            Application.SwitchTo<MenuForm>();
            //Application.ClearHistory();
        }
    }
}
