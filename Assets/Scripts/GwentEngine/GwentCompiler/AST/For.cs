using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class For : ASTNode
        {
            public List<Expression> expressions;

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }
            public For(CodeLocation location) : base(location)
            {

            }
        }
        
    }
}

