using System;
using System.Collections.Generic;
using UnityEngine;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Parser
        {
            public static List<CompilingError> compilingErrors = new List<CompilingError>();
            public static List<string> keywords = new List<string>() {TokenValues.Faction, TokenValues.Rank};
            public Parser(TokenStream stream)
            {
                Stream = stream;
            }
            public TokenStream Stream { get; private set; }

            public ElementalProgram ParseProgram(List<CompilingError> errors)
            {
                ElementalProgram program = new ElementalProgram(new CodeLocation());

                if (!Stream.CanLookAhead(0)) return program;

                //Here we parse all the declared effects
                while (Stream.LookAhead().Value == TokenValues.effect)
                {
                    Effect effect = ParseEffect(errors);
                    program.Effects[effect.Id] = effect;
                    if (!Stream.Next())
                    {
                        break;
                    }
                }
                //Here we parse all the declared cards
                while (Stream.LookAhead().Value == "card")
                {
                    Card card = ParseCard(errors);
                    program.Cards[card.Id] = card;

                    if (!Stream.Next())
                    {
                        break;
                    }
                }
                return program;
            }
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
                if (!Stream.Next(TokenValues.name))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "name expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if (!Stream.Next(TokenType.Text))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                }
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next(TokenValues.power))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "power expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
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
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                Faction fac = ParseFaction(errors);
                if (fac == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                    return card;
                }
                card.faction = fac;
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next(TokenValues.type))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "type expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                CardType typ = ParseCardType(errors);
                if (typ == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                    return card;
                }
                card.type = typ;
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next(TokenValues.rank))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "rank expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                Rank rank = ParseRank(errors);
                if (rank == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                    return card;
                }
                card.rank = rank;
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next(TokenValues.range))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "range expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if (!Stream.Next(TokenValues.OpenBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "[ expected"));
                }
                while (Stream.Position < Stream.count)
                {
                   if(!Stream.Next(TokenType.Text))
                   {
                        Stream.MoveNext(1);
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "text expected"));
                   }
                   else
                   card.range.Add(Stream.LookAhead().Value);
                   if(Stream.Next(TokenValues.ValueSeparator))
                   {
                   }
                   else
                   {  
                        break;
                   }
                }
                if (!Stream.Next(TokenValues.ClosedBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "] expected"));
                }
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next("description"))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "description expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if (!Stream.Next(TokenType.Text))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                }
                else
                {
                    card.description = Stream.LookAhead().Value;
                }
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                ParseOnActivation(errors, out List<CardEffect> effects);
                card.effects = effects;
                if (!Stream.Next(TokenValues.ClosedBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "] expected"));
                }
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                }
                return card;
            }

            private void ParseOnActivation(List<CompilingError> errors, out List<CardEffect> effects)
            {
                effects = new List<CardEffect>();
                if (!Stream.Next(TokenValues.OnActivation))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OnActivation expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if (!Stream.Next(TokenValues.OpenBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "[ expected"));
                }
                while (Stream.Position < Stream.count)
                {
                    Stream.MoveNext(1);
                    if (Stream.LookAhead().Value == TokenValues.ClosedBraces)
                    {
                        break; 
                    }
                    if (Stream.LookAhead().Value == TokenValues.OpenCurlyBraces)
                    {
                        if (!Stream.Next(TokenValues.Effect))
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Effect expected"));
                        }
                        else
                        effects.Add(ParseEffects(errors, false, null));
                    }
                }
                Stream.MoveBack(1);
            }
            private CardEffect ParseEffects(List<CompilingError> errors, bool isPostAct, CardEffect parent)
            {
                CardEffect effect = new CardEffect(isPostAct, Stream.LookAhead().Location);
                if(isPostAct)
                {                    
                     effect.Parent = parent;                    
                }
                
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if (Stream.Next(TokenType.Text))
                {
                    effect.Name = Stream.LookAhead().Value;
                    effect.Params = null;
                }
                else if (!Stream.Next(TokenValues.OpenCurlyBraces))
                {                    
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
                    
                    if (!Stream.Next("Name"))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Name expected"));
                    }
                    if (!Stream.Next(TokenValues.TwoPoints))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                    }
                    if (!Stream.Next(TokenType.Text))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Text expected for assignament"));
                    }
                    else
                    {
                        effect.Name = Stream.LookAhead().Value;
                    }
                    if (!Stream.Next(TokenValues.ValueSeparator))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", expected"));
                    }
                    if (Stream.LookAhead(1).Value != TokenValues.ClosedCurlyBraces)
                    {
                        while (Stream.Position < Stream.count)
                        {
                            if (Stream.Next(TokenValues.ClosedCurlyBraces))
                            {
                                break;
                            }
                            string param;
                            Expression exp;
                            if (!Stream.Next(TokenType.Identifier))
                            {
                                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Identifier expected"));
                                break;
                            }
                            else
                            {
                                param = Stream.LookAhead().Value;
                            }
                            if (!Stream.Next(TokenValues.TwoPoints))
                            {
                                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                            }
                            exp = ParseExpression();
                            if (exp == null)
                            {
                                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                            }
                            if (!Stream.Next(TokenValues.ValueSeparator))
                            {
                                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", expected"));
                            }
                            ParamValue Param;
                            Param = new(param, exp);
                            effect.Params.Add(Param);
                        }
                        Stream.MoveBack(1);
                        if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                        }
                    }
                }
                if (!Stream.Next(TokenValues.ValueSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", expected"));
                }
                if (Stream.LookAhead(1).Value == "Selector")
                {
                    Stream.MoveNext(1);
                    effect.Selector = ParseSelector(errors);

                }
                else
                {
                    if(isPostAct)
                    {
                        effect.Selector = new Selector(Stream.LookAhead().Location);
                        effect.Selector.Source = "parent";
                        effect.Selector.Single = new Bool(false, Stream.LookAhead().Location);
                        effect.Selector.Predicate = null;
                    }
                    else
                    {
                        effect.Selector = new Selector(Stream.LookAhead().Location);
                        effect.Selector.Source = "board";
                        effect.Selector.Single = new Bool(false, Stream.LookAhead().Location);
                        effect.Selector.Predicate = null;
                    }
                }
                if(Stream.LookAhead(1).Value == "PostAction")
                {
                    Stream.MoveNext(1);
                    if (!Stream.Next(TokenValues.TwoPoints))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                    }
                    if (!Stream.Next(TokenValues.OpenCurlyBraces))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
                    }
                    if (!Stream.Next("Type"))
                    {
                         errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Type expected"));
                    }
                    effect.PostAction = ParseEffects(errors, true, effect);
                }
                else
                {
                    effect.PostAction = null;
                }
                if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                }
                if (!Stream.Next(TokenValues.ValueSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", expected"));
                }
                return effect;
            }
            private Selector ParseSelector(List<CompilingError> errors)
            {
                Selector selector = new Selector(Stream.LookAhead().Location);

                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if (!Stream.Next(TokenValues.OpenCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
                }
                if (!Stream.Next(TokenValues.Source))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Source expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                if(!Stream.Next(TokenType.Text))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Invalid assignament"));
                }
                else
                {
                    selector.Source = Stream.LookAhead().Value;
                }
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next("Single"))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Single expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                Expression exp = ParseExpression();
                if (exp == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad Expression"));
                }
                else selector.Single = exp;
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next("Predicate"))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Predicate expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": expected"));
                }
                Expression pred = ParsePredicate(errors);
                if (pred == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad Expression"));
                }
                else selector.Predicate = pred;
                if (!Stream.Next(TokenValues.StatementSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
                if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                }
                if (!Stream.Next(TokenValues.ValueSeparator))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", expected"));
                }
                return selector;
            }
            private Effect ParseEffect(List<CompilingError> errors)
            {

                Effect effect = new Effect("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
                }
                if (!Stream.Next("Name")) // Name
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Name Expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints)) //:
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
                }
                if (!Stream.Next(TokenType.Text)) // Text
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Text Expected"));
                }
                else
                effect.Id = Stream.LookAhead().Value; //Se a�ade el nombre al effecto
                if (!Stream.Next(TokenValues.ValueSeparator)) // ,
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "ValueSeparator Expected"));
                }
                if (Stream.LookAhead(1).Value == TokenValues.Params) // Params
                {
                    Stream.MoveNext(1);
                    if (!Stream.Next(TokenValues.TwoPoints)) // :
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
                    }
                    if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
                    }
                    if (!Stream.Next(TokenValues.ClosedCurlyBraces)) //Si no hay una llave cerrada entra
                    {
                        ParseParams(errors, effect);
                    } // }
                    if (!Stream.Next(TokenValues.ValueSeparator)) // ,
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "ValueSeparator Expected"));
                    }
                }

                if (!Stream.Next(TokenValues.Action)) // Action
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Action Expected"));
                }
                if (!Stream.Next(TokenValues.TwoPoints)) //:
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
                }
                if (!Stream.Next(TokenValues.OpenBracket)) //(
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenBracket Expected"));
                }
                if (!Stream.Next(TokenValues.targets)) //targets
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "targets Expected"));
                }
                if (!Stream.Next(TokenValues.ValueSeparator)) // ,
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "targets Expected"));
                }
                if (!Stream.Next(TokenValues.context)) // context
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "context Expected"));
                }
                if (!Stream.Next(TokenValues.ClosedBracket)) // )
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "context Expected"));
                }

                if (!Stream.Next(TokenValues.Implication)) // =>
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "=> Expected"));
                }

                if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
                }

                if (Stream.LookAhead(1).Value != TokenValues.ClosedCurlyBraces) // Si no hay } parsea Action
                {
                    ParseAction(errors, effect.ActionList);
                    //el cierre restante despues de parsear Action
                    if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} Expected"));
                    }
                    return effect;
                }
                else
                {
                    Stream.MoveNext(1);
                    if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} Expected"));
                    }
                }

                return effect;
            }
            private bool ParseParametro(List<CompilingError> errors, Effect effect) //Parsea una variable, normalmente se llamada desde Params
            {
                string id;
                TypeOfValue typeOfValue;

                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Id expected"));
                    return false;
                }
                else
                id = Stream.LookAhead().Value;

                if (!Stream.Next(TokenValues.TwoPoints))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints expected"));
                }
                Stream.MoveNext(1);

                if (Stream.LookAhead().Value == TokenValues.Bool) typeOfValue = TypeOfValue.Bool;
                else if (Stream.LookAhead().Value == TokenValues.String) typeOfValue = TypeOfValue.String;
                else if (Stream.LookAhead().Value == TokenValues.Number) typeOfValue = TypeOfValue.Number;
                else
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bool, Number or String expected"));
                    Stream.MoveNext(1);
                    return false;
                }

                Param parametro = new Param(id, typeOfValue);
                effect.ParamsExpresions.Add(parametro);

                return true;
            }
            private void ParseAction(List<CompilingError> errors, List<ASTNode> actionList) //Parsea el Action de Effect, parece ser que hay que darle de parametro un objeto especifico para que almacene expresiones de AST
            {
                while (Stream.Position < Stream.count) //Recorre mientras hallan instrucciones
                {
                    if (Stream.Next(TokenValues.While)) //Parsea instrucciones while
                    {
                        ParseWhile(errors, actionList);
                        if (!Stream.Next(TokenValues.StatementSeparator)) //Verificar ; despues de cada instruccion
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                        }
                    }
                    else if (Stream.Next(TokenValues.For)) //Parsea instrucciones for
                    {
                        ParseFor(errors, actionList);
                        if (!Stream.Next(TokenValues.StatementSeparator)) //Verificar ; despues de cada instruccion
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                        }
                    }
                    else if (Stream.Next(TokenValues.ClosedCurlyBraces))   //Si se encuentra una } entonces se acaba el ciclo
                    {
                        break;
                    }
                    else // si no es un while o un for , entonces es una expresiom
                    {
                        Expression? exp = ParseExpression();
                        if (exp == null)
                        {
                            Stream.MoveNext(1);
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                        }
                        else actionList.Add(exp);

                        if (!Stream.Next(TokenValues.StatementSeparator)) //Verificar ; despues de cada instruccion
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                        }
                    }
                }
            }

            private Expression ParseIndexer(List<CompilingError> errors, Expression? expression)
            {
                if (expression == null) return null;

                if (Stream.LookAhead(1).Value == TokenValues.OpenBraces)
                {
                    Stream.MoveNext(1);

                    Expression? exp = ParseExpression();
                    if (exp == null)
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression in indexer"));
                    }
                    if (!Stream.Next(TokenValues.ClosedBraces))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "] expected"));
                    }
                    Indexer indexador = new Indexer(Stream.LookAhead().Location);
                    indexador.Left = expression;
                    indexador.Right = exp;
                    return indexador;
                }
                return expression;
            }
            private void ParseWhile(List<CompilingError> errors, List<ASTNode> actionList)
            {
                While While = new While(null, Stream.LookAhead().Location);

                if (!Stream.Next(TokenValues.OpenBracket))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "( expected"));
                }

                Expression? exp = ParseExpression();
                if (exp == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                }
                else While.Condition = exp;

                if (!Stream.Next(TokenValues.ClosedBracket))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
                }

                if (Stream.LookAhead(1).Value == TokenValues.OpenCurlyBraces)
                {
                    Stream.MoveNext(1);

                    ParseAction(errors, While.ActionList);
                    Stream.MoveBack(1);

                    if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                    }

                    actionList.Add(While);
                    return;
                }
                else //Si es distinto, parsea una instruccion
                {
                    Expression? exp1 = ParseExpression();
                    if (exp1 == null)
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                    }

                    if(!Stream.Next(TokenValues.StatementSeparator))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                    }
                    While.ActionList.Add(exp1);
                    actionList.Add(While);
                }
            }

            private void ParseFor(List<CompilingError> errors, List<ASTNode> actionList) //NO SE HA PUESTO LA DECLARACION DE UN SOLO ARGUMENTO
            {
                For @for = new For(Stream.LookAhead().Location);

                if (!Stream.Next(TokenValues.target))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "target expected"));
                }

                if (!Stream.Next(TokenValues.In))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "in expected"));
                }

                if (!Stream.Next(TokenValues.targets))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "targets expected"));
                }

                if (Stream.LookAhead(1).Value == TokenValues.OpenCurlyBraces)
                {
                    Stream.MoveNext(1);

                    ParseAction(errors, @for.ActionList);
                    Stream.MoveBack(1);

                    if (!Stream.Next(TokenValues.ClosedCurlyBraces))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                    }

                    actionList.Add(@for);
                    return;
                }
                else //Si es distinto, parsea una instruccion
                {
                    Expression? exp1 = ParseExpression();
                    if (exp1 == null)
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                    }
                    if (!Stream.Next(TokenValues.StatementSeparator))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                    }
                    @for.ActionList.Add(exp1);
                    actionList.Add(@for);
                }
            }
            private void ParseParams(List<CompilingError> errors, Effect effect) //Parsea el interior de Params
            {
                Stream.MoveBack(1);
                while (ParseParametro(errors, effect))
                {
                    if (!Stream.Next(TokenValues.ValueSeparator))
                    {
                        
                        if (!Stream.Next(TokenValues.ClosedCurlyBraces)) //aumentar
                        {
                            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
                            Stream.MoveNext(1);
                        }

                        break;
                    }
                }
            }
            public Rank ParseRank(List<CompilingError> errors)
            {
                Rank rank = new Rank("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                else rank.Value = Stream.LookAhead().Value;
                return rank;
            }
            public CardType ParseCardType(List<CompilingError> errors)
            {
                CardType type = new CardType("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                else type.Value = Stream.LookAhead().Value;
                return type;
            }
            public Faction ParseFaction(List<CompilingError> errors)
            {
                Faction faction = new Faction("null", Stream.LookAhead().Location);
                if (!Stream.Next(TokenType.Identifier))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "id expected"));
                }
                else faction.Value = Stream.LookAhead().Value;
                return faction;
            }
            private Expression? ParseExpression()
            {
                Expression exp = ParseExpressionLv0(null);
                return exp;
            }
            private Expression? ParseExpressionLv0(Expression? left)
            {
                Expression? newleft = ParseExpressionLv01(left);
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
                exp = ParseAssign(left);
                if (exp != null)
                {
                    return exp;
                }
                return left;   
            }

            private Expression? ParseExpressionLv01(Expression? left)
            {
                Expression? newleft = ParseExpressionLv02(left);
                return ParseExpressionLv01_(newleft);
            }

            private Expression? ParseExpressionLv01_(Expression? left)
            {
                Expression exp = ParseEqual(left);
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

            private Expression? ParseExpressionLv02(Expression? left)
            {
                Expression? newleft = ParseExpressionLv1(left);
                return ParseExpressionLv02_(newleft);
            }

            private Expression? ParseExpressionLv02_(Expression? left)
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
                exp = ParseDotNotation(left);
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
                exp = ParseResto(left);
                if (exp != null)
                {
                    return exp;
                }
                return left;
            }

            private Expression? ParseExpressionLv3(Expression? left)
            {

                Expression? exp = ParseInBrackectExpression();
                if (exp != null)
                {
                    return exp;
                }
                exp = ParseNumber();
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
                exp = ParseIdentifierKeyWord();
                if(exp != null)
                {
                    exp = ParseIndexer(compilingErrors, exp);
                    exp = ParseBracket(compilingErrors, exp);
                    return exp;
                }
                exp = ParseIdentifier();
                if (exp != null)
                {
                    exp = ParseIndexer(compilingErrors, exp);
                    exp = ParseBracket(compilingErrors, exp);                    
                    return exp;   
                }
                return left;
            }

            private Expression ParseBracket(List<CompilingError> errors, Expression expression)
            {
                if (Stream.LookAhead(1).Value == TokenValues.OpenBracket)
                {
                    Stream.MoveNext(1);

                    MethodBracket bracket = new (Stream.LookAhead().Location);
                    Expression exp = ParseExpression();
                    if (exp == null)
                    {
                        if (Stream.LookAhead(1).Value == TokenValues.ClosedBracket)
                        {
                            Stream.MoveNext(1);
                            bracket.Left = expression;
                            bracket.Right = null;
                            return bracket;
                        }

                        exp = ParsePredicate(compilingErrors);
                        if (exp == null)
                        {
                            return null;
                        }
                    }

                    if (!Stream.Next(TokenValues.ClosedBracket))
                    {
                        errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
                    }

                    bracket.Left = expression;
                    bracket.Right = exp;
                    return bracket;

                }
                return expression;
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

                return ParseExpressionLv02_(great);
            }

            private Expression? ParseLess(Expression? left)
            {
                Less less = new Less(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.LessThan))
                    return null;

                less.Left = left;

                Expression? right = ParseExpressionLv02(null);
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

                return ParseExpressionLv02_(greatEq);
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

                return ParseExpressionLv02_(lessEq);
            }

            private Expression? ParseEqual(Expression? left)
            {
                Equal eq = new Equal(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Equal))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv02(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv01_(eq);
            }

            private Expression? ParseDiferent(Expression? left)
            {
                Diferent eq = new Diferent(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Diferent))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv02(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv01_(eq);
            }

            private Expression? ParseOr(Expression? left)
            {
                Or eq = new Or(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Or))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv02(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                eq.Right = right;

                return ParseExpressionLv0_(eq);
            }
            private Expression? ParseAssign(Expression? left)
            {
                Assign assign = new Assign(Stream.LookAhead().Location);
                if (left == null || !Stream.Next(TokenValues.Assign))
                {
                    return null;
                }
                assign.Left = left;
                Expression? right = ParseExpressionLv01(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                assign.Right = right;

                return ParseExpressionLv01_(assign);
            }

            private Expression? ParseAnd(Expression? left)
            {
                And eq = new And(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.And))
                    return null;

                eq.Left = left;

                Expression? right = ParseExpressionLv02(null);
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
            private Expression? ParseDotNotation(Expression? left)
            {
                DotNotation dotNotation = new DotNotation(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.InnerSeparator))
                    return null;

                dotNotation.Left = left;

                Expression? right = ParseExpressionLv2(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                dotNotation.Right = right;
                Debug.Log(dotNotation.ToString());
                return ParseExpressionLv1_(dotNotation);
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

            private Expression ParsePredicate(List<CompilingError> errors)
            {
                if (!Stream.Next(TokenValues.OpenBracket))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "( expected"));
                }

                Expression? exp = ParseExpression();
                if (exp == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                }

                if (!Stream.Next(TokenValues.ClosedBracket))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
                }

                if (!Stream.Next(TokenValues.Implication))
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "=> expected"));
                }

                Expression? exp1 = ParseExpression();
                if (exp == null)
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                }

                Predicate predicate = new (Stream.LookAhead().Location);
                predicate.Left = exp;
                predicate.Right = exp1;
                if (predicate.Left == null || predicate.Right == null)
                {
                    return null;
                }
                return predicate;
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
            private Expression? ParseResto(Expression? left)
            {
                Resto resto = new Resto(Stream.LookAhead().Location);

                if (left == null || !Stream.Next(TokenValues.Resto))
                    return null;

                resto.Left = left;

                Expression? right = ParseExpressionLv3(null);
                if (right == null)
                {
                    Stream.MoveBack(2);
                    return null;
                }
                resto.Right = right;

                return ParseExpressionLv2_(resto);
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
                if (!Stream.Next(TokenType.Keyword))  return null;
                if (Stream.LookAhead().Value == TokenValues.True || Stream.LookAhead().Value == TokenValues.False)
                    return new Bool(bool.Parse(Stream.LookAhead().Value), Stream.LookAhead().Location);
                else return null;
            }
            private Expression? ParseIdentifier()
            {
                if (!Stream.Next(TokenType.Identifier)) return null;
                return new Identifier(Stream.LookAhead().Value, Stream.LookAhead().Location);
            }
            private Expression? ParseIdentifierKeyWord()
            {
                if (Stream.Next("Type") || Stream.Next("Name") || Stream.Next("Faction") || Stream.Next("Power") || Stream.Next("Rank") || Stream.Next("Range"))
                {
                    return new Keyword(Stream.LookAhead().Value, Stream.LookAhead().Location);
                }
                else if(Stream.Next(TokenType.Keyword) && keywords.Contains(Stream.LookAhead().Value))
                {
                    return new Keyword(Stream.LookAhead().Value, Stream.LookAhead().Location);
                }
                return null;
            }
            private Expression? ParseInBrackectExpression()
            {
                InBracketExpression inBracket = new InBracketExpression(null,Stream.LookAhead().Location);
                if(!Stream.Next(TokenValues.OpenBracket))
                    return null;

                inBracket.InnerExpression = ParseExpression();
                if(inBracket.InnerExpression == null)
                    return null;
                if (!Stream.Next(TokenValues.ClosedBracket))
                    compilingErrors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
                return inBracket;
            }
        }
    }
}


