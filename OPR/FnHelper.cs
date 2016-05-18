using System;
using NCalc;

namespace OPR
{
    public sealed class FnHelper
    {
        private readonly Expression Expression;

        public FnHelper(string expression)
        {
            Expression = new Expression(expression, EvaluateOptions.IgnoreCase);
            //Expression.EvaluateParameter += (name, args) =>
            //{
            //    if (name == "E")
            //    {
            //        args.Result = Math.E;
            //    }
            //};
        }

        public Func<float, float, float> Fn
        {
            get
            {
                Func<float, float, float> fn = (x, y) =>
                {
                    Expression.Parameters["x"] = x;
                    Expression.Parameters["y"] = y;
                    Expression.Parameters["E"] = Math.E;
                    return (float) (double) Expression.Evaluate();
                };
                return fn;
            }
        }
    }
}
