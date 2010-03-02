using System;

namespace Pls.SimpleMongoDb
{
    public interface ISimoSession : IDisposable
    {
        ISimoDatabase this[string name] { get; }
    }
}