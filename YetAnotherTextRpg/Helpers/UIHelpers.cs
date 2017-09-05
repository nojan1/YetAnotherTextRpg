using Cuit;
using Cuit.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Helpers
{
    public static class UIHelpers
    {
        public static Label CreateCenteredLabel(this CuitApplication application, int top, string text)
        {
            int left = (application.Width / 2) - (text.Length / 2);
            var label = new Label(left, top);
            label.Text = text;

            return label;
        }
    }
}
