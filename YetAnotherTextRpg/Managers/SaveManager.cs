using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Managers
{
    class SaveManager
    {
        private const string SAVES_FOLDER = "saves";

        public SaveManager()
        {
            if (!Directory.Exists(SAVES_FOLDER))
            {
                Directory.CreateDirectory(SAVES_FOLDER);
            }
        }

        public void LoadGame(int slot)
        {
            var filename = Path.Combine(SAVES_FOLDER, $"{slot}.save");
            var data = File.ReadAllText(filename);
            var state = JsonConvert.DeserializeObject<GameState>(data); ;

            GameManager.Instance.StartLoadedGame(state);
        }

        public void SaveGame(int slot)
        {
            var filename = Path.Combine(SAVES_FOLDER, $"{slot}.save");
            var data = JsonConvert.SerializeObject(GameManager.Instance.State);

            File.WriteAllText(filename, data);
        }

        public IEnumerable<SaveGameListing> ListSaves()
        {
            return Directory.EnumerateFiles(SAVES_FOLDER, "*.save")
                .Select(s => new SaveGameListing
                {
                    Slot = Convert.ToInt32(Path.GetFileNameWithoutExtension(s)),
                    Created = File.GetCreationTime(s)
                });
        }
    }
}
