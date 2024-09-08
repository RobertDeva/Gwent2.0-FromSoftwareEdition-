using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    public abstract class Card : IPlayable
    {
        public Player Owner { get; set; }
        public List<IPlayable> Origin { get; set; }
        public bool InField { get => inField; set => inField = value; }
        public bool AffectedByWeather { get; set; }
        public bool AffectedByBuff { get; set; }
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
                {
                    if (!ranges.Contains(item.ToString()))
                        ranges.Add(item.ToString());
                    continue;
                }
                return ranges;
            }
        }
        public string Description { get => description; }
        public bool inField;
        public string name;
        protected double? power;
        public double? actualPower;
        public Faction faction;
        public CardType type;
        public Rank rank;
        public List<Position> range;
        public EffectType effect;
        protected string description;

        public abstract void Invoke(FieldZone zone);
        public abstract void ResetState();
    }

    public class UnitCard : Card
    {
        public UnitCard(string name, int power, Faction faction, List<Position> range, Rank rank ,EffectType effect, string desc)
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
            if(effect == EffectType.Buff || effect == EffectType.Weather || effect == EffectType.Despeje)
                this.effect = EffectType.None;
            this.effect = effect;
            description = desc;
            AffectedByBuff = false;
            AffectedByWeather = false;
        }

        public override void Invoke(FieldZone zone)
        {
            MetodosUtiles.MoveList(this, Origin, zone.InvoqueZone);
            Origin = zone.InvoqueZone;
            Effects.CardEffect(this);
        }
        public override void ResetState()
        {
            actualPower = power;
        }
    }

    public class Lure : UnitCard
    {
        public Lure(string name, Faction faction, string desc) : base(name, 0, faction ,new List<Position>() {Position.Melee, Position.Range, Position.Siege}, GwentEngine.Rank.Special,EffectType.Señuelo,desc)
        {
            rank = GwentEngine.Rank.Special;  
        }

        public override void Invoke(FieldZone zone)
        {
            
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
            effect = EffectType.Buff;
            description = desc;
            AffectedByBuff = false;
            AffectedByWeather = false;
        }

        public override void Invoke(FieldZone zone)
        {
            MetodosUtiles.MoveList(this, Origin, zone.InvoqueZone);
            Origin = zone.InvoqueZone;
            Effects.CardEffect(this);
        }
        public override void ResetState()
        {
            Effects.CardEffect(this);
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
            effect = EffectType.Weather;
            description = desc;
        }

        public override void Invoke(FieldZone zone)
        {
            MetodosUtiles.MoveList(this, Origin, zone.InvoqueZone);
            Origin = zone.InvoqueZone;
            Effects.CardEffect(this);
        }
        public override void ResetState()
        {
            Effects.CardEffect(this);
        }
    }
    public class Despeje : WeatherCard
    {
        public Despeje(string name, Faction faction, string desc) : base(name,faction,desc)
        {
            inField = false;
            this.name = name;
            power = null;
            actualPower = power;
            this.faction = faction;
            type = CardType.Weather;
            rank = GwentEngine.Rank.Special;
            range = new List<Position>();
            effect = EffectType.Despeje;
            description = desc;
            AffectedByBuff = false;
            AffectedByWeather = false;
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

    public enum EffectType
    {
        None,
        Buff,
        Weather,
        InvokeGreatherDeath,
        InvokeDeath,
        Draw,
        Companion,
        Destruction,
        Average,
        Señuelo,
        Despeje        
    }
}
