using Cuit.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Controls
{
    class OutputBox : Label
    {
        public OutputBox(int left, int top)
            : base(left, top)
        {
            IsMultiline = true;
        }

        public void AddOutput(string output)
        {
            Text = $"{output}{Environment.NewLine}{Text}";
        }
    }
}
