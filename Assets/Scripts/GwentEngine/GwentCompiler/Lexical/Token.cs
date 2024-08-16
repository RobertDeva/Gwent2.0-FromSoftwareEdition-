using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Token
        {
            public string Value { get; private set; }
            public TokenType Type { get; private set; }
            public CodeLocation Location { get; private set; }
            public Token(TokenType type, string value, CodeLocation location)
            {
                this.Type = type;
                this.Value = value;
                this.Location = location;
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", Type, Value);
            }
        }

        public struct CodeLocation
        {
            public string File;
            public int Line;
            public int Column;
        }


        public enum TokenType
        {
            Unknwon,
            Number,
            Text,
            Keyword,
            Identifier,
            Symbol
        }
        public class TokenValues
        {
            protected TokenValues() { }

            public const string Add = "Addition"; // +
            public const string Sub = "Subtract"; // -
            public const string Mul = "Multiplication"; // *
            public const string Div = "Division"; // /
            public const string Pow = "Potence"; // ^
            public const string Sucessor = "Sucessor"; // ++
            public const string Predecessor = "Predecessor"; // --

            public const string And = "And"; // &&
            public const string Or = "Or"; // ||
            public const string GreatherThan = "GreatherThan"; // >
            public const string LessThan = "LessThan"; // <
            public const string Equal = "Equal"; // ==
            public const string GreatherEqual = "GreatherEqual"; // >=
            public const string LessEqual = "LessEqual"; // <=
            public const string Assign = "Assign"; // =
            public const string ValueSeparator = "ValueSeparator"; // ,
            public const string StatementSeparator = "StatementSeparator"; // ;

            public const string OpenBracket = "OpenBracket"; // (
            public const string ClosedBracket = "ClosedBracket"; // )
            public const string OpenCurlyBraces = "OpenCurlyBraces"; // {
            public const string ClosedCurlyBraces = "ClosedCurlyBraces"; // }

            public const string Card = "Card"; // Card
            public const string Effect = "Effect"; // Effect
            public const string Faction = "Faction"; // Faction
            public const string CardTipe = "CardTipe"; // CardTipe
            public const string Position = "Position"; // Position
            public const string Rank = "Rank"; // Rank
            public const string attack = "attack"; // attack
            public const string description = "description"; // description
            public const string positions = "positions"; // positions
        }
    }
}
