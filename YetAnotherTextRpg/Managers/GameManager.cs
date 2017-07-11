using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Managers
{
    class GameManager
    {
        private const string SAVES_FOLDER = "saves";

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameManager();

                return _instance;
            }
        }

        public bool HasLoadedState => State != null;
        public GameState State { get; private set; }

        public void NewGame()
        {
            State = new GameState();
            State.Inventory.Add(new Item
            {
                Name = "Sword",
                Description = @"A tiny sword...
Can't really hurt anyone with this

Sad kitty :/"
            });
        }

        public void LoadGame(int slot)
        {
            var filename = Path.Combine(SAVES_FOLDER, $"{slot}.save");
            var data = File.ReadAllText(filename);

            State = JsonConvert.DeserializeObject<GameState>(data);
        }

        public void SaveGame(int slot)
        {
            var filename = Path.Combine(SAVES_FOLDER, $"{slot}.save");
            var data = JsonConvert.SerializeObject(State);

            File.WriteAllText(filename, data);
        }
    }
}
