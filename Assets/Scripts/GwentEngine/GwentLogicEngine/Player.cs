using System;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    public class Player
    { 
        public Board Board { get; set; }
        public Player Oponent { get; set; }
        public string Id { get; set; }
        public Deck Mazo { get; set; }
        public LeaderCard Leader { get; set; }
        public List<ICard> Hand { get; set; }
        public List<ICard> Graveyard { get; set; }
        public List<ICard> Deck { get; set; }
        public UnitZone Melee { get; set; }
        public UpgradeZone UpgradeMelee { get; set; }
        public WeatherZone MeleeWeather { get; set; }
        public UnitZone Range { get; set; }
        public UpgradeZone UpgradeRange { get; set; }
        public WeatherZone RangeWeather { get; set; }
        public UnitZone Siege { get; set; }
        public UpgradeZone UpgradeSiege { get; set; }
        public WeatherZone SiegeWeather { get; set; }

        public Player(string id, Deck mazo)
        {
            Id = id;
            Mazo = mazo;
            Leader = mazo.Leader;
            Hand = new List<ICard>();
            Graveyard = new List<ICard>();
            Deck = new List<ICard>();
        }

        public void Draw(out ICard card)
        {
            if (Hand.Count < 10 && Deck.Count > 0)
            {
                MetodosUtiles.MoveList(Deck[^1], Deck, Hand);
                card = Hand[^1];
                card.Origin = card.Owner.Hand;
            }
            else if (Hand.Count > 10 && Deck.Count > 0)
            {
                MetodosUtiles.MoveList(Deck[^1], Deck, Graveyard);
                card = Graveyard[^1];
                card.Origin = card.Owner.Graveyard;
            }
            else
            {
                card = null;
            }
            
        }

        public void PlayGame()
        {
            Deck = new List<ICard>();
            foreach (var card in Mazo.Cards)
            {
                card.Owner = this;
                Deck.Add(card);
                card.Origin = Deck;
            }
            MetodosUtiles.Shuffle(Deck);
        }
        public double GetTotalPower()
        {
            double power = 0;
            ListPower(power, Melee.InvoqueZone);
            ListPower(power, Range.InvoqueZone);
            ListPower(power, Siege.InvoqueZone);
            return power;            
        }
        private double ListPower(double power, List<ICard> cards)
        {
            foreach (var card in cards)
            {
                double cardPow = 0;
                if (card.AffectedByWeather)
                {
                    cardPow += 1;
                }
                else
                {
                    cardPow += card.Power;
                }
                if (card.AffectedByBuff)
                {
                    cardPow *= 2;
                }
                power += cardPow;
            }
            return power;
        }
    }

}

