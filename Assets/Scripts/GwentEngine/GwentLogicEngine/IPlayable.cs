using System;
using System.Collections.Generic;


namespace GwentEngine
{
    public interface IPlayable
    {
        public Player Owner { get; set; }
        public bool InField { get; set; }
        public string Name { get; }
        public abstract double Power { get; set; }
        public abstract string Faction { get; }
        public abstract string Type { get; }
        public abstract string Rank { get; }
        public abstract List<string> Range { get; }

        public abstract void Invoke(FieldZone? zone);
        public abstract void ResetState();
         
    }
}