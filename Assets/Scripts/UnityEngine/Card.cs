using System.Collections;
using System.Collections.Generic;
using GwentEngine;
using GwentEngine.GwentCompiler;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GwentPlayer player;
    public ICard card;
    public Image cardImage;
    public GameObject Back;

    private void Update()
    {
        if (transform.parent == GameBoard.TransformsZones[player.player.Graveyard].transform)
        {
            transform.gameObject.SetActive(false);
        }
        if(!player.PlayerTurn && isActiveAndEnabled)
        {
            Back.SetActive(true);
        }
        if(player.PlayerTurn)
        {
            Back.SetActive(false);
        }
        if(isActiveAndEnabled)
        {
            transform.SetParent(GameBoard.TransformsZones[card.Origin].transform, false);
            if(card.Type == "Weather" && card.Origin != card.Owner.Hand)
            {
                card.CastEffect();
            }
            if (card.Type == "Upgrade" && card.Origin != card.Owner.Hand)
            {
                card.CastEffect();
            }
        }
    }

    public void SetInMelee()
    {
        if(player.PlayerTurn)
        {
            card.InvokeInMelee(out bool InMelee);
            if (InMelee)
            {
                GameManager.ChangeTurn();
            }
        }
    }
    public void SetInRange()
    {
        if(player.PlayerTurn)
        {
            card.InvokeInRange(out bool InRange);
            if(InRange)
            {
                GameManager.ChangeTurn();
            }
        }
    }
    public void SetInSiege()
    {
        if (player.PlayerTurn)
        {
            card.InvokeInSiege(out bool InSiege);
            if (InSiege)
            {
                GameManager.ChangeTurn();
            }
        }
    }
}