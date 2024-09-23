using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GwentEngine.GwentCompiler;

namespace GwentEngine
{
    public class Effects
    {
        public static Dictionary<string,Effect> CompilatedEffects = new Dictionary<string,Effect>();
        public static void CardEffect(Card card)
        {
            if (EffectsDictionary.EffectDictionary.TryGetValue(card.effect.ToString(), out var effect))
            {
                effect(card);
            }
        }
        public static void Señuelo(ICard TriggerCard, ICard AffectedCard, out bool swicht)
        {
            if(TriggerCard.Owner == AffectedCard.Owner && !TriggerCard.InField)
            {
                if(AffectedCard.InField && AffectedCard.Rank == "Silver")
                {
                    MetodosUtiles.MoveList(TriggerCard, TriggerCard.Origin, AffectedCard.Origin);
                    TriggerCard.Origin = AffectedCard.Origin;
                    TriggerCard.InField = true;
                    MetodosUtiles.MoveList(AffectedCard, AffectedCard.Origin, AffectedCard.Owner.Hand);
                    AffectedCard.Origin = AffectedCard.Owner.Hand;
                    AffectedCard.InField = false;
                    swicht = true;
                    return;
                }
            }
            swicht = false;
        }
        public static void Weather(ICard Card)
        {
            WeatherZone weather = CheckWeatherZone((Card)Card);
            if(weather != null)
            {
                foreach(var item in weather.UnitZone1.InvoqueZone)
                    if (item.Rank == Rank.Silver.ToString())
                         item.AffectedByWeather = true;
                foreach (var item in weather.UnitZone2.InvoqueZone)
                    if (item.Rank == Rank.Silver.ToString())
                         item.AffectedByWeather = true;
            }
        }
        public static void Buff(ICard Card)
        {
            UpgradeZone upgrade = CheckUpgradeZone((Card)Card);
            if (upgrade != null)
            {
                foreach(var item in upgrade.UnitZone.InvoqueZone)
                    if (item.Rank == Rank.Silver.ToString())
                         item.AffectedByBuff = true;
            }
        }
        public static void Destruction(ICard Card)
        {
            UnitZoneToDestroy();
        }
        public static void GreaterDeath(ICard Card)
        {
            double M1 = MaxPower(Board.Melee1.InvoqueZone);
            double M2 = MaxPower(Board.Melee2.InvoqueZone);
            double R1 = MaxPower(Board.Range1.InvoqueZone);
            double R2 = MaxPower(Board.Range2.InvoqueZone);
            double S1 = MaxPower(Board.Siege1.InvoqueZone);
            double S2 = MaxPower(Board.Siege2.InvoqueZone);

            double max1 = Math.Max(M1,Math.Max(S1,R1));
            double max2 = Math.Max(M2,Math.Max(S2,R2));

            double max = Math.Max(max1,max2);

            GreaterDeathClean(max);
        }
        public static void Death(ICard Card)
        {
            double MO = MinPower(Card.Owner.Oponent.Melee.InvoqueZone);
            double RO = MinPower(Card.Owner.Oponent.Range.InvoqueZone);
            double SO = MinPower(Card.Owner.Oponent.Siege.InvoqueZone);

            double Min = MinValueNoCero(MO, RO, SO);

            DeathClean(Min, (Card)Card);
        }
        public static void Draw(ICard Card)
        {
            Card.Owner.Draw(out ICard card);            
        }
        public static void Companion(ICard Card)
        {
            int SimilarCards = 0;
            List<ICard> cards = new List<ICard>();
            SimilarCards = CheckSimilarCards(cards,(Card)Card);

            foreach (ICard card in cards)
            {
                card.Power = card.Power * SimilarCards;
            }
        }
        public static void Average(ICard Card)
        {
            int count = 0;
            List<ICard> cards = new List<ICard>();
            double Total = CheckCardsInField(count,cards);

            foreach(ICard card in cards)
            {
                card.Power = Total / count;
            }
        }
        public static void Despeje(ICard Card)
        {
            CleanWeather();
            CleanWeatherConsecuences();
            Card.Origin = Card.Owner.Graveyard;
        }
        private static void CleanWeather()
        {
            foreach (var item in Board.MeleeWeather.InvoqueZone)
            {
                MetodosUtiles.MoveList(item, item.Origin, item.Owner.Graveyard);
                item.Origin = item.Owner.Graveyard;
            }
            foreach (var item in Board.RangeWeather.InvoqueZone)
            {
                MetodosUtiles.MoveList(item, item.Origin, item.Owner.Graveyard);
                item.Origin = item.Owner.Graveyard;
            }
            foreach (var item in Board.SiegeWeather.InvoqueZone)
            {
                MetodosUtiles.MoveList(item, item.Origin, item.Owner.Graveyard);
                item.Origin = item.Owner.Graveyard;
            }
        }
        private static void CleanWeatherConsecuences()
        {
            CleanLine(Board.Melee1.InvoqueZone);
            CleanLine(Board.Range1.InvoqueZone);
            CleanLine(Board.Siege1.InvoqueZone);
            CleanLine(Board.Melee2.InvoqueZone);
            CleanLine(Board.Range2.InvoqueZone);
            CleanLine(Board.Siege2.InvoqueZone);

        }
        private static void CleanLine(List<ICard> cards)
        {
            foreach (var item in cards)
            {
                if (item.AffectedByWeather)
                {
                    item.AffectedByWeather = false;
                }
            }
        }
        private static double CheckCardsInField(int count, List<ICard> cards)
        {
            double Total = 0;
            CheckInLine(count, Board.Melee1.InvoqueZone, cards, Total);
            CheckInLine(count, Board.Range1.InvoqueZone, cards, Total);
            CheckInLine(count, Board.Siege1.InvoqueZone, cards, Total);
            CheckInLine(count, Board.Melee2.InvoqueZone, cards, Total);
            CheckInLine(count, Board.Range2.InvoqueZone, cards, Total);
            CheckInLine(count, Board.Siege2.InvoqueZone, cards, Total);
            return Total;
        }
        private static void CheckInLine(int count,List<ICard> InField , List<ICard> cards, double power)
        {
            foreach (ICard card in InField)
            {
                power += card.Power;
                count++;
                if(card.Rank == Rank.Silver.ToString())
                {
                    cards.Add(card);
                }
            }
        }

