using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool arranque; //Mientras este en false, el juego en si no ha comenzado. Sirve para varias cosas como elegir cartas al comienzo
    public static bool PassP1; //Version estatica que dice si el jugador 1 pasó. Necesario para el metodo de cambio de turnos. Lo modifica el metodo de pasar turno.
    public static bool PassP2; //Version estatica que dice si el jugador 2 pasó. Necesario para el metodo de cambio de turnos Lo modifica el metodo de pasar turno.
    public static int RoundS; //variable que por cual ronda va el juego


    public GameObject TotalAttackP1; //Necesario para evaluar los ataques
    public GameObject TotalAttackP2; //Necesario para evaluar los ataques
    public GameObject Player1;
    public GameObject Player2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Metodo para arrancar el juego desde el boton de continuar 2
    public void ArrancarJuego()
    {
        arranque = true;
    }

    //Metodo para cambiar turnos (en el inspector, el jugador 1 tiene el turno en true y el 2 en false, por tanto este metodo alternará el true de los jugadores)
    public static void ChangeTurn()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        GwentPlayer p1 = player1.GetComponent<GwentPlayer>();
        GwentPlayer p2 = player2.GetComponent<GwentPlayer>();

        if (PassP1 || PassP2) return;
        else
        {
            p1.PlayerTurn = !p1.PlayerTurn;
            p2.PlayerTurn = !p2.PlayerTurn;
        }
    }

    //Metodo para pasar el turno del jugador 1, se activa en un principio desde el boton de pasar turno respectivo
    public static void PassTurnP1()
    {
        GameObject player1 = GameObject.Find("Player1");
        GwentPlayer p1 = player1.GetComponent<GwentPlayer>();
        if (p1.PlayerTurn)
        {
            if (!p1.HasPassed)
            {
                ChangeTurn();
                p1.HasPassed = true;
                PassP1 = true;
            }
        }
    }

    //Metodo para pasar el turno del jugador 2, se activa en un principio desde el boton de pasar turno respectivo
    public static void PassTurnP2()
    {
        GameObject player2 = GameObject.Find("Player2");
        GwentPlayer p2 = player2.GetComponent<GwentPlayer>();
        if (p2.PlayerTurn)
        {
            if (!p2.HasPassed)
            {
                ChangeTurn();
                p2.HasPassed = true;
                PassP2 = true;
            }
        }
    }

}
