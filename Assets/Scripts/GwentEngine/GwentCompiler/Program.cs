using System.Collections;
using System.Collections.Generic;
using GwentEngine.GwentCompiler;
using UnityEngine;

public class Programa
{
	public static bool IsFirstAssign = false;
	public static bool ValidToGenerate = false;
	public static ElementalProgram elementalProgram;

	public static void Main(string code)
	{
		LexicalAnalyzer lexical = Compiling.Lexical;
		string text = code;


		IEnumerable<Token> tokens = lexical.GetTokens("code", text, new List<CompilingError>()); //Crea un enumerable de tokens
		int a = 0;
		foreach (Token token in tokens) //recorre la lista de tokens recien creada e imprime cada token
		{
			Debug.Log(token + " " + a);
			a++;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Parsing
		Debug.Log("Primer parser");

		TokenStream stream = new TokenStream(tokens);
		Debug.Log(stream.count.ToString());
		Parser parser = new Parser(stream);

		List<CompilingError> errors = new List<CompilingError>();

		ElementalProgram program = parser.ParseProgram(errors);
		Dictionarys.effects = program.Effects;
		Dictionarys.cards = program.Cards;

		foreach (CompilingError error in Parser.compilingErrors)
		{
			errors.Add(error);
		}
		if (errors.Count > 0)
		{
			foreach (CompilingError error in errors)
			{
				Debug.Log(error.Location.Line + " " + error.Code + " " + error.Argument);
			}
		}
		else
		{
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//Chequeo semantico
			Context context = new Context();
			Scope scope = new Scope();


			program.CheckSemantic(context, scope, errors);

			if (errors.Count > 0)
			{
				foreach (CompilingError error in errors)
				{
					Debug.Log(error.Location.Line + ", " + error.Code + ", " + error.Argument);
				}
			}
			else
			{
				Debug.Log("Chequeo semantico finalizado");
				ValidToGenerate = true;
				elementalProgram = program;
			}
		}
	}
}