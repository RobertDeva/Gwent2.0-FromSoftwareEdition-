using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler 
    {
        public abstract class ASTNode
        {
            public CodeLocation Location { get; set; }
            public abstract bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors);
            public ASTNode(CodeLocation location)
            {
                Location = location;
            }
        }
    }
}
