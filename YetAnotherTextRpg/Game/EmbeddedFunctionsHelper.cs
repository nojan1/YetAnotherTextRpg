using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using YetAnotherTextRpg.Managers;
using System.Linq;

namespace YetAnotherTextRpg.Game
{
    public static class EmbeddedFunctionsHelper
    {
        public static bool Conditional(string expression)
        {
            var parameters = new ParameterExpression[]
            {
                Expression.Parameter(typeof(Func<string, bool>), "InInventory"),
                Expression.Parameter(typeof(Func<string, string>), "GetVariable"),
                Expression.Parameter(typeof(Func<string, bool>), "PickupInScene")
            };

            var values = new object[]
            {
                new Func<string, bool>((id) => GameManager.Instance.State.Inventory.Any(i => i.Id == id)),
                new Func<string, string>((key) => GameManager.Instance.State.Variables.FirstOrDefault(x => x.Key == key).Value),
                new Func<string, bool>((id) => GameManager.Instance.State.Variables.ContainsKey($"PICKUP-{id}"))
            };

            var lambda = DynamicExpressionParser.ParseLambda(false, parameters, null, expression);
            var del = lambda.Compile();

            return (bool)del.DynamicInvoke(values);
        }
    }
}
