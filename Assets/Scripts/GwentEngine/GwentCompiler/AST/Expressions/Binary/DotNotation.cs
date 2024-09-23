using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class DotNotation : BinaryExpression
        {
            public override ExpressionType Type { get; set; }

            public override object? Value { get; set; }

            public DotNotation(CodeLocation location) : base(location)
            {
                Type = ExpressionType.Anytype;
            }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool right = Right.CheckSemantic(context, scope, errors);
                bool left = Left.CheckSemantic(context, scope, errors);
                if (Left is Identifier)
                {
                    if (!ListOfIdentifiers.IdentifiersList.Contains(Left.Value.ToString()))
                    {
                        bool tuple = scope.AssignedIdentifier(Left.Value.ToString(), out Scope cntx);
                        if (tuple)
                        {
                            Expression expression = cntx.VarYValores[Left.Value.ToString()];
                            Left.Type = expression.Type;
                        }
                    }
                }
                List<string> CardPropertys = new List<string>() { "Name", "Power", "Owner", "Rank", "Type", "Range", "Faction" };
                if ((Right is Identifier) && CardPropertys.Contains(Right.Value.ToString()))
                {
                    if (Left.Type != ExpressionType.Card)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }

                    if ((Right is Identifier) && (Right.Value.ToString() == "Power"))
                    {
                        Type = ExpressionType.Number;
                    }
                    if ((Right is Identifier) && (Right.Value.ToString() == "Owner"))
                    {
                        Type = ExpressionType.Player;
                    }
                    else
                    {
                        Type = ExpressionType.Text;
                    }

                    return left && right;
                }
                else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.TriggerPlayer.ToString()))
                {
                    if (Left.Type != ExpressionType.Context)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }

                    Type = ExpressionType.Player;
                    return left && right;
                }
                else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Board.ToString()))
                {
                    if (Left.Type != ExpressionType.Context)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }

                    Type = ExpressionType.List;
                    return left && right;
                }
                else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Hand.ToString() || Right.Value.ToString() == IdentifierType.Deck.ToString() ||
                         Right.Value.ToString() == IdentifierType.Field.ToString() || Right.Value.ToString() == IdentifierType.Graveyard.ToString()))
                {
                    if (Left.Type != ExpressionType.Context)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }

                    Type = ExpressionType.List;
                    return left && right;
                }
                else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Owner.ToString()))
                {
                    if (Left.Type != ExpressionType.Card)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }

                    Type = ExpressionType.Player;
                    return left && right;
                }
                else if (Right.Type == ExpressionType.Method)
                {
                    if (Left.Type != ExpressionType.List && Left.Type != ExpressionType.ContextList)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a list"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }

                    Type = ExpressionType.Method;
                    return right && left;
                }
                else if (Right.Type == ExpressionType.List)
                {
                    if (Right is MethodBracket)
                    {
                        MethodBracket bracket = (MethodBracket)Right;
                        if (bracket.Right is Predicate)
                        {
                            if (Left.Type != ExpressionType.List)
                            {
                                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a list"));
                                Type = ExpressionType.ErrorType;
                                return false;
                            }
                            Type = ExpressionType.List;
                            return right && left;
                        }

                        if (Left.Type != ExpressionType.Context)
                        {
                            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                            Type = ExpressionType.ErrorType;
                            return false;
                        }

                        Type = ExpressionType.ContextList;
                        return right && left;
                    }
                }
                return false;
            }

            public override string ToString()
            {
                if (Value == null)
                {
                    return String.Format("{0}.{1}", Left, Right);
                }
                return Value.ToString();
            }

            public override void Evaluate()
            {
                Left.Evaluate();
                Right.Evaluate();


                bool Continue = false;

                if ((Left is Identifier) && (Left.Value.ToString() == "context" || (EffectExecutation.identifiers.ContainsKey(Left.Value.ToString()) && EffectExecutation.identifiers[Left.Value.ToString()].Value.ToString() == "context")))
                {
                    if (EffectExecutation.identifiers.ContainsKey(Left.Value.ToString()))
                    {
                        if (EffectExecutation.identifiers[Left.Value.ToString()].Value.ToString() == "context")
                        {
                            Left.Value = EffectExecutation.identifiers[Left.Value.ToString()];
                            Left.Evaluate();
                            Continue = true;
                        }
                    }

                    if (Left.Value.ToString() == "context") Continue = true;

                    if (Continue) // Esto es para evitar que entre con un identificador incorrecto a ejecutarse como context
                    {

                        if (Right is Identifier)
                        {
                            if (Right.Value.ToString() == "TriggerPlayer")
                            {
                                //  string triggerPlayer = EffectExecutation.VerificatePlayer();
                                //  Value = triggerPlayer;
                                return;
                            }
                            else if (Right.Value.ToString() == "Board")
                            {
                                //  EffectExecutation.BoardList();
                                //  Value = EffectExecutation.board;
                                return;
                            }
                            else if (Right.Value.ToString() == "Hand")
                            {
                                List<ICard> hand = new List<ICard>();
                                // string triggerPlayer = EffectExecutation.VerificatePlayer();

                                // if (triggerPlayer == EffectExecutation.player1.name) hand = EffectExecutation.h1;
                                // else hand = EffectCreation.h2;

                                Value = hand;
                                return;
                            }
                            else if (Right.Value.ToString() == "Deck")
                            {
                                List<ICard> deck = new List<ICard>();
                                //  string triggerPlayer = EffectExecutation.VerificatePlayer();

                                // if (triggerPlayer == EffectExecutation.player1.name) deck = EffectExecutation.deck1;
                                // else deck = EffectExecutation.deck2;


                                Value = deck;
                                return;
                            }
                            else if (Right.Value.ToString() == "Field")
                            {
                                List<ICard> field = new List<ICard>();
                                // string triggerPlayer = EffectExecutation.VerificatePlayer();

                                // field = EffectExecutation.FieldOfPlayerList(triggerPlayer);

                                Value = field;
                                return;
                            }
                            else if (Right.Value.ToString() == "Graveyard")
                            {
                                List<ICard> graveyard = new List<ICard>();
                                // string triggerPlayer = EffectExecutation.VerificatePlayer();

                                // if (triggerPlayer == EffectExecutation.player1.name) graveyard = EffectExecutation.g1;
                                // else graveyard = EffectExecutation.g2;

                                Value = graveyard;
                                return;
                            }
                        }
                        else if (Right is MethodBracket)
                        {
                            Right.Evaluate();
                            Value = Right.Value;
                            return;
                        }
                        else if (Right is Indexer) //El indexador despues del context. es complicado debido a su forma de parsear
                        {
                            Indexer index = (Indexer)Right;
                            index.Right.Evaluate();
                            double ind = (double)index.Right.Value;
                            int indexer = (int)ind;

                            if (index.Left is MethodBracket) //Parece que despues de un context. nunca viene un Find(Predicate) ni con el indexador
                            {
                                MethodBracket bracket = (MethodBracket)index.Left;

                                bracket.Evaluate();
                                index.Left.Value = bracket.Value;
                                //   Value = EffectExecutation.VerificateIndexer(indexer, (List<ICard>)index.Left.Value);
                                return;
                            }
                            else if (index.Left is Identifier)
                            {
                                //  Value = EffectExecutation.VerificateIdentifierLeftIndexer(index.Left.ToString(), indexer);
                                return;
                            }
                        }
                    }
                }
                else if (Left is DotNotation || (Left is Identifier && EffectExecutation.identifiers.ContainsKey(Left.Value.ToString())))
                {
                    if (Left is Identifier && EffectExecutation.identifiers.ContainsKey(Left.Value.ToString()))
                    {
                        Left.Value = EffectExecutation.identifiers[Left.Value.ToString()].Value;
                        Left.Evaluate();
                    }


                    if (Left.Value is List<ICard>)
                    {
                        List<ICard> list = (List<ICard>)Left.Value;
                        if (Right is MethodBracket)
                        {
                            MethodBracket bracket = (MethodBracket)Right;

                            if (bracket.Left.Value.ToString() == "Shuffle")
                            {
                                MetodosUtiles.Shuffle(list);
                                Value = list;
                                return;
                            }
                            else if (bracket.Left.Value.ToString() == "Push")
                            {
                                bracket.Right.Evaluate();
                                ICard card = (ICard)bracket.Right.Value;
                                // EffectExecutation.Push(list, card);
                                Value = list;
                                return;
                            }
                            else if (bracket.Left.Value.ToString() == "Remove")
                            {
                                bracket.Right.Evaluate();
                                ICard card = (ICard)bracket.Right.Value;
                                // EffectExecutation.Remove(list, card);
                                Value = list;
                                return;
                            }
                            else if (bracket.Left.Value.ToString() == "SendBottom")
                            {
                                bracket.Right.Evaluate();
                                ICard card = (ICard)bracket.Right.Value;
                                //  EffectExecutation.SendBottom(list, card);
                                Value = list;
                                return;
                            }
                            else if (bracket.Left.Value.ToString() == "Pop")
                            {
                                //  ICard card = ICard.Pop(list);
                                //  Value = card;
                                return;
                            }
                            else if (bracket.Left.Value.ToString() == "Find")
                            {
                                //   EffectsDictionary.predicateList = list;
                                //  Value = EffectExecutation.PredicateList(list, (Predicate)bracket.Right);
                                return;
                            }
                            else if (bracket.Left.Value.ToString() == "Add")
                            {
                                bracket.Right.Evaluate();
                                ICard card = (ICard)bracket.Right.Value;
                                // Value = EffectExecutation.Add(list, card);
                                return;
                            }
                        }
                        else if (Right is Indexer) //Solamente puede venir un Find(predicate) antes de un indexer si hay una lista en la izq
                        {
                            Indexer indexador = (Indexer)Right;
                            MethodBracket bracket = (MethodBracket)indexador.Left;
                            Predicate predicate = (Predicate)bracket.Right;

                            EffectExecutation.predicateList = list;
                            //  List<ICard> filterList = EffectExecutation.PredicateList(list, predicate);
                            indexador.Right.Evaluate();
                            int indexer = (int)indexador.Right.Value;

                            //  if (filterList.Count == 0)
                            {
                                // Value = filterList;
                                return;
                            }

                            //  if (indexer < 0 || indexer >= filterList.Count)
                            {
                                //Value = filterList;
                                return;
                            }

                            //  Value = filterList[indexer];
                            return;
                        }
                    }
                    else if (Left.Value is ICard || (Left.Value is Identifier && Left.Value.ToString() == "card"))
                    {
                        ICard card = null;
                        //Todo esto del if y else es debido a el predicado, no se verá un "card" en un lugar que no sea el right de un predicate, no se rompe la asignacion del indexer debido a que el metodo del predicate verifica que la lista no este vacia
                        if (Left.Value is Identifier && Left.Value.ToString() == "card") card = EffectExecutation.predicateList[EffectExecutation.cardIndex];
                        else card = (ICard)Left.Value;

                        if (Right is Identifier || Right.Value.ToString() == "Owner")
                        {
                            //  string property = EffectExecutation.CardPropertyString(card, Right.Value.ToString());
                            if (Right.Value.ToString() == "Power")
                            {
                                //  double power = double.Parse(property);
                                //  int value = (int)power;
                                // Value = value;
                                return;
                            }

                            //  Value = property;
                            return;
                        }
                    }
                }
            }
        }
    }
}