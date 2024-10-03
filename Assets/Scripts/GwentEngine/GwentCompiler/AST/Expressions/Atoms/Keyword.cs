using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Keyword : AtomExpression
        {
            public override ExpressionType Type
            {
                get
                {
                    return ExpressionType.Identifier;
                }
                set { }
            }

            public override object? Value { get; set; }

            public Keyword(string value, CodeLocation location) : base(location)
            {
                Value = value;
            }

            public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
            {
                return true;
            }

            public override void Evaluate()
            {

            }

            public override string ToString()
            {
                return String.Format("{0}", Value);
            }
        }
    }
}