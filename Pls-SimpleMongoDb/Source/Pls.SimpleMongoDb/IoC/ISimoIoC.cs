using System;
using Pls.SimpleMongoDb.Globalization;

namespace Pls.SimpleMongoDb.IoC
{
    public interface ISimoIoC
    {
        Func<ISimoPluralizer> PluralizerFactory { set; }
        ISimoPluralizer GetPluralizer();
    }
}