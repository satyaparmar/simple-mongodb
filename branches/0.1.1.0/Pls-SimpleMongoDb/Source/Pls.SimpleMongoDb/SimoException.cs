using System;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public class SimoException
        : Exception
    {
        public SimoException(string message)
            : base(message)
        {
        }
    }
}