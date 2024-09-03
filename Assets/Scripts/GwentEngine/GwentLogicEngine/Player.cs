using System;
using System.Collections.Generic;
using GwentEngine;

namespace GwentEngine
{
    public class Player
    { 
        public Board GetBoard { get; set; }
        public string Id { get; set; }
        public Deck Mazo { get; set; }
        public LeaderCard Leader { get; set; }
        public List<IPlayable> Hand { get; set; }
        public List<IPlayable> Graveyard { get; set; }
        public List<IPlayable> Deck { get; set; }
        public UnitZone Melee { get; set; }
        public UpgradeZone UpgradeMelee { get; set; }
        public UnitZone Range { get; set; }
        public UpgradeZone UpgradeRange { get; set; }
        public UnitZone Siege { get; set; }
        public UpgradeZone UpgradeSiege { get; set; }

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
            }
        }
    }

}

