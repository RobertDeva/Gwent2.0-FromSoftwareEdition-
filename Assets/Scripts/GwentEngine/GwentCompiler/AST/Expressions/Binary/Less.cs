using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Less : BinaryExpression
        {
            public override ExpressionType Type { get; set; }
            public override object? Value { get; set; }

            public Less(CodeLocation location) : base(location) { }

            public override void Evaluate()
            {
                Right.Evaluate();
                Left.Evaluate();

                if ((double)Left.Value < (double)Right.Value)
                    Value = true;
                else
                    Value = false;
            }
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool right = Right.CheckSemantic(context, scope, errors);
                bool left = Left.CheckSemantic(context, scope, errors);
                if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "We don't do that here... "));
                    Type = ExpressionType.ErrorType;
                    return false;
                }

                Type = ExpressionType.Bool;
                return left && right;
            }
            public override string ToString()
            {
                if (Value == null)
                {
                    return String.Format("({0} < {1})", Left, Right);
                }
                return Value.ToString();
            }
        }
    }
}
