using System;

namespace Pls.SimpleMongoDb
{
    public class SimoDatabase
        : ISimoDatabase
    {
        public ISimoConnection Connection { get; private set; }
        
        public string Name { get; private set; }

        public ISimoCollection this[string name]
        {
            get { return GetCollection(name); }
        }

        public SimoDatabase(ISimoConnection connection, string name)
        {
            Connection = connection;
            Name = name;
        }

        protected virtual ISimoCollection GetCollection(string name)
        {
            var collection = new SimoCollection(this, name);

            return collection;
        }
    }
}