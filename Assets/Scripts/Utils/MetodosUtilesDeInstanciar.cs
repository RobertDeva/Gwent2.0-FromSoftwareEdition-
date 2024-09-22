using System.Collections;
using System.Collections.Generic;
using GwentEngine;
using UnityEngine;

public class MetodosUtilesDeInstanciar : MonoBehaviour
{
    public static void InstanciarCarta(GameObject card, ICard carta, GwentPlayer player)
    {
        Instantiate(card, GameBoard.TransformsZones[carta.Origin].transform, false);
        card.GetComponent<Card>().player = player;
        card.GetComponent<Card>().card = carta;
        card.GetComponent<Card>().cardImage.sprite = CardImages.CardImagesColection[carta];
    }
    public static void InstanciarSeñuelo(GameObject card, ICard carta, GwentPlayer player)
    {
        Instantiate(card, GameBoard.TransformsZones[carta.Origin].transform, false);
        card.GetComponent<Lure>().player = player;
        card.GetComponent<Lure>().card = carta;
        card.GetComponent<Lure>().cardImage.sprite = CardImages.CardImagesColection[carta];
    }
}
