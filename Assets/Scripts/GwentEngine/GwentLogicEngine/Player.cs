using System;
using System.Collections.Generic;

namespace GwentEngine
{
    public class Player
    { 
        public string Id { get; set; }
        public Deck Mazo { get; set; }
        public LeaderCard Leader { get; set; }
        public List<IPlayable> Hand { get; set; }
        public List<IPlayable> Graveyard { get; set; }
        public List<IPlayable> Deck { get; set; }

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
                Deck.Add(card);
            }
        }
    }

}

