using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Game
{
    public class CommandParser
    {
        private readonly Dictionary<string, Func<string[], CommandParseResult>> _verbHandlers;

        public CommandParser()
        {
            _verbHandlers = new Dictionary<string, Func<string[], CommandParseResult>>()
            {
                {"go", HandleGo }
            };
        }

        public CommandParseResult ProcessCommand(string command)
        {
            var parts = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.ToLower().Trim())
                    .ToArray();

            if (parts.Length < 2)
                return new CommandParseResult { Succeeded = false, Output = "I didn't get that" };

            if (!_verbHandlers.ContainsKey(parts[0]))
                return new CommandParseResult { Succeeded = false, Output = "I don't know how to do that" };

            return _verbHandlers[parts[0]](parts.Skip(1).ToArray());
        }

        private CommandParseResult HandleGo(string[] args)
        {
            if (!Enum.TryParse(args[0], true, out Direction direction))
                return new CommandParseResult { Succeeded = false, Output = "That is not a direction I know off" };

            var exit = GameManager.Instance.ActiveScene.Exits.FirstOrDefault(x => x.Direction == direction);
            if (exit == null)
                return new CommandParseResult { Succeeded = false, Output = "Can't go that way" };

            if (!string.IsNullOrEmpty(exit.Conditional) && !EmbeddedFunctionsHelper.Conditional(exit.Conditional))
                return new CommandParseResult { Succeeded = false, Output = "Seems that is not possible to go that way" };


            GameManager.Instance.SwitchScene(exit.To);
            return new CommandParseResult { Succeeded = true };
        }
    }
}
