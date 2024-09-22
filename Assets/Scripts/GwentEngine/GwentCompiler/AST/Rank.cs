using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Rank : ASTNode
        {
            public string Value { get; set; }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
               if(context.ranks.Contains(Value))
                    return true;
                return false;
            }

            public Rank( string value, CodeLocation location) : base(location)
            {
                Value = value;
                Location = location;
            }

        }

    }
}
