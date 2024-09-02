using System; 
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    public abstract class Card : IPlayable
    {
        public string Name { get; protected set; }
        public int? power { get; protected set; }
        public int? actualPower { get; set; }
        public Faction faction { get; protected set; }
        public CardType type { get; protected set; }
        public Rank rank { get; protected set; }
        public List<Position> range { get; protected set; }
        //public Effect;
        public string description { get; protected set; }

        public abstract void Invoke();
    }

    public class UnitCard : Card
    {
        public UnitCard(string name, int power, Faction faction, List<Position> range, Rank rank , string desc)
        {
            Name = name;
            this.power = power;
            actualPower = power;
            this.faction = faction;
            type = CardType.Unit;
            this.range = range;
            if(rank == Rank.Special)
            {
                this.rank = Rank.Silver;
            }
            this.rank = rank;
            description = desc;
        }

        public override void Invoke()
        {

        }
    }

    public class Lure : UnitCard
    {
        public Lure(string name, Faction faction, string desc) : base(name, 0, faction ,new List<Position>() {Position.Melee, Position.Range, Position.Siege}, Rank.Silver,desc)
        {
           
        }

        public override void Invoke()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class SpecialCard : Card
    {
       
    }

    public class UpgradeCard : SpecialCard
    {
        public UpgradeCard(string name, Faction faction, string desc)
        {
            Name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Upgrade;
            rank = Rank.Special;
            range = new List<Position>();
            description = desc;
        }

        public override void Invoke()
        {
            throw new NotImplementedException();
        }
    }

    public class WeatherCard : SpecialCard
    {
        public WeatherCard(string name, Faction faction, string desc)
        {
            Name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Weather;
            rank = Rank.Special;
            range = new List<Position>();
            description = desc;
        }

        public override void Invoke()
        {
            throw new NotImplementedException();
        }
    }

    public class LeaderCard : SpecialCard
    {
        public LeaderCard(string name, Faction faction, string desc)
        {
            Name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Leader;
            rank = Rank.Special;
            range = new List<Position>();
            description = desc;
        }

        public override void Invoke()
        {
            throw new NotImplementedException();
        }
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
