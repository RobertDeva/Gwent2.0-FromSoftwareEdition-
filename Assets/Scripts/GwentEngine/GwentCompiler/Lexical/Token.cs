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
            Unknown,
            Number,
            Text,
            Bool,
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
            public const string Resto = "Resto"; // %
            public const string Pow = "Potence"; // ^
            public const string Sucessor = "Sucessor"; // ++
            public const string Predecessor = "Predecessor"; // --

            public const string And = "And"; // &&
            public const string Or = "Or"; // ||
            public const string Concatenation = "Concatenation"; // @
            public const string ConcatenationWithSpace = "ConcatenationWithSpace"; // @@
            public const string GreatherThan = "Greather"; // >
            public const string LessThan = "LessThan"; // <
            public const string Equal = "Equal"; // ==
            public const string Diferent = "Diferent"; // !=
            public const string GreatherEqual = "GreatherEqual"; // >=
            public const string LessEqual = "LessEqual"; // <=
            public const string Assign = "Assign"; // =
            public const string Implication = "Implication"; // =>
            public const string InnerSeparator = "InnerSeparetor"; // .
            public const string TwoPoints = "TwoPoints"; // :
            public const string ValueSeparator = "ValueSeparator"; // ,
            public const string StatementSeparator = "StatementSeparator"; // ;

            public const string OpenBracket = "OpenBracket"; // (
            public const string ClosedBracket = "ClosedBracket"; // )
            public const string OpenBraces = "OpenBraces"; // [
            public const string ClosedBraces = "ClosedBraces"; // ]
            public const string OpenCurlyBraces = "OpenCurlyBraces"; // {
            public const string ClosedCurlyBraces = "ClosedCurlyBraces"; // }

            public const string Card = "Card"; // Card
            public const string Effect = "Effect"; // Effect
            public const string Faction = "Faction"; // Faction
            public const string CardType = "CardType"; // CardTipe
            public const string Rank = "Rank"; // Rank
            public const string power = "power"; // attack
            public const string faction = "faction"; // faction
            public const string type = "type"; // type
            public const string rank = "rank"; // rank
            public const string effect = "effect"; //effect
            public const string description = "description"; // description
            public const string name = "name"; // Name
            public const string range = "range"; // positions
            public const string For = "for"; // cicle for
            public const string While = "while"; // cicle while
            public const string If = "if"; // keyword if
            public const string Else = "else"; // keyword else
            public const string In = "in"; // keyword in
            public const string String = "String"; // keyword string
            public const string Bool = "Bool"; // keyword bool
            public const string Number = "Number"; // keyword int
            public const string True = "true"; // keyword true
            public const string False = "false"; // keyword false
            public const string Params = "Params";
            public const string Source = "Source";
            public const string Selector = "Selector";
            public const string Action = "Action";
            public const string context = "context";
            public const string targets = "targets";
            public const string target = "target";
            public const string OnActivation = "OnActivation";
        }
    }
}
