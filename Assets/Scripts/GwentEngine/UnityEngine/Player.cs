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
    public ICard se�uelo;
    public bool CardSelected; //Representa si se va selecciono la carta para hacer el cambio;
    public ICard selectedCard;
    public double TotalPower;

    public GameObject Card;
    public GameObject SilverCard;
    public GameObject GoldCard;
    public GameObject LureCard;

    public GameObject LifeToken1;
    public GameObject LifeToken2;

    public void Draw()
    {
        ICard playable;

        player.Draw(out ICard card);
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

    public void LeaderHabilty()
    {
        if (!ActivateLeader && PlayerTurn)
        { 
            player.Leader.CastEffect();
            ActivateLeader = true;
            GameManager.ChangeTurn();
        }
    }

    public void CancelLure()
    {
        Se�ueloActivo = false;
        se�uelo = null;
    }
    private void Lure()
    {
        Effects.Se�uelo(se�uelo, selectedCard, out bool swicht);
        if (swicht)
        {
            Se�ueloActivo = false;
            CardSelected = false;
            se�uelo = null;
            selectedCard = null;
            GameManager.ChangeTurn();
        }
    }

    public void Update()
    {
        TotalPower = player.GetTotalPower();
        if (PlayerTurn && Se�ueloActivo && CardSelected)
        {
           Lure();
        }
    }
}
