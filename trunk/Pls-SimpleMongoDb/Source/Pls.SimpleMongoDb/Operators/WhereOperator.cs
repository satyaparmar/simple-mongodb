using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class WhereOperator
        : SimoOperator
    {
        public WhereOperator(string expression)
        {
            Expression = string.Format("{{$where : \"{0}\"}}", expression);
        }

        public static implicit operator WhereOperator(string whereStatement)
        {
            return new WhereOperator(whereStatement);
        }
    }
}