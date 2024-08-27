using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Context
        {
            public List<string> positions;
            public List<string> cards;

            public Context()
            {
                positions = new List<string>();
                cards = new List<string>();
            }

        }
    }
}