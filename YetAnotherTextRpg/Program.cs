using Cuit;
using System;
using YetAnotherTextRpg.Forms;
using YetAnotherTextRpg.Managers;

namespace YetAnotherTextRpg
{
    class Program
    {
        static void Main(string[] args)
        {
            var application = new CuitApplication();
            DialogueManager.Instanciate(application);
            EncounterManager.Instanciate(application);

            application.Run<MenuForm>();
        }
    }
}