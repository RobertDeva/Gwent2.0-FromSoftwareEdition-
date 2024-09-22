using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Greather : BinaryExpression
        {
            public override ExpressionType Type { get; set; }
            public override object? Value { get; set; }

            public Greather(CodeLocation location) : base(location) { }

            public override void Evaluate()
            {
                Right.Evaluate();
                Left.Evaluate();

                if ((double)Left.Value > (double)Right.Value)
                    Value = true;
                else
                    Value = false;
            }
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool right = Right.CheckSemantic(context, scope, errors);
                bool left = Left.CheckSemantic(context, scope, errors);
                if (Left is Identifier)
                {
                    bool identifier = scope.AssignedIdentifier(Left.Value.ToString(), out Scope cntx);
                    if (identifier)
                    {
                        Expression exp = cntx.VarYValores[Left.Value.ToString()];
                        Left.Type = exp.Type;
                    }
                }
                if (Right is Identifier)
                {
                    bool identifier = scope.AssignedIdentifier(Right.Value.ToString(), out Scope cntx);
                    if (identifier)
                    {
                        Expression exp = cntx.VarYValores[Right.Value.ToString()];
                        Left.Type = exp.Type;
                    }
                }
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
                    return String.Format("({0} > {1})", Left, Right);
                }
                return Value.ToString();
            }
        }
    }
}