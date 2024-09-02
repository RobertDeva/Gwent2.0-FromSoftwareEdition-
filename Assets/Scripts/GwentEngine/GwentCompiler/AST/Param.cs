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

            public TypeOfValue typeOfValue { get; private set; }


            public Param(string id, TypeOfValue typeOfValue)
            {
                this.Id = id;
                this.typeOfValue = typeOfValue;
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

