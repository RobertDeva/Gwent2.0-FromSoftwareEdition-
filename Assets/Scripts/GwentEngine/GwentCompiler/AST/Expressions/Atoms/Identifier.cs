using System;
using System.Collections.Generic;


namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Identifier : AtomExpression
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

            public Identifier(string value, CodeLocation location) : base(location)
            {
                Value = value;
            }

            public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
            {
                if (this.Value.ToString() == "target" || this.Value.ToString() == "unit" || this.Value.ToString() == "card")
                {
                    Type = ExpressionType.Card;
                }
                else if (this.Value.ToString() == "context")
                {
                    Type = ExpressionType.Context;
                }
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