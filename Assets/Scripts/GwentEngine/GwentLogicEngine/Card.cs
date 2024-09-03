using System; 
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    public abstract class Card : IPlayable
    {
        public Player Owner { get; set; }
        public bool InField { get => inField; set => inField = value; }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public double Power
        {
            get
            {
                return (double)actualPower;
            }
            set
            {
                actualPower = value;
            }
        }
        public string Faction
        {
            get
            {
                return faction.ToString();
            }
        }
        public string Type
        {
            get
            {
                return type.ToString();
            }
        }
        public string Rank
        {
            get
            {
                return rank.ToString();
            }
        }
        public List<string> Range
        {
            get
            {
                List<string> ranges = new List<string>();
                foreach (var item in range)
                    ranges.Add(item.ToString());
                return ranges;
            }
        }
        public bool inField;
        public string name;
        protected double? power;
        public double? actualPower;
        public Faction faction;
        public CardType type;
        public Rank rank;
        public List<Position> range;
        //public Effect;
        protected string description;

        public abstract void Invoke(FieldZone zone);
        public abstract void ResetState();
    }

    public class UnitCard : Card
    {
        public UnitCard(string name, int power, Faction faction, List<Position> range, Rank rank , string desc)
        {
            inField = false;
            this.name = name;
            this.power = power;
            actualPower = power;
            this.faction = faction;
            type = CardType.Unit;
            this.range = range;
            if(rank == GwentEngine.Rank.Special)
            {
                this.rank = GwentEngine.Rank.Silver;
            }
            this.rank = rank;
            description = desc;
        }

        public override void Invoke(FieldZone zone)
        {

        }
        public override void ResetState()
        {
            actualPower = power;
        }
    }

    public class Lure : UnitCard
    {
        public Lure(string name, Faction faction, string desc) : base(name, 0, faction ,new List<Position>() {Position.Melee, Position.Range, Position.Siege}, GwentEngine.Rank.Special,desc)
        {
            rank = GwentEngine.Rank.Special;  
        }

        public override void Invoke(FieldZone zone)
        {
            throw new NotImplementedException();
        }
        public override void ResetState()
        {
        }
    }
    public abstract class SpecialCard : Card
    {
       
    }

    public class UpgradeCard : SpecialCard
    {
        public UpgradeCard(string name, Faction faction, string desc)
        {
            inField = false;
            this.name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Upgrade;
            rank = GwentEngine.Rank.Special;
            range = new List<Position>();
            description = desc;
        }

        public override void Invoke(FieldZone zone)
        {
            throw new NotImplementedException();
        }
        public override void ResetState()
        {
        }
    }

    public class WeatherCard : SpecialCard
    {
        public WeatherCard(string name, Faction faction, string desc)
        {
            inField = false;
            this.name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Weather;
            rank = GwentEngine.Rank.Special;
            range = new List<Position>();
            description = desc;
        }

        public override void Invoke(FieldZone zone)
        {
            throw new NotImplementedException();
        }
        public override void ResetState()
        {
        }
    }

    public class LeaderCard : SpecialCard
    {
        public LeaderCard(string name, Faction faction, string desc)
        {
            this.name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Leader;
            rank = GwentEngine.Rank.Special;
            range = new List<Position>();
            description = desc;
        }

        public override void Invoke(FieldZone zone)
        {
            throw new NotImplementedException();
        }
        public override void ResetState()
        {
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
