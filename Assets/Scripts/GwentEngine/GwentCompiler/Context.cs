using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Context
        {
            public List<string> range = new List<string>(){ "Melee" , "Range" , "Siege" };
            public List<string> effects;
            public List<string> cards;

            public Context()
            {     
                effects = new List<string>();
                cards = new List<string>();
            }

        }
    }
}