using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Context
        {
            public List<string> range = new(){ "Melee" , "Range" , "Siege" };
            public List<string> factions = new() { "DarkSoul", "Sekiro", "EldenRing"};
            public List<string> ranks = new() { "Gold", "Silver", "Special" };
            public List<string> types = new() { "Unit", "Weather", "Upgrade" };
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