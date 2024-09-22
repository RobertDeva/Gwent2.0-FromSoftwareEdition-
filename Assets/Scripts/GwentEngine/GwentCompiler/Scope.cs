using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Scope
        {
            public Scope? Parent;

            public List<string> range;
            public Dictionary<string, Expression> VarYValores;

            public Scope()
            {
                range = new ();
                VarYValores = new();
            }

            public Scope CreateChild()
            {
                Scope child = new();
                child.Parent = this;

                return child;
            }

            public bool AssignedIdentifier(string Identifier, out Scope scope)
            {
                if (VarYValores.ContainsKey(Identifier))
                {
                    scope = this;
                    return true;
                }
                else
                {
                    if (Parent == null)
                    {
                        scope = null;
                        return false;
                    }

                    return Parent.AssignedIdentifier(Identifier, out scope);
                }
            }
        }
    }
}

