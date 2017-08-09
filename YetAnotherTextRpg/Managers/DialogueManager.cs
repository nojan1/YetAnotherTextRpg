using Cuit;
using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Forms;
using YetAnotherTextRpg.Game;

namespace YetAnotherTextRpg.Managers
{
    public class DialogueManager
    {
        private const string DIALOGUE_FOLDER = "Dialogues";

        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("Not instanciated");

                return _instance;
            }
        }

        public static void Instanciate(CuitApplication application)
        {
            _instance = new DialogueManager(application);
        }

        private DialogueForm _form;
        private readonly CuitApplication _application;
        private DialogueManager(CuitApplication application)
        {
            _application = application;
        }

        public void Open(string name)
        {
            Exit();

            var dialogue = DialogueParser.GetDialogue(name);

            _form = new DialogueForm(dialogue);
            _form.Application = _application;
            _form.InstantiateComponents();

            _application.SwitchTo(_form);
        }

        public void Goto(string id)
        {
            if (_form != null)
                _form.Goto(id);
        }

        public void Exit()
        {
            if(_application.ActiveScreen is DialogueForm)
            {
                _application.GoBack();
            }
        }
    }
}
