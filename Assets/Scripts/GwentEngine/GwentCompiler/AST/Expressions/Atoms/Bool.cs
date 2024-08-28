using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Bool : AtomExpression
        {
            public bool IsBool
            {
                get
                {
                    bool a;
                    return bool.TryParse(Value.ToString(), out a);
                }
                
            }
            public override ExpressionType Type
            {
                get
                {
                    return ExpressionType.Bool;
                }
                set { }
            }

            public override object? Value { get; set; }

            public Bool(string value, CodeLocation location) : base(location)
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