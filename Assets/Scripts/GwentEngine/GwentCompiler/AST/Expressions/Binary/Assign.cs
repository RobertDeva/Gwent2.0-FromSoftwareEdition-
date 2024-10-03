using System;
using System.Collections;
using System.Collections.Generic;


namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Assign : BinaryExpression
        {

            public override ExpressionType Type { get; set; }
            public override object? Value { get; set; }

            public Assign(CodeLocation location) : base(location) { }

            public override void Evaluate()
            {
                Right.Evaluate();
                Left.Evaluate();


                if (Left is Identifier)
                {

                    if (EffectExecutation.identifiers.ContainsKey(Left.Value.ToString()))
                    {
                        EffectExecutation.identifiers[Left.Value.ToString()] = Right;
                    }
                }
                if (Left is DotNotation)
                {
                    DotNotation dotNotation = (DotNotation)Left;
                    if (dotNotation.Left.Type == ExpressionType.Card)
                    {
                        if(dotNotation.Right.Type == ExpressionType.Number)
                        {
                            dotNotation.Value = Right.Value;
                        }
                    }
                }
            }

            public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
            {
                bool right = Right.CheckSemantic(context, scope, errors);
                bool left = Left.CheckSemantic(context, scope, errors);

                if(Left is Identifier)
                {
                    bool identifier = scope.AssignedIdentifier(Left.Value.ToString(), out Scope cntx);
                    if(identifier)
                    {
                        cntx.VarYValores[Left.Value.ToString()] = Right;
                    }
                    else
                    {
                        scope.VarYValores.Add(Left.Value.ToString(), Right);
                    }

                }
                if(Left is DotNotation)
                {
                    BinaryExpression dot = (DotNotation)Left;
                    if(dot.Type == ExpressionType.Card)
                    {
                        if((string)dot.Right.Value == "Power")
                        {
                            if(Right.Type != ExpressionType.Number)
                            {
                                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid type for assign"));
                                Type = ExpressionType.ErrorType;
                                return false;
                            }
                        }
                        else
                        {
                            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid type for assign"));
                            Type = ExpressionType.ErrorType;
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid "));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }

                if (Right.Type == ExpressionType.Number)
                {
                    double a;
                    if (Left.Type != ExpressionType.Number || double.TryParse(Left.Value.ToString(), out a))
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid type for assign"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }
                else if (Right.Type == ExpressionType.Text)
                {
                    string a = Left.Value.ToString();
                    if (Left.Type != ExpressionType.Text || a[0] == '"')
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid type for assign"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }
                else if (Right.Type == ExpressionType.Bool)
                {
                    bool a;
                    if (Left.Type != ExpressionType.Bool || bool.TryParse(Left.Value.ToString(), out a))
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid type for assign"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }

                Type = ExpressionType.Anytype;
                return right && left;
            }

            public override string ToString()
            {
                if (Value == null)
                {
                    return String.Format("{0} = {1}", Left, Right);
                }
                return Value.ToString();
            }
        }

    }
}