using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class For : ASTNode
        {
            public List<ASTNode> ActionList;

            public For(CodeLocation location) : base(location)
            {
                ActionList = new List<ASTNode>();
            }
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }
            
        }
        
    }
}

