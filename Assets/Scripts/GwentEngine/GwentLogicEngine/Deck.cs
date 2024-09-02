using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{ 
    public class Deck
    {
        public string Id { get; }
        public Faction DeckFaction { get; set; }
        public LeaderCard Leader { get; }
        public List<IPlayable> Cards { get; set; }
       
        public Deck(string id,LeaderCard leader, List<IPlayable> cards)
        {
            Id = id;
            Leader = leader;
            DeckFaction = leader.faction;
            if(cards.Count < 25)
            {
                throw new Exception("Invalid Deck, don't have enough cards");
            }
            Cards = cards;           
        }

        public override string ToString()
        {
            return Id + " " + DeckFaction.ToString() + " " + Cards.Count.ToString();
        }
    }
}
