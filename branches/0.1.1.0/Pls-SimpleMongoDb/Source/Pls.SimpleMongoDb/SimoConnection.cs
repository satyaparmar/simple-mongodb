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
        public virtual ISimoConnectionInfo SimoConnectionInfo { get; private set; }
        public virtual bool IsConnected { get { return Socket != null && Socket.Connected; } }

        public SimoConnection(ISimoConnectionInfo simoConnectionInfo)
        {
            if (simoConnectionInfo == null)
                throw new ArgumentNullException("simoConnectionInfo");

            SimoConnectionInfo = simoConnectionInfo;
        }

        public virtual void Dispose()
        {
            Disconnect();
        }

        public virtual void Connect()
        {
            lock (LockSocket)
            {
                if (IsConnected)
                    throw new SimoCommunicationException(Exceptions.MongoConnection_AllreadyEstablished);

                Socket = new TcpClient();
                Socket.Connect(SimoConnectionInfo.Host, SimoConnectionInfo.Port);
            }
        }

        public virtual void Disconnect()
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

        public virtual Stream GetPipeStream()
        {
            if (!IsConnected)
                throw new SimoCommunicationException(Exceptions.MongoConnection_NoPipestreamWhileDisconnected);

            return Socket.GetStream();
        }
    }
}
