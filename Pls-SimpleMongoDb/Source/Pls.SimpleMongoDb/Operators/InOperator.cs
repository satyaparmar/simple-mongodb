using System;
using System.Linq;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class InOperator : SimoOperator
    {
        public InOperator(string propertyName, params string[] operands)
        {
            var combinedOperands = string.Join(",", operands.Select(o => string.Format("\"{0}\"", o)));

            Expression = string.Format("{{ {0} : {{ $in : [{1}] }} }}", propertyName, combinedOperands);
        }
    }
}