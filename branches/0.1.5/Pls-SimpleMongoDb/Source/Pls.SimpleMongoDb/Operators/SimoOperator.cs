using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public abstract class SimoOperator 
        : ISimoOperator
    {
        protected string Expression { get; set; }

        public static implicit operator string(SimoOperator orOperator)
        {
            return orOperator.Expression;
        }

        public override string ToString()
        {
            return Expression;
        }
    }
}