using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Effect : ASTNode
        {

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }

            public Effect(CodeLocation location) : base(location)
            {
                Location = location;
            }

        }

    }
}