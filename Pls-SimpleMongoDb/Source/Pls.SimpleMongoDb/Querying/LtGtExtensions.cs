namespace Pls.SimpleMongoDb.Querying
{
    public static class LtGtExtensions
    {
        public static QueryProperty Gt<T>(this QueryProperty property, T operand)
        {
            AddExpression(property, "$gt", operand.ToString());

            return property;
        }

        public static QueryProperty GtE<T>(this QueryProperty property, T operand)
        {
            AddExpression(property, "$gte", operand.ToString());

            return property;
        }

        public static QueryProperty Lt<T>(this QueryProperty property, T operand)
        {
            AddExpression(property, "$lt", operand.ToString());

            return property;
        }

        public static QueryProperty LtE<T>(this QueryProperty property, T operand)
        {
            AddExpression(property, "$lte", operand.ToString());

            return property;
        }

        private static void AddExpression(QueryProperty property, string operatorName, string value)
        {
            var expression = string.Format("{0} : {1}",
                                           operatorName, value);

            property.AddExpression(expression);
        }
    }
}