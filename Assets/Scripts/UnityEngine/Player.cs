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
            if (playable.Rank == Rank.Silver.ToString())
            {
                MetodosUtilesDeInstanciar.InstanciarCarta(SilverCard,playable,this);
            }
            else if (playable.Rank == Rank.Gold.ToString())
            {
                MetodosUtilesDeInstanciar.InstanciarCarta(GoldCard, playable, this);
            }
            else
            {
                if (playable.Type == CardType.Unit.ToString())
                {
                    MetodosUtilesDeInstanciar.InstanciarSe�uelo(LureCard,playable,this);
                }
                else
                {
                    MetodosUtilesDeInstanciar.InstanciarCarta(Card, playable, this);
                }
            }
        }
    }

    public void CancelLure()
    {
        Se�ueloActivo = false;
    }

    
}
