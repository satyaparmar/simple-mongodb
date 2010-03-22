﻿using System;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.Serialization.Converters;

namespace Pls.SimpleMongoDb.DataTypes
{
    [Serializable, JsonConverter(typeof(SimoRegexJsonConverter))]
    public class SimoRegex
    {
        private string _options;

        public string Expression { get; set;}

        public string Options
        {
            get { return _options; }
            set { _options = value ?? ""; }
        }

        public SimoRegex(string expression, string options = null) 
        {
            Expression = expression;
            Options = options;
        }
    }
}