using System.Collections;
using System.Collections.Generic;


namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class While : ASTNode
        {
            public Expression expression { get; set; }
            public List<Expression> expressions;

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }
            public While(CodeLocation location) : base(location)
            {

            }
        }

    }
}
