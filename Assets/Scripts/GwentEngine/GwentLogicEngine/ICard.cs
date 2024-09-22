using System;
using System.Collections.Generic;


namespace GwentEngine
{
    public interface ICard
    {
        public Player Owner { get; set; }
        public bool InField { get; set; }
        public bool AffectedByWeather { get; set; }
        public bool AffectedByBuff { get; set; }

        public List<ICard> Origin { get; set; }
        public string Name { get; }
        public abstract double Power { get; set; }
        public abstract string Faction { get; }
        public abstract string Type { get; }
        public abstract string Rank { get; }
        public abstract List<string> Range { get; }
        public abstract string Description { get; }

        public void Invoke(FieldZone? zone)
        {
            MetodosUtiles.MoveList(this, Origin, zone.InvoqueZone);
            Origin = zone.InvoqueZone;
        }
        public void ResetState();
        public abstract void CastEffect();
        public void InvokeInMelee(out bool cardplay)
        {
           Set(Owner.Melee, out cardplay);
        }
        public void InvokeInRange(out bool cardplay)
        {
           Set(Owner.Range, out cardplay);
        }
        public void InvokeInSiege(out bool cardplay)
        {
           Set(Owner.Siege, out cardplay);
        }
        
        public void Set(UnitZone zone,out bool played)
        {
            if (Origin == Owner.Hand)
            {
                if (Type == CardType.Weather.ToString())
                {
                    if (zone.WeatherZone.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.MeleeWeather);
                        InField = true;
                        played = true;
                        return;
                    }
                    else
                    {
                        CastEffect();
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                        played = true;
                        return;
                    }
                }
                else if (Type == CardType.Upgrade.ToString())
                {
                    if (zone.UpgradeZone.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.UpgradeMelee);
                        InField = true;
                        played = true;
                        return;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                        played = true;
                        return;
                    }
                }
                else if (Range.Contains(zone.FieldRange.ToString()))
                {
                    Invoke(Owner.Melee);
                    InField = true;
                    played = true;
                    return;
                }
            }
            played = false;
        }
    }
}