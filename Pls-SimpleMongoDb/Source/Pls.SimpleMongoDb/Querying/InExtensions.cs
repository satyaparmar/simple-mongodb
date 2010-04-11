using System.Linq;

namespace Pls.SimpleMongoDb.Querying
{
    public static class InExtensions
    {
        public static QueryProperty In<T>(this QueryProperty property, params T[] operands)
        {
            var expression = string.Format("$in : [{0}]",
                                           string.Join(",", 
                                                       operands.Select(
                                                           o => o is string 
                                                                    ? string.Format("'{0}'", (object) o) 
                                                                    : o.ToString())));

            property.AddExpression(expression);

            return property;
        }
    }
}