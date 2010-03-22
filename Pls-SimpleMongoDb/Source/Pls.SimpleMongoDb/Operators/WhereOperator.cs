using System;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.Serialization.Converters;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable, JsonConverter(typeof(WhereOperatorConverter))]
    public class WhereOperator
        : SimoOperator
    {
        public WhereOperator(string expression)
            : base("$where")
        {
            Expression = expression;
        }
    }
}