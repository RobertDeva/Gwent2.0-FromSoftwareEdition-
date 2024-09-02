using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class InBracketExpression : Expression
        {
            public Expression InnerExpression { get; set; }
            public override ExpressionType Type {
                get 
                {
                    return InnerExpression.Type; 
                }
                set 
                { }
            }
            public override object Value { get; set; }

            public InBracketExpression(CodeLocation location): base(location)
            {
               Value = InnerExpression.Value;
            }
            public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
            {
                throw new NotImplementedException();
            }

            public override void Evaluate()
            {

            }

            public override string ToString()
            {
                return String.Format("({0})", Value);
            }
        }
    }
}

