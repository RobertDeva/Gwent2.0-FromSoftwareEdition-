using System.Collections;
using System.Collections.Generic;
using GwentEngine;
using UnityEngine;
using UnityEngine.UI;

public class Lure : MonoBehaviour
{
    public GwentPlayer player;
    public ICard card;
    public Image cardImage;
    public GameObject Back;

    public void Update()
    {
        if (transform.parent == GameBoard.TransformsZones[player.player.Graveyard].transform)
        {
            transform.gameObject.SetActive(false);
        }
        if (!player.PlayerTurn && isActiveAndEnabled)
        {
            Back.SetActive(true);
        }
        if (player.PlayerTurn)
        {
            Back.SetActive(false);
        }
        if (isActiveAndEnabled)
        {
            transform.SetParent(GameBoard.TransformsZones[card.Origin].transform, false);
        }
    }
    public void LureSwicht()
    {
        if (player.PlayerTurn)
        {
            player.SeñueloActivo = true;
            player.señuelo = card;
        }
    }
}
