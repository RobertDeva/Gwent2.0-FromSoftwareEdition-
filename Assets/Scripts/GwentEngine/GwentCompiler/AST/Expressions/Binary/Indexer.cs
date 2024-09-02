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
                throw new NotImplementedException();
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
