using System.Collections;
using System.Collections.Generic;
using GwentEngine;
using UnityEngine;

public class GwentPlayer : MonoBehaviour
{
    public Player player;
    public bool HasPassed; //Representa si el jugador ha pasado su turno
    public int PlayerLife; //Representa las "Vidas" de cada jugador en el juego, si es cero, ese jugador pierde
    public bool ActivateLeader; //Booleano que avisa si el jugador ya jug� su carta de lider
    public bool PlayerTurn; //Representa si es turno del jugador
    public bool Se�ueloActivo; //Representa si se va a jugar un se�uelo

    public GameObject Card;
    public GameObject SilverCard;
    public GameObject GoldCard;
    public GameObject LureCard;

    public GameObject LifeToken1;
    public GameObject LifeToken2;

    public void Draw()
    {
        IPlayable playable;

        player.Draw(out IPlayable card);
        playable = card;
        if (playable != null)
        {
            if (player.Hand[player.Hand.Count - 1].Rank == Rank.Silver.ToString())
            {
                Instantiate(SilverCard, GameBoard.TransformsZones[playable.Origin].transform, false);

            }
            else if (player.Hand[player.Hand.Count - 1].Rank == Rank.Gold.ToString())
            {
                Instantiate(GoldCard, GameBoard.TransformsZones[playable.Origin].transform, false);
            }
            else
            {
                if (player.Hand[player.Hand.Count - 1].Type == CardType.Unit.ToString())
                {
                    Instantiate(LureCard, GameBoard.TransformsZones[playable.Origin].transform, false);
                }
                else
                {
                    Instantiate(Card, GameBoard.TransformsZones[playable.Origin].transform, false);
                }
            }
        }
    }

    public void CancelLure()
    {
        Se�ueloActivo = false;
    }

    
}
