using System;
using Pls.SimpleMongoDb.Exceptions;

namespace Pls.SimpleMongoDb.Commands
{
    [Serializable]
    public class SimoCommandException
        : SimoException
    {
        public string MongoDbMessage { get; private set; }

        public SimoCommandException(string message, string mongoDbMessage = null) : base(message)
        {
            MongoDbMessage = mongoDbMessage ?? string.Empty;
        }
    }
}