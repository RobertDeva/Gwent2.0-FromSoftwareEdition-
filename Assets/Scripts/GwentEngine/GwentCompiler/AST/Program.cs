using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class ElementalProgram : ASTNode
        {
            public List<CompilingError> Errors { get; set; }
            public Dictionary<string, Card> Cards { get; set; }
            public Dictionary<string, Effect> Effects { get; set; }


            public ElementalProgram(CodeLocation location) : base(location)
            {
                Errors = new List<CompilingError>();
                Cards = new Dictionary<string, Card>();
                Effects = new Dictionary<string, Effect>();
            }

            /* To check a program semantic we sould first collect all the existing elements and store them in the context.
            Then, we check semantics of elements and cards */
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool checkEffects = true;
                foreach (Effect effect in Effects.Values)
                {
                    checkEffects = checkEffects && effect.CollectElements(context, scope.CreateChild(), errors);
                }
                foreach (Effect effect in Effects.Values)
                {
                    checkEffects = checkEffects && effect.CheckSemantic(context, scope.CreateChild(), errors);
                }

                bool checkCards = true;
                foreach (Card card in Cards.Values)
                {
                    checkCards = checkCards && card.CheckSemantic(context, scope, errors);
                }

                return checkCards && checkEffects;
            }

            public void Evaluate()
            {
                foreach (Card card in Cards.Values)
                {
                    //card.Evaluate();
                }
            }

            public override string ToString()
            {
                string s = "";

                foreach (Card card in Cards.Values)
                {
                    s += "\n" + card.ToString();
                }
                return s;
            }
        }
    }
}
