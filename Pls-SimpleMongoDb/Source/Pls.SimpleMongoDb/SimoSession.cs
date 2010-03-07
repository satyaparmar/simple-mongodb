using System;

namespace Pls.SimpleMongoDb
{
    public class SimoSession
        : ISimoSession
    {
        protected virtual ISimoConnection Connection { get; private set; }

        public ISimoDatabase this[string name]
        {
            get { return GetDatabase(name); }
        }

        public SimoSession(ISimoConnection connection)
        {
            Connection = connection;
        }

        #region Object lifetime, Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A call to Dispose(false) should only clean up native resources.
        /// A call to Dispose(true) should clean up both managed and native resources.
        /// </summary>
        /// <param name="disposeManagedResources"></param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
                DisposeManagedResources();
        }

        ~SimoSession()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }

        #endregion

        protected virtual ISimoDatabase GetDatabase(string name)
        {
            var db = new SimoDatabase(Connection, name);

            return db;
        }
    }
}