using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Faction : ASTNode
        {
            public string Value { get; set; }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
               if(context.factions.Contains(Value))
                    return true;
               return false;
            }

            public Faction(string value, CodeLocation location) : base(location)
            {
                Value = value;
                Location = location;
            }

        }

    }
}
