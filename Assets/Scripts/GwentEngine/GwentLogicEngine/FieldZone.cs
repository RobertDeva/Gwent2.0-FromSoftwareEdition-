using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{

    public abstract class FieldZone
    {
        public List<ICard> InvoqueZone { get; set; }
    }

    public class UnitZone : FieldZone
    {
        public Position FieldRange { get; set; }
        public WeatherZone WeatherZone { get; set; }
        public UpgradeZone UpgradeZone { get; set; }

        public UnitZone(Position range)
        {
            FieldRange = range;            
        }

    }

    public class WeatherZone : FieldZone
    {
        public UnitZone UnitZone1 { get; }
        public UnitZone UnitZone2 { get; }
        public int Limit { get; }

        public WeatherZone(UnitZone unitZone1, UnitZone unitZone2)
        {
            Limit = 1;
            UnitZone1 = unitZone1;
            UnitZone1.WeatherZone = this;
            UnitZone2 = unitZone2;
            UnitZone2.WeatherZone = this;
        }
    }

    public class UpgradeZone : FieldZone
    {
        public UnitZone UnitZone { get; }
        public int Limit { get; }

        public UpgradeZone(UnitZone unitZone)
        {
            Limit = 1;
            UnitZone = unitZone;
            UnitZone.UpgradeZone = this;
        }
    }

}


