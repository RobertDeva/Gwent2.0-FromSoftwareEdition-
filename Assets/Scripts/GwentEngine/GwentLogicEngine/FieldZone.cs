using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{

    namespace GwentEngine
    {
        public abstract class FieldZone
        {
            public List<IPlayable> InvoqueZone { get; set; }
            public int Limit { get; protected set; }
        }

        public class UnitZone : FieldZone
        {
            public Position FieldRange { get; set; }
            public WeatherZone WeatherZone { get; set; }
            public UpgradeZone UpgradeZone { get; set; }

            public UnitZone(Position range)
            {
                FieldRange = range;
                Limit = 5;
            }

        }

        public class WeatherZone : FieldZone
        {
            public UnitZone UnitZone1 { get; set; }
            public UnitZone UnitZone2 { get; set; }


            public WeatherZone(UnitZone unitZone1, UnitZone unitZone2)
            {
                Limit = 1;
                UnitZone1 = unitZone1;
                UnitZone2 = unitZone2;
            }
        }

        public class UpgradeZone : FieldZone
        {
            public UnitZone UnitZone { get; set; }

            public UpgradeZone(UnitZone unitZone)
            {
                Limit = 1;
                UnitZone = unitZone;
            }
        }
    }

}