        private static int CheckSimilarCards(List<ICard> Affected, Card Card)
        {
            int count = 0;
            CheckInList(count, Board.Melee1.InvoqueZone, Affected, Card);
            CheckInList(count, Board.Range1.InvoqueZone, Affected, Card);
            CheckInList(count, Board.Siege1.InvoqueZone, Affected, Card);
            CheckInList(count, Board.Melee2.InvoqueZone, Affected, Card);
            CheckInList(count, Board.Range2.InvoqueZone, Affected, Card);
            CheckInList(count, Board.Siege2.InvoqueZone, Affected, Card);
            return count;
        }
        private static void CheckInList(int count, List<ICard> InFieldCards, List<ICard> Affected, Card Card)
        {
            foreach(var card in InFieldCards)
            {
                if (Card.Name == card.Name)
                {
                    Affected.Add(card);
                    count++;
                }
            }
        }

        public static void DeathClean(double min, ICard Card)
        {
            List<UnitZone> unitZones = new List<UnitZone>() {Card.Owner.Oponent.Melee, Card.Owner.Oponent.Range, Card.Owner.Oponent.Siege };
            foreach (UnitZone unitZone in unitZones)
            {
                foreach (var item in unitZone.InvoqueZone)
                {
                    if(item.Rank == Rank.Silver.ToString())
                    {
                        if(item.Power == min)
                        {
                            MetodosUtiles.MoveList(item,item.Origin,item.Owner.Graveyard);
                            item.Origin = item.Owner.Graveyard;
                        }
                    }
                }
            }
        }
        private static double MinPower(List<ICard> cards)
        {
            double Min = int.MaxValue;
            foreach (var card in cards)
            {
                if (card.Rank == Rank.Silver.ToString())
                {
                    if(card.Power < Min)
                    {
                        Min = card.Power;
                    }
                }

            }
            return Min;
        }

