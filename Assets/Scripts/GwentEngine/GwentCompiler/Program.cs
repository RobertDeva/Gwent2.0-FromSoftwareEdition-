using System.Collections;
using System.Collections.Generic;
using GwentEngine.GwentCompiler;
using UnityEngine;

public class Programa
{
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
		Debug.Log("Parser");

		TokenStream stream = new TokenStream(tokens);
		Parser parser = new Parser(stream);

		List<CompilingError> errors = new List<CompilingError>();

		ElementalProgram program = parser.ParseProgram(errors);

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
	}
}