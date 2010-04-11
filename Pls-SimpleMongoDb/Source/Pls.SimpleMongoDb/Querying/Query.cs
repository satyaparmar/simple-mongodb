using System;
using System.Collections.Generic;
using System.Linq;

namespace Pls.SimpleMongoDb.Querying
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

        private Query(Action<Query> initialize)
        {
            _propertiesLock = new object();
            _properties = new Dictionary<string, QueryProperty>();

            initialize(this);
        }

        public static Query New(Action<Query> initialize)
        {
            return new Query(initialize);
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
}