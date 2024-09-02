using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Predicate : BinaryExpression
        {
            public override ExpressionType Type { get; set; }
            public override object? Value { get; set; }

            public Predicate(CodeLocation location) : base(location) { }

            public override void Evaluate()
            {
                Type = ExpressionType.Anytype;
            }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new NotImplementedException();
            }
            public override string ToString()
            {
                if (Value == null)
                {
                    return String.Format("({0}) => {1}", Left, Right);
                }
                return Value.ToString();
            }
        }

    }

}
