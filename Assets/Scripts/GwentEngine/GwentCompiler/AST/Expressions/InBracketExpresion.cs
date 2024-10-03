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

            public InBracketExpression(Expression exp,CodeLocation location): base(location)
            {
               InnerExpression = exp;
            }
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                return InnerExpression.CheckSemantic(context, scope, errors);
            }

            public override void Evaluate()
            {
                InnerExpression.Evaluate();
                Value = InnerExpression.Value;
            }

            public override string ToString()
            {
                string x = InnerExpression.ToString();
                return String.Format("({0})", x);
            }
        }
    }
}

