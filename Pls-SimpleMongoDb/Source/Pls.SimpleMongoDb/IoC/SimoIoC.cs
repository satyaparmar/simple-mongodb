using System;
using Pls.SimpleMongoDb.Globalization;

namespace Pls.SimpleMongoDb.IoC
{
    public class SimoIoC : ISimoIoC
    {
        public Func<ISimoPluralizer> PluralizerFactory { private get; set; }

        public SimoIoC()
        {
            PluralizerFactory = () => new SimoPluralizer();
        }

        public ISimoPluralizer GetPluralizer()
        {
            return PluralizerFactory();
        }
    }
}