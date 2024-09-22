using System.Collections;
using System.Collections.Generic;
using System;

namespace GwentEngine
{
    namespace GwentCompiler
    {
        public class Param
        {
            public string Id { get; private set; }

            public TypeOfValue TypeOfValue { get; private set; }


            public Param(string id, TypeOfValue typeOfValue)
            {
                Id = id;
                TypeOfValue = typeOfValue;
            }
        }
        public class ParamValue
        {
            public string Id { get; private set; }
            public Expression Expression { get; private set; }

            public ParamValue(string id, Expression expression)
            {
                Id = id;
                Expression = expression;
            }

        }


        public enum TypeOfValue
        {
            Bool,
            String,
            Number,
        }
    }
}

