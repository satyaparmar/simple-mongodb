using System;
using System.IO;
using System.Net.Sockets;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Used to establish connection to a MongoDB-server
    /// by communication using TCP-sockets.
    /// </summary>
    public class SimoConnection
        : ISimoConnection
    {
        protected readonly object LockSocket = new object();

        protected virtual TcpClient Socket { get; set; }
        public ISimoConnectionInfo SimoConnectionInfo { get; private set; }
        public bool IsConnected { get { return Socket != null && Socket.Connected; } }

        public SimoConnection(ISimoConnectionInfo simoConnectionInfo)
        {
            if (simoConnectionInfo == null)
                throw new ArgumentNullException("simoConnectionInfo");

            SimoConnectionInfo = simoConnectionInfo;
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

        ~SimoConnection()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
            Disconnect();
        }

        #endregion

        public void Connect()
        {
            lock (LockSocket)
            {
                if (IsConnected)
                    throw new SimoCommunicationException(Exceptions.MongoConnection_AllreadyEstablished);

                Socket = new TcpClient();
                Socket.Connect(SimoConnectionInfo.Host, SimoConnectionInfo.Port);
            }
        }

        public void Disconnect()
        {
            lock (LockSocket)
            {
                if (Socket != null)
                {
                    if (Socket.Connected)
                        Socket.Close();

                    Socket = null;
                }
            }
        }

        public Stream GetPipeStream()
        {
            if (!IsConnected)
                throw new SimoCommunicationException(Exceptions.MongoConnection_NoPipestreamWhileDisconnected);

            return Socket.GetStream();
        }
    }
}
