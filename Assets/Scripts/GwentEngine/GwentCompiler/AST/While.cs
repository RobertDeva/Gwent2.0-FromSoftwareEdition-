using System.Collections;
using System.Collections.Generic;


namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class While : ASTNode
        {
            public Expression Condition;
            public List<ASTNode> ActionList;

            public While(Expression condition, CodeLocation location) : base(location)
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
