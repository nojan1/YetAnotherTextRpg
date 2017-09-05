using Cuit;
using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Forms;
using YetAnotherTextRpg.Game;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Managers
{
    public class EncounterManager
    {
        private static EncounterManager _instance;
        public static EncounterManager Instance
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
            _instance = new EncounterManager(application);
        }

        private EncounterForm _encounterForm;
        private readonly CuitApplication _application;
        private EncounterManager(CuitApplication application)
        {
            _application = application;
        }

        public void LoadEncounter(string name)
        {
            var encounter = EncounterParser.GetEncounter(name);

            _encounterForm = new EncounterForm(encounter);
            _encounterForm.Application = _application;
            _encounterForm.InstantiateComponents();

            _application.SwitchTo(_encounterForm);
        }

        public void PlayerDied(Encounter encounter)
        {
            Exit();

            if (!string.IsNullOrEmpty(encounter.OnDefeatAction))
            {
                var result = EmbeddedFunctionsHelper.Conditional(encounter.OnDefeatAction);
                if (result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Output) && _application.ActiveScreen is GameScreen)
                    {
                        (_application.ActiveScreen as GameScreen).AppendOutput(result.Output);
                    }

                    return;
                }
            }

            _application.SwitchTo<GameOverForm>();
        }

        public void PlayerWon(Encounter encounter)
        {
            Exit();

            if (!string.IsNullOrEmpty(encounter.OnVictoryAction))
            {
                var output = EmbeddedFunctionsHelper.Action(encounter.OnVictoryAction);
                if (!string.IsNullOrEmpty(output) && _application.ActiveScreen is GameScreen)
                {
                    (_application.ActiveScreen as GameScreen).AppendOutput(output);
                }
            }
        }

        public void Exit()
        {
            if (_application.ActiveScreen is EncounterForm)
            {
                _application.GoBack();
            }
        }
    }
}
