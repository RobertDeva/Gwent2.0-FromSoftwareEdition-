using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Card : ASTNode
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public Expression power { get; set; }
            public Faction faction { get; set; }
            public CardType type { get; set; }
            public Rank rank { get; set; }
            public List<string> range { get; set; }
            public string description { get; set; }
            public Effect effect { get; set; }
            

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new System.NotImplementedException();
            }

            public Card(string id ,CodeLocation location) : base(location)
            {
                Id = id;
                Location = location;
            }

        }

    }
}
