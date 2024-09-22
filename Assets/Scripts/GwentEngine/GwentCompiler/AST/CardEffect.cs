using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class CardEffect : ASTNode
        {
            public bool IsPostAction { get; set; } 
            public CardEffect? Parent { get; set; } 
            public string Name { get; set; }
            public List<ParamValue> Params { get; set; }
            public Selector Selector { get; set; }
            public CardEffect? PostAction { get; set; }

            public CardEffect(bool PostAction ,CodeLocation location) : base(location)
            {
                IsPostAction = PostAction;
                Location = location;
            }
            

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool checkEffect;
                bool checkSelector;
                bool checkPostAction;

                if (!context.effects.Contains(Name))
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "this is not a declared effect"));
                    checkEffect = false;
                }
                else
                {
                    if(Params.Count != Effects.CompilatedEffects[Name].ParamsExpresions.Count)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "this is diferent of the declared effect"));
                        checkEffect = false;
                    }
                    else
                    {
                        bool checkParams = true;
                        foreach(var param in Params)
                        {
                            bool checkParam = false;
                            foreach(var item in Effects.CompilatedEffects[Name].ParamsExpresions)
                            {
                                if(item.Id == param.Id)
                                {
                                    if(item.TypeOfValue == TypeOfValue.Bool && param.Expression.Type  == ExpressionType.Bool)
                                    {
                                        checkParam = true;
                                    }
                                    if (item.TypeOfValue == TypeOfValue.Number && param.Expression.Type == ExpressionType.Number)
                                    {
                                        checkParam = true;
                                    }
                                    if (item.TypeOfValue == TypeOfValue.String && param.Expression.Type == ExpressionType.Text)
                                    {
                                        checkParam = true;
                                    }
                                }
                            }
                            if(checkParam)
                            {
                                checkParams = true;
                            }
                            else
                            {
                                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid param assignation"));
                                checkParams = false;                              
                            }
                        }
                        checkEffect = checkParams; 
                    }  
                    
                }
                checkSelector = Selector.CheckSemantic(context, scope, errors);
                checkPostAction = PostAction.CheckSemantic(context, scope, errors);
                return checkEffect && checkSelector && checkPostAction;
            }
        }
    }
}
