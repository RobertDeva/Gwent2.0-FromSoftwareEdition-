using System;
using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Card : ASTNode, ICard
        {
            public Player Owner { get; set; }
            public List<ICard> Origin { get; set; }
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
            public string Description { get => description; }
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
            public List<CardEffect> effects;
            

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool checkpower;
                bool faction;
                bool type;
                bool rank;
                bool range = true;

                List<string> ranges = new List<string>() { "Melee", "Range", "Siege"};
                checkpower = power.CheckSemantic(context, scope, errors);
                faction = this.faction.CheckSemantic(context, scope, errors);
                type = this.type.CheckSemantic(context, scope, errors);
                rank = this.rank.CheckSemantic(context, scope, errors);

                foreach(var rang in this.range)
                {
                    if (ranges.Contains(rang))
                    {
                        if (!scope.range.Contains(rang))
                        {
                            scope.range.Add(rang);
                        }
                        else
                        {
                            range = false;
                            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "this range is already assigned"));
                        }
                    }
                    else
                    {
                        range
                            = false;
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "this range is not acceptable"));
                    }
                }
                if(power.Type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "power must be numerical"));
                    checkpower = false;
                }
                return checkpower && faction && type && rank && range;
            }

            public Card(string id ,CodeLocation location) : base(location)
            {
                Id = id;
                Location = location;
                inField = false;
                AffectedByWeather = false;
                AffectedByBuff = false;
            }

            public void Invoke(FieldZone? zone)
            {
                MetodosUtiles.MoveList(this, Origin, zone.InvoqueZone);
                Origin = zone.InvoqueZone;
                CastEffect();
            }
            public void ResetState()
            {
                actualpower = power;
                inField = false;
                AffectedByBuff = false;
                AffectedByWeather = false;
            }

            public void CastEffect()
            {

            }
               
        }

    }
}
