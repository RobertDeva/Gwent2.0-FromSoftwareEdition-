using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class EffectExecutation
        {
            public static Dictionary<string, Expression> identifiers = new Dictionary<string, Expression>();
            public static bool card;
            public static int cardIndex = 0;
            public static List<ICard> predicateList;

            public static List<ICard> board;
            public static void AddCompilerEffect(Dictionary<string, Effect> effects)
            {
                foreach (var effect in effects)
                {
                  EffectsDictionary.EffectDictionary.Add(effect.Key, ApplyEffect);
                }
            }
            public static void ApplyEffect(ICard Card)
            {
                Card card = (Card)Card;
                List<CardEffect> onActivation = card.effects;

                if (onActivation.Count > 0)
                {
                    for (int i = 0; i < onActivation.Count; i++)
                    {
                        ActivateEffect(card, onActivation[i]);
                    }
                }
            }
            public static void ActivateEffect(Card card, CardEffect cardEffect)
            {
                Dictionary<string, Effect> effects = Effects.CompilatedEffects; //Se llama al diccionario de los effectos

                string effectName = cardEffect.Name; //Se guarda el nombre del effecto que tenia la carta

                Effect effect = effects[effectName]; //Se llama al effecto especifico que la carta llamó
                

                List<ParamValue> paramsList = cardEffect.Params;
                foreach (ParamValue parametroValor in paramsList)
                {
                    parametroValor.Expression.Evaluate();
                    identifiers[parametroValor.Id] = parametroValor.Expression;
                }
                if (cardEffect.Selector != null) //Entra si el selector no es nulo
                {
                    List<ICard> result = new List<ICard>(); //Lista que se le enviará al effecto original para targets
                    string source = cardEffect.Selector.Source;

                    List<ICard> sourceList = SourceList(source); //Se guarda la lista que llamó el source de selector
                    List<ICard> predicateList = PredicateList(sourceList,(Predicate)cardEffect.Selector.Predicate); //Se filtra la lista anterior mediante el predicado

                    cardEffect.Selector.Single.Evaluate();
                    bool single = (bool)cardEffect.Selector.Single.Value;

                    if (predicateList.Count != 0)
                    {
                        if (single) result.Add(predicateList[0]);
                        else result = predicateList;
                    }

                    effect.targets = result;
                    //Hasta aqui, el efecto tiene un selector y se llenó la lista de listSelector (targets) del efecto
                }


                List<ASTNode> effectInstructions = effect.ActionList;

                foreach (ASTNode instruction in effectInstructions)
                {
                    if (instruction is Expression)
                    {
                        Expression expression = (Expression)instruction;
                        expression.Evaluate();
                    }
                    else if (instruction is While)
                    {
                        NewWhile((While)instruction, effect.targets);
                    }
                    else if (instruction is For)
                    {
                        NewFor((For)instruction, effect.targets);
                    }
                }

                if(cardEffect.PostAction != null)
                {
                    ActivateEffect(card, cardEffect.PostAction);
                }
            }

            public static List<ICard> SourceList(string source)
            {
                Player player = VerificatePlayer();

                if (source == "board")
                {
                    BoardList();
                    return board;
                }
                else if (source == "hand")
                {
                    return player.Hand;
                }
                else if (source == "otherHand")
                {
                    return player.Oponent.Hand;
                }
                else if (source == "deck")
                {
                    return player.Deck;
                }
                else if (source == "otherDeck")
                {
                    return player.Oponent.Deck;
                }
                else if (source == "field")
                {
                   return FieldOfPlayerList(player);
                }
                else if (source == "otherField")
                {
                    return FieldOfPlayerList(player.Oponent);
                }

                return null;
            }

            public static void BoardList()
            {
                board.Clear();
                List<ICard>[] lists = new List<ICard>[6]
                {Board.Melee1.InvoqueZone,Board.Melee2.InvoqueZone,Board.Range1.InvoqueZone,Board.Range2.InvoqueZone,Board.Siege1.InvoqueZone,Board.Siege2.InvoqueZone};

                foreach (List<ICard> list in lists)
                {
                    foreach (ICard card in list)
                    {
                        board.Add(card);
                    }
                }
            }

            public static List<ICard> FieldOfPlayerList(Player player)
            {
                List<ICard> fieldList = new List<ICard>();
                foreach (var card in player.Melee.InvoqueZone)
                {
                    fieldList.Add(card);
                }
                foreach (var card in player.Range.InvoqueZone)
                {
                    fieldList.Add(card);
                }
                foreach (var card in player.Siege.InvoqueZone)
                {
                    fieldList.Add(card);
                }
                return fieldList;
            }

            public static Player VerificatePlayer()
            {
                GameManager manager = GameObject.Find("Scorer").GetComponent<GameManager>();
                if (manager.Player1.GetComponent<GwentPlayer>().PlayerTurn)
                {
                    return manager.Player1.GetComponent<GwentPlayer>().player;
                }
                else return manager.Player2.GetComponent<GwentPlayer>().player;
            }
            public static ICard VerificateIndexer(int indexer, List<ICard> list)
            {
                if (indexer >= list.Count)
                {
                    if (list.Count == 0)
                    {
                        return null;
                    }
                    indexer = list.Count - 1;
                }
                else if (indexer < 0)
                {
                    if (list.Count == 0)
                    {
                        return null;
                    }
                    indexer = 0;
                }
                return list[indexer];
            }

            public static ICard VerificateIdentifierLeftIndexer(string identifier, int indexer)
            {
                List<ICard> list = new List<ICard>();
                ICard card;

                if (identifier == "Board")
                {
                    BoardList();
                    list = board;
                }
                else if (identifier == "Hand")
                {
                    Player triggerPlayer = VerificatePlayer();

                    list = triggerPlayer.Hand;
                }
                else if (identifier == "Deck")
                {
                    Player triggerPlayer = VerificatePlayer();

                    list = triggerPlayer.Deck;
                }
                else if (identifier == "Graveyard")
                {
                    Player triggerPlayer = VerificatePlayer();

                    list = triggerPlayer.Graveyard;
                }
                else if (identifier == "Field")
                {
                    Player triggerPlayer = VerificatePlayer();

                    list = FieldOfPlayerList(triggerPlayer);
                }

                card = VerificateIndexer(indexer, list);
                return card;
            }

            public static void Push(List<ICard> list, ICard card)
            {
                if (!list.Contains(card)) list.Add(card);
                else
                {
                    int index = list.IndexOf(card);
                    ICard lastElement = list[list.Count];
                    list[list.Count] = card;
                    list[index] = lastElement;
                }
            }

            public static List<ICard> Add(List<ICard> list, ICard card)
            {
               list.Add(card);
               return list;
            }

            public static void Remove(List<ICard> list, ICard card)
            {
                if (list.Contains(card)) list.Remove(card);
            }

            public static void SendBottom(List<ICard> list, ICard card)
            {
                if (!list.Contains(card))
                {
                    if (list.Count == 0) list.Add(card);
                    else list.Insert(0, card);
                }
                else
                {
                    int index = list.IndexOf(card);
                    ICard firstElement = list[0];
                    list[0] = card;
                    list[index] = firstElement;
                }
            }

            public static ICard Pop(List<ICard> list)
            {
                ICard card;

                if (list.Count != 0)
                {
                    card = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                    return card;
                }
               
                return null;
            }

            public static object CardPropertyString(ICard card, string property)
            {
                if (property == "Type") return card.Type;
                else if (property == "Name") return card.Name;
                else if (property == "Faction") return card.Faction;
                else if (property == "Power") return card.Power;
                else if (property == "Range") return card.Range;
                else if (property == "Owner") return card.Owner;
                return "error";
            }

            public static List<ICard> PredicateList(List<ICard> list, Predicate predicate)
            {
                if (list.Count != 0)
                {
                    bool condition = false;
                    List<ICard> resultList = new List<ICard>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        cardIndex = i;
                        predicate.Right.Evaluate();
                        if (predicate.Right.Value == null) condition = false;
                        else condition = (bool)predicate.Right.Value;

                        if (condition)
                        {
                            resultList.Add(list[i]);
                        }
                    }
                    return resultList;
                }
                return list;
            }


            public static void NewWhile(While While, List<ICard> targets)
            {
                Expression conditionExp = While.Condition;
                List<ASTNode> instructions = While.ActionList;

                conditionExp.Evaluate();
                bool condition = (bool)conditionExp.Value;

                while (condition)
                {
                    foreach (ASTNode instruction in instructions)
                    {
                        if (instruction is Expression)
                        {
                            Expression expression = (Expression)instruction;
                            expression.Evaluate();
                        }
                        else if (instruction is While)
                        {
                            NewWhile((While)instruction, targets);
                        }
                        else if (instruction is For)
                        {
                            NewFor((For)instruction, targets);
                        }

                        conditionExp.Evaluate();
                        condition = (bool)conditionExp.Value;
                    }
                }
            }
            public static void NewFor(For @for, List<ICard> targets)
            {
                List<ASTNode> instructions = @for.ActionList;
                foreach (var target in targets)
                {
                    foreach (ASTNode instruction in instructions)
                    {
                        if (instruction is Expression)
                        {
                            Expression expression = (Expression)instruction;
                            expression.Evaluate();
                        }
                        else if (instruction is While)
                        {
                            NewWhile((While)instruction, targets);
                        }
                        else if (instruction is For)
                        {
                            NewFor((For)instruction, targets);
                        }
                    }
                }
            }
        }
    }
}
