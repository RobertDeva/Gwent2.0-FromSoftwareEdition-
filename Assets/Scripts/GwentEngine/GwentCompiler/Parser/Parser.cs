using System;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Parser
        {
            public Parser(TokenStream stream)
            {
                Stream = stream;
            }
            public TokenStream Stream { get; private set; }

            public Card ParseCard(List<CompilingError> errors)
            {
                Card card = new Card("null", Stream.LookAhead().Location);

                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                else
                {
                    card.Id = Stream.LookAhead().Value;
                }
                if (!Stream.Next(TokenValues.OpenCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
                }
                if (!Stream.Next(TokenValues.Name))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Name expected"));
                }
                if (!Stream.Next(TokenValues.Assign))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= expected"));
                }
                if (!Stream.Next(TokenType.Text))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                }
                if (!Stream.Next(TokenValues.power))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "power expected"));
                }
                if (!Stream.Next(TokenValues.Assign))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= expected"));
                }

                /* Here we parse the expression. If null is returned, we send an error */
                Expression? exp = ParseExpression();
                if (exp == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                    return card;
                }
                card.power = exp;

                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }

                if(!Stream.Next(TokenValues.faction))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "faction expected"));
                }
                if (!Stream.Next(TokenValues.Assign))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= expected"));
                }
                Faction fac = ParseFaction(errors);
                if (fac == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                }
                card.faction = fac;
                if (!Stream.Next(TokenValues.type))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "type expected"));
                }
                if (!Stream.Next(TokenValues.Assign))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= expected"));
                }
                CardType typ = ParseCardType(errors);
                if (typ == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament")); 
                }
                card.type = typ;
                if (!Stream.Next(TokenValues.rank))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "rank expected"));
                }
                if (!Stream.Next(TokenValues.Assign))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= expected"));
                }
                Rank rank = ParseRank(errors);
                if (rank == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                }
                card.rank = rank;
                if (!Stream.Next(TokenValues.range))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "range expected"));
                }
                if (!Stream.Next(TokenValues.Assign))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= expected"));
                }
                if (!Stream.Next(TokenValues.OpenCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
                }
                while (Stream.Next(TokenType.Identifier) || Stream.Next(TokenValues.ValueSeparator))
                {
                   if(Stream.Next(TokenValues.ValueSeparator))
                   {
                        if (!Stream.Next(TokenType.Identifier))
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                        }
                   }
                }
                if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                }
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ";  expected"));
                }

                //OnActivation aqui

                //
                if(!Stream.Next(TokenValues.ClosedCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                }
                return card;
            }
            public Effect ParseEffect(List<CompilingError> errors)
            {
                throw new NotImplementedException();
            }
            public Rank ParseRank(List<CompilingError> errors)
            {
                Rank rank = new Rank("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                rank.Value = Stream.LookAhead().Value;
                return rank;
            }
            public CardType ParseCardType(List<CompilingError> errors)
            {
                CardType type = new CardType("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                type.Value = Stream.LookAhead().Value;
                return type;
            }
            public Faction ParseFaction(List<CompilingError> errors)
            {
                Faction faction = new Faction("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                faction.Value = Stream.LookAhead().Value;
                return faction;
            }

            private Expression? ParseExpression()
            {
                return ParseExpressionLv0(null);
            }

            private Expression? ParseExpressionLv05(Expression? left)
            {
                Expression? newleft = ParseExpressionLv1(left);
                return ParseExpressionLv05_(newleft);                
            }
            
            private Expression? ParseExpressionLv05_(Expression? left)
            {
                Expression? exp = ParseGreather(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseGreatherEqual(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseLess(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseLessEqual(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseGreatherEqual(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseEqual(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseDiferent(left);
                if (exp != null)
                {
                    return exp;
                }
                return left;
            }

            private Expression? ParseExpressionLv0(Expression? left)
            {
                Expression? newleft = ParseExpressionLv05(left);
                Expression? exp = ParseExpressionLv0_(newleft);
                return exp;
            }

            private Expression? ParseExpressionLv0_(Expression? left)
            {
                Expression? exp = ParseOr(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseAnd(left);
                if (exp != null)
                {
                    return exp;
                }
                return left;   
            }

            private Expression? ParseExpressionLv1(Expression? left)
            {
                Expression? newLeft = ParseExpressionLv2(left);
                return ParseExpressionLv1_(newLeft);
            }

            private Expression? ParseExpressionLv1_(Expression? left)
            {
                Expression? exp = ParseAdd(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseSub(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseConcat(left);
                if(exp != null)
                {
                    return exp;
                }
                exp = ParseConcatWS(left);
                if (exp != null)
                {
                    return exp;
                }
                return left;
            }
            
            private Expression? ParseExpressionLv2(Expression? left)
            {
                Expression? newLeft = ParseExpressionLv3(left);
                return ParseExpressionLv2_(newLeft);
            }
            private Expression? ParseExpressionLv2_(Expression? left)
            {
                Expression? exp = ParseMul(left);
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseDiv(left);
                if (exp != null)
                {
                    return exp;
                }
                return left;
            }

            private Expression? ParseExpressionLv3(Expression? left)
            {
                /*if(Stream.Next(TokenValues.OpenBracket) || Stream.Next(TokenValues.ClosedBracket))
                {
                    Expression? bracket = ParseInBrackectExpression(left);
                    return bracket;
                }*/
                Expression? exp = ParseNumber();
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseText();
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseBool();
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseIdentifier();
                if (exp != null)
                {
                    return exp;
                }
                return null;
            }

            private Expression? ParseGreather(Expression? left)
            {
                Greather great = new Greather(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.GreatherThan))
                    return null;

                great.Left = left;

                Expression? right = ParseExpressionLv1(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                great.Right = right;

                return ParseExpressionLv05_(great);
            }

            private Expression? ParseLess(Expression? left)
            {
                Less less = new Less(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.LessThan))
                    return null;

                less.Left = left;

                Expression? right = ParseExpressionLv05(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                less.Right = right;

                return ParseExpressionLv0_(less);
            }

            private Expression? ParseGreatherEqual(Expression? left)
            {
                GreatherEqual greatEq = new GreatherEqual(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.GreatherEqual))
                    return null;

                greatEq.Left = left;

                Expression? right = ParseExpressionLv1(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                greatEq.Right = right;

                return ParseExpressionLv05_(greatEq);
            }

            private Expression? ParseLessEqual(Expression? left)
            {
                LessEqual lessEq = new LessEqual(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.LessEqual))
                    return null;

                lessEq.Left = left;

                Expression? right = ParseExpressionLv1(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                lessEq.Right = right;

                return ParseExpressionLv05_(lessEq);
            }

            private Expression? ParseEqual(Expression? left)
            {
                Equal eq = new Equal(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Equal))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv1(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv05_(eq);
            }

            private Expression? ParseDiferent(Expression? left)
            {
                Diferent eq = new Diferent(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Diferent))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv1(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv05_(eq);
            }

            private Expression? ParseOr(Expression? left)
            {
                Or eq = new Or(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Or))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv05(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv0_(eq);
            }

            private Expression? ParseAnd(Expression? left)
            {
                And eq = new And(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.And))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv05(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv0_(eq);
            }
            private Expression? ParseAdd(Expression? left)
            {
                Add sum = new Add(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Add))
                    return null;

                sum.Left = left;

                Expression? right = ParseExpressionLv2(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                sum.Right = right;

                return ParseExpressionLv1_(sum);
            }

            private Expression? ParseSub(Expression? left)
            {
                Sub sub = new Sub(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Sub))
                    return null;

                sub.Left = left;

                Expression? right = ParseExpressionLv2(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                sub.Right = right;

                return ParseExpressionLv1_(sub);
            }

            private Expression? ParseConcat(Expression? left)
            {
                Concatenation concat = new Concatenation(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Concatenation))
                    return null;

                concat.Left = left;

                Expression? right = ParseExpressionLv2(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                concat.Right = right;

                return ParseExpressionLv1_(concat);
            }

            private Expression? ParseConcatWS(Expression? left)
            {
                ConcatenationWS concatws = new ConcatenationWS(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.ConcatenationWithSpace))
                    return null;

                concatws.Left = left;

                Expression? right = ParseExpressionLv2(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                concatws.Right = right;

                return ParseExpressionLv1_(concatws);
            }

            private Expression? ParseMul(Expression? left)
            {
                Mul mul = new Mul(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Mul))
                    return null;

                mul.Left = left;

                Expression? right = ParseExpressionLv3(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                mul.Right = right;

                return ParseExpressionLv2_(mul);
            }

            private Expression? ParseDiv(Expression? left)
            {
                Div div = new Div(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Div))
                    return null;

                div.Left = left;

                Expression? right = ParseExpressionLv3(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                div.Right = right;

                return ParseExpressionLv2_(div);
            }

            private Expression? ParseNumber()
            {
                if (!Stream.Next(TokenType.Number))
                    return null;
                return new Number(double.Parse(Stream.LookAhead().Value), Stream.LookAhead().Location);
            }

            private Expression? ParseText()
            {
                if (!Stream.Next(TokenType.Text))
                    return null;
                return new Text(Stream.LookAhead().Value, Stream.LookAhead().Location);
            }

            private Expression? ParseBool()
            {
                if (!Stream.Next(TokenValues.True) || !Stream.Next(TokenValues.False))
                     return null;
                return new Bool(Stream.LookAhead().Value, Stream.LookAhead().Location);
            }

            private Expression? ParseIdentifier()
            {
                if (!Stream.Next(TokenType.Identifier) )
                    return null;
                return new Identifier(Stream.LookAhead().Value, Stream.LookAhead().Location);
            }
            /*private Expression? ParseInBrackectExpression(Expression? left)
            {
                if (Stream.Next(TokenValues.ClosedBracket))
                    return null;
                return ParseExpressionLv0(left);
            }*/
        }
    }
}


