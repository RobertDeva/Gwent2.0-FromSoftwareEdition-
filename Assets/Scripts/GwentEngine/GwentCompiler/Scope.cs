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

            public Scope()
            {
                range = new List<string>();
            }

            public Scope CreateChild()
            {
                Scope child = new Scope();
                child.Parent = this;

                return child;
            }

        }
    }
}