        private static double MaxPower(List<ICard> cards)
        {
            double Max = int.MinValue;
            foreach(var card in cards)
            {
                if(card.Rank == Rank.Silver.ToString())
                    if(card.Power > Max)
                        Max = card.Power;
            }
            return Max;
        }
        private static void GreaterDeathClean(double power)
        {
            List<UnitZone> BoardZones = new List<UnitZone>() { Board.Melee1, Board.Range1, Board.Siege1, Board.Melee2, Board.Range2, Board.Siege2 };

            foreach(var zone in BoardZones)
            {
                foreach(var item in zone.InvoqueZone)
                    if(item.Rank == Rank.Silver.ToString())
                    {
                        if (item.Power == power)
                        {
                            MetodosUtiles.MoveList(item, item.Origin, item.Owner.Graveyard);
                            item.Origin = item.Owner.Graveyard;
                        }
                    }    
            }
        }
        private static void UnitZoneToDestroy()
        {            
            int M1 = Board.Melee1.InvoqueZone.Count;
            int M2 = Board.Melee2.InvoqueZone.Count;
            int R1 = Board.Range1.InvoqueZone.Count;
            int R2 = Board.Range2.InvoqueZone.Count;
            int S1 = Board.Siege1.InvoqueZone.Count;
            int S2 = Board.Siege2.InvoqueZone.Count;
            double MinValue1 = MinValueNoCero(M1,R1,S1);
            double MinValue2 = MinValueNoCero(M2,R2,S2);

            double Count = 0;

            if (MinValue1 == int.MaxValue && MinValue2 != int.MaxValue) Count = MinValue2;
            else if (MinValue1  != int.MaxValue && MinValue2 == int.MaxValue) Count = MinValue1;
            else if (MinValue1 != int.MaxValue && MinValue2 != int.MaxValue) Count = Math.Min(MinValue1, MinValue2);

            ClearLine(Count);
        }
        private static UpgradeZone CheckUpgradeZone(ICard Card)
        {
            if (Card.Origin == Card.Owner.UpgradeMelee.InvoqueZone)
                return Card.Owner.UpgradeMelee;
            if (Card.Origin == Card.Owner.UpgradeRange.InvoqueZone)
                return Card.Owner.UpgradeRange;
            if (Card.Origin == Card.Owner.UpgradeSiege.InvoqueZone)
                return Card.Owner.UpgradeSiege;

            return null;
        }
        
        private static WeatherZone CheckWeatherZone(ICard Card)
        {
            if (Card.Origin == Card.Owner.MeleeWeather.InvoqueZone)
                return Card.Owner.MeleeWeather;
            if (Card.Origin == Card.Owner.RangeWeather.InvoqueZone)
                return Card.Owner.RangeWeather;
            if (Card.Origin == Card.Owner.SiegeWeather.InvoqueZone)
                return Card.Owner.SiegeWeather;

            return null;
        }
        private static double MinValueNoCero(double a, double b, double c)
        {
            if (a == 0) a = int.MaxValue;
            if (b == 0) b = int.MaxValue;
            if (c == 0) c = int.MaxValue;
            double min = Math.Min(a, Math.Min(b, c));
            return min;
        }
        private static void ClearLine(double count)
        {
            List<UnitZone> BoardZones = new List<UnitZone>() {Board.Melee1,Board.Range1,Board.Siege1,Board.Melee2,Board.Range2,Board.Siege2};
            {
                foreach (UnitZone zone in BoardZones)
                {
                   if ((double)zone.InvoqueZone.Count == count)
                   {
                        foreach (var item in zone.InvoqueZone)
                        {
                            if(item.Rank != Rank.Gold.ToString())
                            {
                                MetodosUtiles.MoveList(item,item.Origin,item.Owner.Graveyard);
                                item.Origin = item.Owner.Graveyard;
                            }
                        }
                   }
                }
            }
        }
    }
}