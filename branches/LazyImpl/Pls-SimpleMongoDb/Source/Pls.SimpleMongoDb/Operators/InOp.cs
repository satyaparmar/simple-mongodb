using System;
using System.Linq;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class InOp : SimoOperator
    {
        public InOp(string propertyName, params string[] operands)
        {
            var combinedOperands = string.Join(",", operands.Select(o => string.Format("\"{0}\"", o)));

            Expression = string.Format("{{ {0} : {{ $in : [{1}] }} }}", propertyName, combinedOperands);
        }
    }
}