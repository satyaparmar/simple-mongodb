using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Exceptions;

namespace Pls.SimpleMongoDb.Tests.UnitTests
{
    [TestClass]
    public class SimoConnectionInfoTests
    {
        [TestMethod]
        public void Ctor_WithAppConfig_UsingConnectionStringName()
        {
            var connectionInfo = new SimoConnectionInfo(Constants.ConnectionStringName);

            Assert.AreEqual(27017, connectionInfo.Port);
            Assert.AreEqual("localhost", connectionInfo.Host);
        }

        [TestMethod, ExpectedException(typeof(SimoCommunicationException))]
        public void Ctor_NoAppConfig_UsingConnectionStringName()
        {
            var dummyConnectionStringName = Guid.NewGuid().ToString();
            var connectionInfo = new SimoConnectionInfo(dummyConnectionStringName);

            Assert.IsNull(connectionInfo, "Should have failed.");
        }

        [TestMethod]
        public void Ctor_ManualHostAndPort_MappsCorrectly()
        {
            var connectionInfo = new SimoConnectionInfo("thehost", 12345);

            Assert.AreEqual(12345, connectionInfo.Port);
            Assert.AreEqual("thehost", connectionInfo.Host);
        }

        [TestMethod]
        public void Ctor_DefaultMethodFactory_GivesDefaultHostAndPort()
        {
            var connectionInfo = SimoConnectionInfo.Default;

            Assert.AreEqual(SimoConnectionInfo.DefaultPort, connectionInfo.Port);
            Assert.AreEqual(SimoConnectionInfo.DefaultHost, connectionInfo.Host);
        }
    }
}