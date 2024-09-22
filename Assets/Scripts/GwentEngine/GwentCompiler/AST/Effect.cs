using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Effect : ASTNode
        {
            public string Id { get; set; }           
            public List<Param> ParamsExpresions;
            public List<ASTNode> ActionList;
            public List<ICard> targets;
            public Card card;

            public bool CollectElements(Context context, Scope scope, List<CompilingError> errors)
            {
                if (context.effects.Contains(Id))
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Effect already defined"));
                    return false;
                }
                else
                {
                    context.effects.Add(Id);
                    Effects.CompilatedEffects.Add(Id,this);
                }
                return true;
            }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
               
                foreach (Param parametro in ParamsExpresions)
                {

                    if (parametro.TypeOfValue == TypeOfValue.Number)
                        scope.VarYValores.Add(parametro.Id, new Number(0, new CodeLocation()));
                    else if (parametro.TypeOfValue == TypeOfValue.String)
                        scope.VarYValores.Add(parametro.Id, new Text("", new CodeLocation()));
                    else if (parametro.TypeOfValue == TypeOfValue.Bool)
                        scope.VarYValores.Add(parametro.Id, new Bool(false, new CodeLocation()));
                    else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid parameter value"));
                }


                bool checkInstruction = false;
                bool checkInstructions = true;

                foreach (ASTNode instruction in ActionList)
                {
                    if (instruction is not Assign)
                    {
                        if (instruction is not While && instruction is not For && instruction is not DotNotation)
                        {
                            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid instruction"));
                            checkInstructions = false;
                            continue;
                        }
                    }

                    if (instruction is While || instruction is For)
                    {
                        checkInstruction = instruction.CheckSemantic(context, scope.CreateChild(), errors);
                    }
                    else
                    {
                        checkInstruction = instruction.CheckSemantic(context, scope, errors);
                    }


                    if (checkInstruction == false)
                    {
                        checkInstructions = false;
                    }
                }
                return checkInstructions;
            }

            public Effect(string id, CodeLocation location) : base(location)
            {
                Id = id;
                Location = location;
            }

        }

    }
}