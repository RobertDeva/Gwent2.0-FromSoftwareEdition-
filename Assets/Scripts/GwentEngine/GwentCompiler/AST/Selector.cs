using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Selector : ASTNode
        {
            public bool IsPostAction { get; set; }
            public string Source { get; set; }
            public Expression Single { get; set; }
            public Expression Predicate { get; set; }

            public Selector(CodeLocation location) : base(location)
            {
                Location = location;
            }
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool source;
                bool single;
                bool predicate;
                List<string> sources = new List<string>()
                {
                    "board", "field", "otherField", "hand", "otherHand", "deck", "otherDeck", "parent"
                };

                if(sources.Contains(Source))
                {
                    if (Source != "parent")
                        source = true;
                    else
                    {
                        if (IsPostAction)
                            source = true;
                        else
                        {
                            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Is not an accesible source"));
                            source = false;
                        }
                    }
                }
                else
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Is not an accesible source"));
                    source = false;
                }

                if(Single.Type == ExpressionType.Bool)
                {
                    single = true;
                }
                else
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Single must be boolean"));
                    single = false;
                }

                if (Predicate is Predicate || Predicate == null)
                {
                    predicate = true;
                }

                else
                {
                    predicate = false;
                }

                return source && single && predicate;
            }
        }
    }
}

