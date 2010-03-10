using System;
using Pls.SimpleMongoDb.Utils;

namespace Pls.SimpleMongoDb
{
    public interface ISimoSession : IDisposable
    {
        ISimoConnection Connection { get; }
        ISimoPluralizer Pluralizer { get; set; }
        ISimoDatabase this[string name] { get; }
    }
}