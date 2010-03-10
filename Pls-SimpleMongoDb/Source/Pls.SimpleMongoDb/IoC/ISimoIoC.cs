using System;
using Pls.SimpleMongoDb.Utils;

namespace Pls.SimpleMongoDb.IoC
{
    public interface ISimoIoC
    {
        Func<ISimoPluralizer> PluralizerFactory { set; }
        ISimoPluralizer GetPluralizer();
    }
}