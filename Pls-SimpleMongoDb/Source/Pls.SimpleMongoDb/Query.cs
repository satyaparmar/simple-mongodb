using System;
using System.Collections.Generic;
using System.Linq;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public class Query
    {
        private readonly Dictionary<string, QueryProperty> _properties;
        private readonly object _propertiesLock;

        public QueryProperty this[string name]
        {
            get
            {
                return GetProperty(name);
            }
        }

        public Query()
        {
            _propertiesLock = new object();
            _properties = new Dictionary<string, QueryProperty>();
        }

        private QueryProperty GetProperty(string name)
        {
            lock (_propertiesLock)
            {
                if(!_properties.ContainsKey(name))
                    _properties.Add(name, new QueryProperty(this, name));
            }

            return _properties[name];
        }

        internal void InsertProperty(QueryProperty property)
        {
            lock (_propertiesLock)
            {
                _properties.Add(property.Name, property);
            }
        }

        public override string ToString()
        {
            var propertiesString = string.Join(", ", _properties.Values.Select(v => v.GenerateExpression()));

            return string.Format("{{ {0} }}", propertiesString);
        }
    }

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

    public static class InExtensions
    {
        public static QueryProperty In<T>(this QueryProperty property, params T[] operands)
        {
            var expression = string.Format("$in : [{0}]",
                string.Join(",", 
                    operands.Select(
                        o => o is string 
                            ? string.Format("'{0}'", o) 
                            : o.ToString())));

            property.AddExpression(expression);

            return property;
        }
    }

    public static class WhereExtensions
    {
        public static QueryProperty Where(this Query query, string expression)
        {
            var property = new WhereQueryProperty(query);
            property.SetExpression(expression);

            query.InsertProperty(property);

            return property;
        }

        public static QueryProperty Where(this QueryProperty property, string expression)
        {
            return Where(property.Query, expression);
        }
    }

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