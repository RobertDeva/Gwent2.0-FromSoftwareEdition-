using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class ListOfIdentifiers
        {
            public static List<string> IdentifiersList = new List<string>()
            {
                "Power","Name","Type","Faction","Range","TriggerPlayer","Board","Hand", "Deck",
                "Field","Graveyard","Owner"
            };
        }
        public enum ExpressionType
        {
            Anytype,
            Text,
            Number,
            Bool,
            Identifier,
            List,
            Card,
            CardProperty,
            Method,
            Context,
            Player,
            ContextList,
            ErrorType
        }
        public enum IdentifierType
        {
            context,
            TriggerPlayer,
            Board,
            HandOfPlayer,
            FieldOfPlayer,
            GraveyardOfPlayer,
            DeckOfPlayer,
            Hand,
            Field,
            Deck,
            Graveyard,
            Owner,
            Find,
            Push,
            SendBottom,
            Pop,
            Remove,
            Shuffle,
            Type,
            Name,
            Faction,
            Power,
            Range
        }
    }
}