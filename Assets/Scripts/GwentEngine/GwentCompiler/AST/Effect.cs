using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Effect : ASTNode
        {
            public string Id { get; set; }           
            public List<Param> ParamsExpresions;
            public List<ASTNode> ActionList;
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }

            public Effect(string id, CodeLocation location) : base(location)
            {
                Id = id;
                Location = location;
            }

        }

    }
}