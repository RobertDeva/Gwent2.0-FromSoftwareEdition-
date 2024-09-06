using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Card : ASTNode, IPlayable
        {
            public Player Owner { get; set; }
            public List<IPlayable> Origin { get; set; }
            public bool InField { get => inField; set => inField = value; }
            public bool AffectedByWeather { get; set; }
            public bool AffectedByBuff { get; set; }
            public string Name
            {
                get
                {
                    return name;
                }
            }
            public double Power 
            { 
                get 
                {
                    return (double)actualpower.Value;
                } 
                set
                {
                    actualpower.Value = value;
                }
            }
            public string Faction
            {
                get
                {
                    return faction.Value;
                }
            }
            public string Type
            {
                get
                {
                    return type.Value;
                }
            }
            public string Rank
            {
                get
                {
                    return rank.Value;
                }
            }
            public List<string> Range
            {
                get
                {
                    return range;
                }
            }
            public bool inField;
            public string Id { get; set; }
            public string name;
            public Expression power;
            public Expression actualpower;
            public Faction faction;
            public CardType type;
            public Rank rank;
            public List<string> range;
            public string description;
           // public Effect effect { get; set; }
            

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                throw new NotImplementedException();
            }

            public Card(string id ,CodeLocation location) : base(location)
            {
                Id = id;
                Location = location;
                inField = false;
            }

            public void Invoke(FieldZone? zone)
            {
                MetodosUtiles.MoveList(this, Origin, zone.InvoqueZone);
                Origin = zone.InvoqueZone;
            }
            public void ResetState()
            {
                actualpower = power;
            }
               
        }

    }
}
