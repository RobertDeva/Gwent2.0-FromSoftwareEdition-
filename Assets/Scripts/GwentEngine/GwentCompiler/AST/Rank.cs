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
                throw new System.NotImplementedException();
            }

            public Rank( string value, CodeLocation location) : base(location)
            {
                Value = value;
                Location = location;
            }

        }

    }
}
