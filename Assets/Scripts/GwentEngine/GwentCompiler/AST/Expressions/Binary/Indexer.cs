using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Indexer : BinaryExpression
        {
            public override ExpressionType Type { get; set; }

            public override object? Value { get; set; }

            public Indexer(CodeLocation location) : base(location)
            {
                Type = ExpressionType.Anytype;
            }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool right = Right.CheckSemantic(context, scope, errors);
                bool left = Left.CheckSemantic(context, scope, errors);

                if (Left is Identifier)
                {
                    bool tuple = scope.AssignedIdentifier(Left.Value.ToString(), out Scope cntx);
                    if (tuple)
                    {
                        Expression expression = cntx.VarYValores[Left.Value.ToString()];
                        Left.Type = expression.Type;
                    }
                }

                if (Right is Identifier)
                {
                    bool tuple = scope.AssignedIdentifier(Left.Value.ToString(), out Scope cntx);
                    if (tuple)
                    {
                        Expression expression = cntx.VarYValores[Left.Value.ToString()];
                        Left.Type = expression.Type;
                    }
                }


                if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.List)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid indexer"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }

                Type = ExpressionType.Card;
                return right && left;
            }

            public override string ToString()
            {
                if (Value == null)
                {
                    return String.Format("{0}[{1}]", Left, Right);
                }
                return Value.ToString();
            }

            public override void Evaluate()
            {

            }
        }
    }
}
