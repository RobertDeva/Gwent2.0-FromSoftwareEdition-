using System.Collections;
using System.Collections.Generic;
using GwentEngine;
using GwentEngine.GwentCompiler;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GwentPlayer Player1;
    public GwentPlayer Player2;
    public GameObject Hand1;
    public GameObject Hand2;
    public GameObject Graveyard1;
    public GameObject Graveyard2;
    public GameObject Melee1;
    public GameObject MeleeUp1;
    public GameObject Melee2;
    public GameObject MeleeUp2;
    public GameObject Range1;
    public GameObject RangeUp1;
    public GameObject Range2;
    public GameObject RangeUp2;
    public GameObject Siege1;
    public GameObject SiegeUp1;
    public GameObject Siege2;
    public GameObject SiegeUp2;
    public GameObject MeleeWeather;
    public GameObject RangeWeather;
    public GameObject SiegeWeather;

    public GameObject Card;
    public GameObject GoldCard;
    public GameObject SilverCard;
    public GameObject LureCard;

    public Board Board = new();
    public static Dictionary<List<ICard>, GameObject> TransformsZones = new();

    public void Start()
    {
        TransformsZones[Board.Melee1.InvoqueZone] = Melee1;
        TransformsZones[Board.Melee2.InvoqueZone] = Melee2;
        TransformsZones[Board.Range1.InvoqueZone] = Range1;
        TransformsZones[Board.Range2.InvoqueZone] = Range2;
        TransformsZones[Board.Siege1.InvoqueZone] = Siege1;
        TransformsZones[Board.Siege2.InvoqueZone] = Siege2;
        TransformsZones[Board.UpgradeMelee1.InvoqueZone] = MeleeUp1;
        TransformsZones[Board.UpgradeMelee2.InvoqueZone] = MeleeUp2;
        TransformsZones[Board.UpgradeRange1.InvoqueZone] = RangeUp1;
        TransformsZones[Board.UpgradeRange2.InvoqueZone] = RangeUp2;
        TransformsZones[Board.UpgradeSiege1.InvoqueZone] = SiegeUp1;
        TransformsZones[Board.UpgradeSiege2.InvoqueZone] = SiegeUp2;
        TransformsZones[Board.MeleeWeather.InvoqueZone] = MeleeWeather;
        TransformsZones[Board.RangeWeather.InvoqueZone] = RangeWeather;
        TransformsZones[Board.SiegeWeather.InvoqueZone] = SiegeWeather;
    }

    public void StartGame()
    {
        Board.SetPlayers(Player1.player, Player2.player);
        Board.Start();
        TransformsZones[Board.player1.Hand] = Hand1;
        TransformsZones[Board.player2.Hand] = Hand2;
        TransformsZones[Board.player1.Graveyard] = Graveyard1;
        TransformsZones[Board.player2.Graveyard] = Graveyard2;

        for (int i = 0; i < 10; i++)
        {
            Player1.Draw();
            Player2.Draw();
        }
    }
    public void Update()
    {
        if (Hand1.transform.childCount < Player1.player.Hand.Count || Hand2.transform.childCount < Player2.player.Hand.Count)
        {
            if (Hand1.transform.childCount < Player1.player.Hand.Count)
            {
                UpdateHand(Player1);
            }
            if (Hand2.transform.childCount < Player2.player.Hand.Count)
            {
                UpdateHand(Player2);
            }
        }
    }

    private void UpdateHand(GwentPlayer player)
    {
        foreach (var card in player.player.Hand)
        {
            bool CardInstanciated = false;
            foreach (Transform carta in TransformsZones[player.player.Hand].transform)
            {
                Card gameobject = carta.GetComponent<Card>();
                if (gameobject.card == card)
                    CardInstanciated = true;
                else
                {
                    Lure lure = carta.GetComponent<Lure>();
                    if (lure.card == card)
                        CardInstanciated = true;
                }
            }
            if (!CardInstanciated)
            {
                InstanciateInHand(player, card);
            }
        }
    }
    private void InstanciateInHand(GwentPlayer player, ICard card)
    {
        if (card.Rank == GwentEngine.Rank.Silver.ToString())
        {
            MetodosUtilesDeInstanciar.InstanciarCarta(SilverCard, card, player);
        }
        else if (card.Rank == GwentEngine.Rank.Gold.ToString())
        {
            MetodosUtilesDeInstanciar.InstanciarCarta(GoldCard, card, player);
        }
        else
        {
            if (card.Type == GwentEngine.CardType.Unit.ToString())
            {
                MetodosUtilesDeInstanciar.InstanciarSeñuelo(LureCard, card, player);
            }
            else
            {
                MetodosUtilesDeInstanciar.InstanciarCarta(Card, card, player);
            }
        }
    }

}
