using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    public class Board
    {
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public static UnitZone Melee1 = new (Position.Melee);
        public static UpgradeZone UpgradeMelee1 = new (Melee1);
        public static UnitZone Melee2 = new (Position.Melee);
        public static UpgradeZone UpgradeMelee2 = new (Melee2);
        public static WeatherZone MeleeWeather = new(Melee1, Melee2);
        public static UnitZone Range1 = new(Position.Range);
        public static UpgradeZone UpgradeRange1 = new(Range1);
        public static UnitZone Range2 = new(Position.Range);
        public static UpgradeZone UpgradeRange2 = new(Range2);
        public static WeatherZone RangeWeather = new(Range1, Range2);
        public static UnitZone Siege1 = new(Position.Siege);
        public static UpgradeZone UpgradeSiege1 = new(Siege1);
        public static UnitZone Siege2 = new(Position.Siege);
        public static UpgradeZone UpgradeSiege2 = new(Siege2);
        public static WeatherZone SiegeWeather = new(Siege1, Siege2);
        
        public Board()
        {
            player1.GetBoard = this;
            player2.GetBoard = this;
            player1.Melee = Melee1;
            player2.Melee = Melee2;
            player1.UpgradeMelee = UpgradeMelee1;
            player2.UpgradeMelee = UpgradeMelee2;
            player1.Range = Range1;
            player2.Range = Range2;
            player1.UpgradeRange = UpgradeRange1;
            player2.UpgradeRange = UpgradeRange2;
            player1.Siege = Siege1;
            player2.Siege = Siege2;
        }
    }
    
}