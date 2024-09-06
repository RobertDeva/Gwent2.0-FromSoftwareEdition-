using System;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    public class Player
    { 
        public Board GetBoard { get; set; }
        public Player Oponent { get; set; }
        public string Id { get; set; }
        public Deck Mazo { get; set; }
        public LeaderCard Leader { get; set; }
        public List<IPlayable> Hand { get; set; }
        public List<IPlayable> Graveyard { get; set; }
        public List<IPlayable> Deck { get; set; }
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
            Hand = new List<IPlayable>();
            Graveyard = new List<IPlayable>();
            Deck = new List<IPlayable>();
            foreach (var card in mazo.Cards)
            { 
                card.Owner = this;
                Deck.Add(card);
                card.Origin = Deck;
            }
        }

        public void Draw()
        {
            if(Hand.Count < 10 && Deck.Count > 0)
            {
                MetodosUtiles.MoveList(Deck[Deck.Count - 1],Deck,Hand);
            }
            else if(Hand.Count > 10 && Deck.Count > 0)
            {
                MetodosUtiles.MoveList(Deck[Deck.Count - 1],Deck,Graveyard);
            }
        }
    }

}

