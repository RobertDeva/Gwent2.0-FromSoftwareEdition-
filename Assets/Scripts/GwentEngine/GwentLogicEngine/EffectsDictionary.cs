using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    public class EffectsDictionary
    {
        public delegate void Effect(ICard card);
        public static Dictionary<string,Effect> EffectDictionary = new();

        public void AddToDictionary()
        {
            EffectDictionary.Add(EffectType.Weather.ToString(), Effects.Weather);
            EffectDictionary.Add(EffectType.Buff.ToString(), Effects.Buff);
            EffectDictionary.Add(EffectType.InvokeGreatherDeath.ToString(), Effects.GreaterDeath);
            EffectDictionary.Add(EffectType.InvokeDeath.ToString(), Effects.Death);
            EffectDictionary.Add(EffectType.Draw.ToString(), Effects.Draw);
            EffectDictionary.Add(EffectType.Companion.ToString(), Effects.Companion);
            EffectDictionary.Add(EffectType.Destruction.ToString(), Effects.Destruction);
            EffectDictionary.Add(EffectType.Average.ToString(), Effects.Average);
            EffectDictionary.Add(EffectType.Despeje.ToString(), Effects.Despeje);
        }
    }
}
