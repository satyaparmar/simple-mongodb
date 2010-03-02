using System;

namespace Pls.SimpleMongoDb.Commands
{
    [Serializable]
    public class SimoCommandException
        : SimoException
    {
        public SimoCommandException(string message) : base(message)
        {
        }
    }
}