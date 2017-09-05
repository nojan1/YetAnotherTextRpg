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
        public Scene ActiveScene { get; set; }

        private GameManager() { }

        public void NewGame()
        {
            State = new GameState();
            State.Inventory.Add(new MoneyItem());

            State.Health = 40;
            State.MaxHealth = 40;
            State.Variables["armor"] = "5";
            State.Variables["attack"] = "8";
            State.Variables["defense"] = "6";
            State.Variables["strength"] = "7";

            SwitchScene("start");
        }

        public void ClearState()
        {
            State = null;
        }

        public void StartLoadedGame(GameState state)
        {
            State = state;
            SwitchScene(state.CurrentScene);
        }

        public void SwitchScene(string sceneName)
        {
            State.CurrentScene = sceneName;
            ActiveScene = Game.SceneParser.GetScene(State.CurrentScene);
        }
    }
}
