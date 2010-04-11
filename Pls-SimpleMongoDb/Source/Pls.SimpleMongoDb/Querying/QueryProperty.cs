using System;
using System.Collections.Generic;

namespace Pls.SimpleMongoDb.Querying
{
    [Serializable]
    public class QueryProperty
    {
        private readonly List<string> _expressionParts;

        public Query Query { get; private set; }
        public string Name { get; private set; }

        internal QueryProperty(Query query, string name)
        {
            _expressionParts = new List<string>();

            Query = query;
            Name = name;
        }

        public QueryProperty And()
        {
            return this;
        }

        public QueryProperty And(string name)
        {
            var p = Query[name];

            return p;
        }

        internal void AddExpression(string expression)
        {
            _expressionParts.Add(expression);
        }

        internal void SetExpression(string expression)
        {
            _expressionParts.Clear();
            _expressionParts.Add(expression);
        }

        public virtual string GenerateExpression()
        {
            var expressions = GetExpressionPartsString();

            return string.Format("{0} : {{ {1} }}",
                                 Name, expressions);
        }

        protected string GetExpressionPartsString()
        {
            var expressions = string.Join(", ", _expressionParts.ToArray());

            return expressions;
        }

        public override string ToString()
        {
            return Query.ToString();
        }
    }
}