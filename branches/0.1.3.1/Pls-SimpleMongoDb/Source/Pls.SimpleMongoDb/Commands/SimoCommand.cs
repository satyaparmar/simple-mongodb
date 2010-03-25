using System;
using Pls.SimpleMongoDb.Resources;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    /// <summary>
    /// Base-class which you can build your commands upon.
    /// </summary>
    public abstract class SimoCommand
    {
        public ISimoConnection Connection { get; set; }

        /// <summary>
        /// Defines which Node in the DB that the command should be executed against.
        /// Most often this is FullCollectionName, e.g
        /// <![CDATA["dbname.collectionname"]]>.
        /// </summary>
        public string NodeName { get; set; }

        protected SimoCommand(ISimoConnection connection)
        {
            Connection = connection;
        }

        public void Execute()
        {
            EnsureOpenConnection();
            OnEnsureValidForExecution();
            OnExecute(Connection);
        }

        protected abstract void OnEnsureValidForExecution();

        private void EnsureOpenConnection()
        {
            if (!Connection.IsConnected)
                Connection.Connect();
        }

        protected virtual void OnExecute(ISimoConnection connection)
        {
            var request = GenerateRequest();

            using (var requestStream = connection.GetPipeStream())
            {
                using (var requestWriter = new RequestWriter(requestStream))
                {
                    requestWriter.Write(request);

                    if (!(this is ISimoResponseCommand))
                        return;

                    using (var responseStream = Connection.GetPipeStream())
                    {
                        using (var responseReader = new ResponseStreamReader(responseStream))
                        {
                            OnReadResponse(responseReader);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Implement if the command is supposed to handle responses from MongoDb.
        /// </summary>
        /// <param name="responseStreamReader"></param>
        protected virtual void OnReadResponse(ResponseStreamReader responseStreamReader)
        {
            throw new NotSupportedException(ExceptionMessages.MongoCommand_OnReadResponseNotImplemented);
        }

        /// <summary>
        /// Should generate the Request that will be serialized
        /// to the Network stream, hence sent to the server.
        /// </summary>
        /// <returns></returns>
        protected abstract Request GenerateRequest();
    }
}