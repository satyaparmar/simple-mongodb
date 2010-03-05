using System;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pls.SimpleMongoDb.Tests
{
    [TestClass]
    public class MongoConnectionTests
        : TestBase
    {
        private ISimoConnection _connection;

        [TestCleanup]
        public void TestCleanup()
        {
            if (_connection != null && _connection.IsConnected)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        [TestMethod]
        public void Connect_ServerIsUp_CanConnect()
        {
            _connection = CreateConnection();

            _connection.Connect();

            Assert.IsTrue(_connection.IsConnected);
        }

        [TestMethod, ExpectedException(typeof(SocketException))]
        public void Connect_NoServerIsUp_ThrowsException()
        {
            var dummyHost = Guid.NewGuid().ToString();
            var dummyConnectionInfo = new SimoConnectionInfo(dummyHost, SimoConnectionInfo.DefaultPort);
            _connection = new SimoConnection(dummyConnectionInfo);

            _connection.Connect();

            Assert.Fail("Connect should have generated exception.");
        }

        [TestMethod, ExpectedException(typeof(SimoCommunicationException))]
        public void Connect_WhileConnected_ThrowsException()
        {
            _connection = CreateConnection();
            _connection.Connect();

            _connection.Connect();

            Assert.IsTrue(_connection.IsConnected);
        }

        [TestMethod]
        public void Disconnect_WhenConnected_GetsDisconnected()
        {
            _connection = CreateConnection();
            _connection.Connect();

            _connection.Disconnect();

            Assert.IsFalse(_connection.IsConnected);
        }

        [TestMethod]
        public void Dispose_WhenConnected_GetsDisconnected()
        {
            _connection = CreateConnection();
            _connection.Connect();

            _connection.Dispose();

            Assert.IsFalse(_connection.IsConnected);
        }

        [TestMethod]
        public void GetPipeStream_WhenConnected_ReturnsStream()
        {
            _connection = CreateConnection();
            _connection.Connect();

            using(var stream = _connection.GetPipeStream())
            {
                Assert.IsNotNull(stream);
            }
        }

        [TestMethod, ExpectedException(typeof(SimoCommunicationException))]
        public void GetPipeStream_WhileNotConnected_ThrowsException()
        {
            _connection = CreateConnection();

            var stream = _connection.GetPipeStream();

            Assert.IsNull(stream, "Pipestream shall not be returned when not connected!");
        }
    }
}