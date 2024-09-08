using System;
using System.Collections.Generic;


namespace GwentEngine
{
    public interface IPlayable
    {
        public Player Owner { get; set; }
        public bool InField { get; set; }
        public bool AffectedByWeather { get; set; }
        public bool AffectedByBuff { get; set; }

        public List<IPlayable> Origin { get; set; }
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
        public void InvokeInMelee()
        {
            if (Origin == Owner.Hand)
            {
                if (Type == CardType.Weather.ToString())
                {
                    if (Owner.MeleeWeather.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.MeleeWeather);
                        InField = true;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                    }
                }
                else if (Type == CardType.Upgrade.ToString())
                {
                    if (Owner.UpgradeMelee.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.UpgradeMelee);
                        InField = true;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                    }
                }
                else if (Range.Contains(Position.Melee.ToString()))
                {
                    Invoke(Owner.Melee);
                    InField = true;
                } 
            }
        }
        public void InvokeInRange()
        {
            if (Origin == Owner.Hand)
            {
                if (Type == CardType.Weather.ToString())
                {
                    if (Owner.RangeWeather.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.RangeWeather);
                        InField = true;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                    }
                }
                else if (Type == CardType.Upgrade.ToString())
                {
                    if (Owner.UpgradeRange.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.UpgradeRange);
                        InField = true;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                    }
                }
                else if (Range.Contains(Position.Range.ToString()))
                {
                    Invoke(Owner.Melee);
                    InField = true;
                }
            }
        }
        public void InvokeInSiege()
        {
            if (Origin == Owner.Hand)
            {
                if (Type == CardType.Weather.ToString())
                {
                    if (Owner.SiegeWeather.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.SiegeWeather);
                        InField = true;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                    }
                }
                else if (Type == CardType.Upgrade.ToString())
                {
                    if (Owner.UpgradeSiege.InvoqueZone.Count < 1)
                    {
                        Invoke(Owner.UpgradeSiege);
                        InField = true;
                    }
                    else
                    {
                        MetodosUtiles.MoveList(this, Owner.Hand, Owner.Graveyard);
                    }
                }
                else if (Range.Contains(Position.Siege.ToString()))
                {
                    Invoke(Owner.Siege);
                    InField = true;
                }
            }
        }

    }
}