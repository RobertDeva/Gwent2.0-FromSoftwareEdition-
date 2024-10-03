using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GwentEngine;
using GwentEngine.GwentCompiler;

public class Cards
{
    public static List<ICard> CompilatedCards = new List<ICard>();
    public Sprite sprite = CardImages.Compileted;

    public void SetImages()
    {
        foreach (var card in CompilatedCards)
        {
            CardImages.CardImagesColection.Add(card, sprite);
        }
    }
}
public class Dictionarys
{
    public static Dictionary<string, Effect> effects;
    public static Dictionary<string, GwentEngine.GwentCompiler.Card> cards;
}