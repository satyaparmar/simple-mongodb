using System;

namespace Pls.SimpleMongoDb
{
    public class SimoDatabase
        : ISimoDatabase
    {
        public virtual ISimoConnection Connection { get; private set; }
        
        public virtual string Name { get; private set; }

        public virtual ISimoCollection this[string name]
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