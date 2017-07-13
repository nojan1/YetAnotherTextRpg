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
                Expression.Parameter(typeof(InventoryParameterHelper), "Inventory"),
                Expression.Parameter(typeof(VariablesParameterHelper), "Variables"),
                Expression.Parameter(typeof(PickupsParameterHelper), "Pickup")
            };

            var values = new object[]
            {
                new InventoryParameterHelper(),
                new VariablesParameterHelper(),
                new PickupsParameterHelper()
            };

            var lambda = DynamicExpressionParser.ParseLambda(false, parameters, null, expression);
            var del = lambda.Compile();

            return (bool)del.DynamicInvoke(values);
        }
    }
}
