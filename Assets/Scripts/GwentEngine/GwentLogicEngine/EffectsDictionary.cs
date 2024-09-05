using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    public static class EffectsDictionary
    {
        public delegate void Effect(Card card);
        public static Dictionary<string,Effect> EffectDictionary;
    }
}
