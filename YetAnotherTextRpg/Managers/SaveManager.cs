using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YetAnotherTextRpg.Managers
{
    class SaveManager
    {
        private const string SAVES_FOLDER = "saves";

        //public void LoadGame(int slot)
        //{
        //    var filename = Path.Combine(SAVES_FOLDER, $"{slot}.save");
        //    var data = File.ReadAllText(filename);

        //    GameManager.Instance.State = JsonConvert.DeserializeObject<GameState>(data);
        //}

        //public void SaveGame(int slot)
        //{
        //    var filename = Path.Combine(SAVES_FOLDER, $"{slot}.save");
        //    var data = JsonConvert.SerializeObject(State);

        //    File.WriteAllText(filename, data);
        //}
    }
}
