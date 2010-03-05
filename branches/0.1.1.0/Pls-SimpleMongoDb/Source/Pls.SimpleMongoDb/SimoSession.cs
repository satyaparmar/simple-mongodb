using System.IO;
using Pls.SimpleMongoDb.Commands;

namespace Pls.SimpleMongoDb
{
    public class SimoSession
        : ISimoSession
    {
        protected virtual ISimoConnection Connection { get; private set; }

        public virtual ISimoDatabase this[string name]
        {
            get { return GetDatabase(name); }
        }

        public SimoSession(ISimoConnection connection)
        {
            Connection = connection;
        }

        public virtual void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }

        protected virtual ISimoDatabase GetDatabase(string name)
        {
            var db = new SimoDatabase(Connection, name);

            return db;
        }
    }
}