using System;

namespace Pls.SimpleMongoDb.Querying
{
    [Serializable]
    public class WhereQueryProperty
        : QueryProperty
    {
        internal WhereQueryProperty(Query query)
            : base(query, "$where")
        {
        }

        public override string GenerateExpression()
        {
            var expressions = GetExpressionPartsString();

            return string.Format("{0} : \" {1} \"",
                                 Name, expressions);
        }
    }
}