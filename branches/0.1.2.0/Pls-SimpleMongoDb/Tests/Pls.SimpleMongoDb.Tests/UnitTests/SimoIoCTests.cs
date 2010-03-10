using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.IoC;
using Pls.SimpleMongoDb.Utils;

namespace Pls.SimpleMongoDb.Tests.UnitTests
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
