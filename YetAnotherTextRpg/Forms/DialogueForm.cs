using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YetAnotherTextRpg.Game;
using YetAnotherTextRpg.Helpers;

public partial class DialogueScreenChoice
{
    public override string ToString()
    {
        return Value.Trim();
    }
}

namespace YetAnotherTextRpg.Forms
{
    public class DialogueForm : SingleFocusControlFormScreen
    {
        private Listbox<DialogueScreenChoice> _choices;
        private Label _output;

        private readonly Dialogue _dialogue;

        public DialogueForm(Dialogue dialogue)
        {
            _dialogue = dialogue;
        }

        public override void InstantiateComponents()
        {
            base.InstantiateComponents();

            _choices = new Listbox<DialogueScreenChoice>(5, 8);
            _choices.Width = Application.Width - 10;
            _choices.Height = 10;
            _choices.SelectionChanged += _choices_SelectionChanged;
            Controls.Add(_choices);

            _output = new Label(5, 2);
            _output.Width = Application.Width - 10;
            _output.IsMultiline = true;
            Controls.Add(_output);

            UpdateDialogue(_dialogue.Screen.First(s => s.Default));
        }

        public void Goto(string id)
        {
            UpdateDialogue(_dialogue.Screen.First(s => s.Id == id));
        }

        private void _choices_SelectionChanged(object sender, DialogueScreenChoice e)
        {
            if (e == null)
                return;

            var output = EmbeddedFunctionsHelper.Action(e.Action);

            if (!string.IsNullOrEmpty(output))
            {
                _output.Text = output;
            }

            _choices.SetSelection(e, false);
        }

        private void UpdateDialogue(DialogueScreen screen)
        {
            _output.Text = OutputHelpers.ProcessOutput(screen.Says.Trim());
            
            _choices.Items.Clear();
            _choices.Items.AddRange(
                screen.Choices.Where(c => string.IsNullOrEmpty(c.DisplayCondition) || 
                                          EmbeddedFunctionsHelper.Conditional(c.DisplayCondition).Success));
        }
    }
}
