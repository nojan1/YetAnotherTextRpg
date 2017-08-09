using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using YetAnotherTextRpg.Managers;
using System.Linq;

namespace YetAnotherTextRpg.Game
{
    public class FunctionReturnValue
    {
        public bool Success { get; set; }
        public string Output { get; set; }
    }

    public static class EmbeddedFunctionsHelper
    {
        public static FunctionReturnValue Conditional(string expression)
        {
            expression = expression.Replace(" AND ", " && ");

            var output = new OutputParameterHelper();

            var parameters = new ParameterExpression[]
            {
                Expression.Parameter(typeof(InventoryParameterHelper), "Inventory"),
                Expression.Parameter(typeof(VariablesParameterHelper), "Variables"),
                Expression.Parameter(typeof(PickupsParameterHelper), "Pickup"),
                Expression.Parameter(typeof(OutputParameterHelper), "Output"),
                Expression.Parameter(typeof(SceneParameterHelper), "Scene")
            };

            var values = new object[]
            {
                new InventoryParameterHelper(),
                new VariablesParameterHelper(),
                new PickupsParameterHelper(),
                output,
                new SceneParameterHelper()
            };

            var lambda = DynamicExpressionParser.ParseLambda(false, parameters, null, expression);
            var del = lambda.Compile();

            return new FunctionReturnValue
            {
                Success = (bool)del.DynamicInvoke(values),
                Output = output.Output
            };
        }

        public static string Action(string expression)
        {
            return Conditional(expression).Output;
        }
    }
}
