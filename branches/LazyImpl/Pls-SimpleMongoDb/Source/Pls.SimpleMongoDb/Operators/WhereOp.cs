using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class WhereOp
        : SimoOperator
    {
        public WhereOp(string expression)
        {
            Expression = string.Format("{{$where : \"{0}\"}}", expression);
        }

        public static implicit operator WhereOp(string whereStatement)
        {
            return new WhereOp(whereStatement);
        }
    }
}