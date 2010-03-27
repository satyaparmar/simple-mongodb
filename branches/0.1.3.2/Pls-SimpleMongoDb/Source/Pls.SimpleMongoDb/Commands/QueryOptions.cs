using System;

namespace Pls.SimpleMongoDb.Commands
{
    [Serializable]
    public enum QueryOptions : int
    {
        None = 0,
        TailableCursor = 2,
        SlaveOk = 4,
        NoCursorTimeOut = 16
    }
}