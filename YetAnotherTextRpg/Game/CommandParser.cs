using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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
                {".*", HandleTrigger },
                {"go", HandleGo },
                {"pickup", HandlePickup }
            };
        }

        public CommandParseResult ProcessCommand(string command)
        {
            var parts = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.ToLower().Trim())
                    .ToArray();

            if (parts.Length < 2)
                return new CommandParseResult { Succeeded = false, Output = "I didn't get that" };

            foreach(var verbHandler in _verbHandlers)
            {
                if (Regex.IsMatch(parts[0], verbHandler.Key))
                {
                    var result = verbHandler.Value(parts);
                    if (result.Continue)
                        continue;

                    return result;
                }
            }

            return new CommandParseResult { Succeeded = false, Output = "I don't know how to do that" };
        }

        private CommandParseResult HandleGo(string[] args)
        {
            if (!Enum.TryParse(args[1], true, out Direction direction))
                return new CommandParseResult { Succeeded = false, Output = "That is not a direction I know off" };

            var exit = GameManager.Instance.ActiveScene.Exits.FirstOrDefault(x => x.Direction == direction);
            if (exit == null)
                return new CommandParseResult { Succeeded = false, Output = "Can't go that way" };

            if (!string.IsNullOrEmpty(exit.Conditional))
            {
                var result = EmbeddedFunctionsHelper.Conditional(exit.Conditional);
                if (!result.Success)
                    return new CommandParseResult { Succeeded = false, Output = result.Output ?? "Seems that is not possible to go that way" };
            }

            GameManager.Instance.SwitchScene(exit.To);
            return new CommandParseResult { Succeeded = true };
        }

        private CommandParseResult HandlePickup(string[] args)
        {
            var pickup = GameManager.Instance.ActiveScene.Pickups.FirstOrDefault(p => p.Phrase == args[1].ToLower());
            if(pickup == null)
                return new CommandParseResult { Succeeded = false, Output = "No such thing to pickup" };

            if (!string.IsNullOrEmpty(pickup.Conditional))
            {
                var result = EmbeddedFunctionsHelper.Conditional(pickup.Conditional);
                if (!result.Success)
                    return new CommandParseResult { Succeeded = false, Output = result.Output ?? "Can't pick that up right now" };
            }

            GameManager.Instance.ActiveScene.Pickups.Remove(pickup);
            GameManager.Instance.State.Variables[$"PICKUP-{pickup.Item.Id}"] = "yes";

            GameManager.Instance.State.Inventory.Add(pickup.Item);

            return new CommandParseResult { Succeeded = true , Output = $"Picked up \"{pickup.Item.Name}\""};
        }

        private CommandParseResult HandleTrigger(string[] args)
        {
            var fullLine = string.Join(" ", args);
            foreach(var trigger in GameManager.Instance.ActiveScene.Triggers)
            {
                if (Regex.IsMatch(fullLine, trigger.Phrase))
                {
                    if (!string.IsNullOrEmpty(trigger.Conditional))
                    {
                        var result = EmbeddedFunctionsHelper.Conditional(trigger.Conditional);
                        if (!result.Success)
                            return new CommandParseResult { Succeeded = false, Output = result.Output ?? "Nothing seems to happen" };
                    }
                    if (!string.IsNullOrEmpty(trigger.Conditional) && !EmbeddedFunctionsHelper.Conditional(trigger.Conditional).Success)
                        continue;

                    var output = EmbeddedFunctionsHelper.Action(trigger.Action);

                    return new CommandParseResult { Succeeded = true, Output = output };
                }
            }

            return new CommandParseResult { Continue = true };
        }
    }
}
