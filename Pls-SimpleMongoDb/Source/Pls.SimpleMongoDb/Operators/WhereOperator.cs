using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
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