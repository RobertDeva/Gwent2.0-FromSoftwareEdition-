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


                        __LexicalProcess.RegisterOperator(",", TokenValues.ValueSeparator);
                        __LexicalProcess.RegisterOperator(";", TokenValues.StatementSeparator);
                        __LexicalProcess.RegisterOperator("(", TokenValues.OpenBracket);
                        __LexicalProcess.RegisterOperator(")", TokenValues.ClosedBracket);
                        __LexicalProcess.RegisterOperator("{", TokenValues.OpenCurlyBraces);
                        __LexicalProcess.RegisterOperator("}", TokenValues.ClosedCurlyBraces);



                        /*  */
                        __LexicalProcess.RegisterText("\"", "\"");

                        __LexicalProcess.RegisterKeyword("Card", TokenValues.Card);
                        __LexicalProcess.RegisterKeyword("Effect", TokenValues.Effect);
                        __LexicalProcess.RegisterKeyword("power", TokenValues.power);
                        __LexicalProcess.RegisterKeyword("Faction", TokenValues.Faction);
                        __LexicalProcess.RegisterKeyword("CardTipe", TokenValues.CardTipe);
                        __LexicalProcess.RegisterKeyword("Rank", TokenValues.Rank);
                        __LexicalProcess.RegisterKeyword("Range", TokenValues.Range);
                        __LexicalProcess.RegisterKeyword("description", TokenValues.description);
                    }

                    return __LexicalProcess;
                }
            }
        }
    }
}