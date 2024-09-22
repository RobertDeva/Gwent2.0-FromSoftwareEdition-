using System.Collections;
using System.Collections.Generic;


namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class While : ASTNode
        {
            public Expression Condition;
            public List<ASTNode> ActionList;

            public While(Expression condition, CodeLocation location) : base(location)
            {
                ActionList = new List<ASTNode>();
            }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool condition = Condition.CheckSemantic(context, scope, errors);
                if (Condition.Type != ExpressionType.Bool)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Condition of while must be bool"));
                    return false;
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

                return condition && checkInstructions;
            }
           
        }

    }
}
