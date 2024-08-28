using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    public abstract class Card
    {
        //public Player Owner { get; }
        public string Name { get; }
        public int Power { get; }
        public Faction CardFaction { get; }
        public CardType Type { get;}
        public Rank CardRank { get; }
        //public Effect;
        public string description;
    }

    public class UnitCard : Card
    {

    }

    public class SpecialCard : Card
    {
    }


    public enum Faction
    {
        DarkSoul,
        Sekiro,
        EldenRing
    }
    public enum CardType
    {
        Leader,
        Unit,
        Weather,
        ClearwWeather,
        Upgrade
    }
    public enum Position
    {
        Melee,
        Range,
        Siege
    }
    public enum Rank
    {
        Gold,
        Silver,
        Special,
    }
}
