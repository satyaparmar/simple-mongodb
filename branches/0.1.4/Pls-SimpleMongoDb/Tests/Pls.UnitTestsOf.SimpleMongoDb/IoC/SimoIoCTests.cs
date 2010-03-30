using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Globalization;
using Pls.SimpleMongoDb.IoC;

namespace Pls.UnitTestsOf.SimpleMongoDb.IoC
{
    [TestClass]
    public class SimoIoCTests
    {
        [TestMethod]
        public void PluralizerFactory_Default_IsSimoPluralizer()
        {
            var pluralizer = new SimoIoC().GetPluralizer();

            Assert.IsInstanceOfType(pluralizer, typeof(SimoPluralizer));
        }
    }
}


