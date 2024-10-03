using System;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Identifier : AtomExpression
        {
            public override ExpressionType Type { get; set; }
            public override object? Value { get; set; }

            public Identifier(string value, CodeLocation location) : base(location)
            {
                Value = value;
                Type = ExpressionType.Identifier;
            }

            public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
            {
                if (this.Value.ToString() == "target" || this.Value.ToString() == "unit" || this.Value.ToString() == "card")
                {
                    Type = ExpressionType.Card;
                }
                else if (Value.ToString() == "context")
                {
                    Type = ExpressionType.Context;
                }
                Debug.Log(Type.ToString() +"/"+Value);
                return true;
            }

            public override void Evaluate()
            {
                if (EffectExecutation.identifiers.ContainsKey(this.Value.ToString()))
                {
                    if (this.Value == null)
                    {
                        return;
                    }
                    Value = EffectExecutation.identifiers[this.Value.ToString()].Value;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}", Value);
            }
        }
    }
}