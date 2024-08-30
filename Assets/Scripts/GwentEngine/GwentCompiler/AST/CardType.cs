using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class CardType : ASTNode
        {
            public string Value { get; set; }


            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }

            public CardType(string value, CodeLocation location) : base(location)
            {
                Value = value;
                Location = location;
            }

        }

    }
}