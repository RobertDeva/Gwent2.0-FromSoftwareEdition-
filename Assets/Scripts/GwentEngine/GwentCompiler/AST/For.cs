using System.Collections;
using System.Collections.Generic;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class For : ASTNode
        {
            public List<ASTNode> ActionList;

            public For(CodeLocation location) : base(location)
            {
                ActionList = new List<ASTNode>();
            }
            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool CheckInstruction = false;
                bool CheckInstructions = true;
                foreach (ASTNode instruction in ActionList)
                {
                    if (!(instruction is Assign))
                    {
                        if (!(instruction is While) && !(instruction is For) && !(instruction is DotNotation))
                        {
                            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid instruction"));
                            CheckInstructions = false;
                            continue;
                        }
                    }


                    if (instruction is While || instruction is For)
                    {
                        CheckInstruction = instruction.CheckSemantic(context, scope.CreateChild(), errors);
                    }
                    else
                    {
                        CheckInstruction = instruction.CheckSemantic(context, scope, errors);
                    }


                    if (CheckInstruction == false)
                    {
                        CheckInstructions = false;
                    }
                }

                return CheckInstructions;
            }
            
        }
        
    }
}

