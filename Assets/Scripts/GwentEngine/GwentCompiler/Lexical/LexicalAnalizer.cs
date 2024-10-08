using System.Collections;
using System.Collections.Generic;


namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Compiling
        {
            private static LexicalAnalyzer? __LexicalProcess;
            public static LexicalAnalyzer Lexical
            {
                get
                {
                    if (__LexicalProcess == null)
                    {
                        __LexicalProcess = new LexicalAnalyzer();


                        __LexicalProcess.RegisterOperator("+", TokenValues.Add);
                        __LexicalProcess.RegisterOperator("*", TokenValues.Mul);
                        __LexicalProcess.RegisterOperator("-", TokenValues.Sub);
                        __LexicalProcess.RegisterOperator("/", TokenValues.Div);
                        __LexicalProcess.RegisterOperator("%", TokenValues.Resto);
                        __LexicalProcess.RegisterOperator("^", TokenValues.Pow);
                        __LexicalProcess.RegisterOperator("++", TokenValues.Sucessor);
                        __LexicalProcess.RegisterOperator("--", TokenValues.Predecessor);
                        __LexicalProcess.RegisterOperator("@", TokenValues.Concatenation);
                        __LexicalProcess.RegisterOperator("@@", TokenValues.ConcatenationWithSpace);
                        __LexicalProcess.RegisterOperator("&&", TokenValues.And);
                        __LexicalProcess.RegisterOperator("||", TokenValues.Or);
                        __LexicalProcess.RegisterOperator("=", TokenValues.Assign);
                        __LexicalProcess.RegisterOperator("==", TokenValues.Equal);
                        __LexicalProcess.RegisterOperator("!=", TokenValues.Diferent);
                        __LexicalProcess.RegisterOperator(">", TokenValues.GreatherThan);
                        __LexicalProcess.RegisterOperator("<", TokenValues.LessThan);
                        __LexicalProcess.RegisterOperator(">=", TokenValues.GreatherEqual);
                        __LexicalProcess.RegisterOperator("<=", TokenValues.LessEqual);
                        __LexicalProcess.RegisterOperator("=>", TokenValues.Implication);
                        __LexicalProcess.RegisterOperator(".", TokenValues.InnerSeparator);


                        __LexicalProcess.RegisterOperator(".", TokenValues.InnerSeparator);
                        __LexicalProcess.RegisterOperator(":", TokenValues.TwoPoints);
                        __LexicalProcess.RegisterOperator(",", TokenValues.ValueSeparator);
                        __LexicalProcess.RegisterOperator(";", TokenValues.StatementSeparator);
                        __LexicalProcess.RegisterOperator("(", TokenValues.OpenBracket);
                        __LexicalProcess.RegisterOperator(")", TokenValues.ClosedBracket);
                        __LexicalProcess.RegisterOperator("[", TokenValues.OpenBraces);
                        __LexicalProcess.RegisterOperator("]", TokenValues.ClosedBraces);
                        __LexicalProcess.RegisterOperator("{", TokenValues.OpenCurlyBraces);
                        __LexicalProcess.RegisterOperator("}", TokenValues.ClosedCurlyBraces);



                        /*  */
                        __LexicalProcess.RegisterText("\"", "\"");

                        __LexicalProcess.RegisterKeyword("Card", TokenValues.Card);
                        __LexicalProcess.RegisterKeyword("name", TokenValues.name);
                        __LexicalProcess.RegisterKeyword("Effect", TokenValues.Effect);
                        __LexicalProcess.RegisterKeyword("power", TokenValues.power);
                        __LexicalProcess.RegisterKeyword("true", TokenValues.True);
                        __LexicalProcess.RegisterKeyword("false", TokenValues.False);
                        __LexicalProcess.RegisterKeyword("range", TokenValues.range);
                        __LexicalProcess.RegisterKeyword("description", TokenValues.description);
                        __LexicalProcess.RegisterKeyword("effect", TokenValues.effect);
                        __LexicalProcess.RegisterKeyword("faction", TokenValues.faction);
                        __LexicalProcess.RegisterKeyword("rank", TokenValues.rank);
                        __LexicalProcess.RegisterKeyword("type", TokenValues.type);
                        __LexicalProcess.RegisterKeyword("description", TokenValues.description);
                        __LexicalProcess.RegisterKeyword("Action", TokenValues.Action);
                        __LexicalProcess.RegisterKeyword("targets", TokenValues.targets);
                        __LexicalProcess.RegisterKeyword("Bool", TokenValues.Bool);
                        __LexicalProcess.RegisterKeyword("String", TokenValues.String);
                        __LexicalProcess.RegisterKeyword("Number", TokenValues.Number);
                        __LexicalProcess.RegisterKeyword("OnActivation", TokenValues.OnActivation);
                    }

                    return __LexicalProcess;
                }
            }
        }
    }
}