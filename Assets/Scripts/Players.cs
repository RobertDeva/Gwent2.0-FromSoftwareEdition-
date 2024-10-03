using System.Collections;
using System.Collections.Generic;
using GwentEngine;
using UnityEngine;

public class Players : MonoBehaviour
{
    public static Player DarkSouls;
    public static Player EldenRing;
    public static Player Sekiro;

    public static List<ICard> elden;
    public static List<ICard> dark;
    public static List<ICard> sekiro;
    public static LeaderCard Gwyn = new LeaderCard("Gwyn", Faction.DarkSoul, "un tipo duro");
    public static LeaderCard Radan = new LeaderCard("Radan", Faction.EldenRing, "un tipo duro");
    public static LeaderCard Genichiro = new LeaderCard("Genichiro", Faction.Sekiro, "unn tipo mas o menos");
}
