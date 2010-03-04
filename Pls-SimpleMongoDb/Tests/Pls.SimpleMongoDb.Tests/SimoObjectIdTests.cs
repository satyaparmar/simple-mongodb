using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pls.SimpleMongoDb.Tests
{
    [TestClass]
    public class SimoObjectIdTests
    {
        [TestMethod]
        public void Empty_Value_Contains12EmptyBytes()
        {
            var id = SimoObjectId.Empty;

            CollectionAssert.AreEqual(new byte[12], id.Value);
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_NullBytes_ThrowsException()
        {
            var id = new SimoObjectId(null as byte[]);

            Assert.Fail("Should have thrown exception!");
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_EmptyBytes_ThrowsException()
        {
            var id = new SimoObjectId(new byte[11]);

            Assert.Fail("Should have thrown exception!");
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_ToFewBytes_ThrowsException()
        {
            var id = new SimoObjectId(new byte[11]);

            Assert.Fail("Should have thrown exception!");
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_ToManyBytes_ThrowsException()
        {
            var id = new SimoObjectId(new byte[13]);

            Assert.Fail("Should have thrown exception!");
        }

        [TestMethod]
        public void NewId_NotEqualToEmptyObjectId()
        {
            var id = SimoObjectId.NewId();

            Assert.AreNotEqual(SimoObjectId.Empty, id);
        }

        [TestMethod]
        public void NewId_GetTwo_NotEqualToEachOther()
        {
            var id1 = SimoObjectId.NewId();
            var id2 = SimoObjectId.NewId();

            Assert.AreNotEqual(id1, id2);
        }

        [TestMethod]
        public void TimeStamp_NewId_HasTodaysUtcDate()
        {
            //TODO: Isolate date
            var id = SimoObjectId.NewId();

            Assert.AreEqual(DateTime.UtcNow.Date, id.TimeStamp.Date);
        }

        [TestMethod]
        public void IsNullOrEmpty_ForEmpty_IsTrue()
        {
            var id = SimoObjectId.Empty;

            Assert.IsTrue(SimoObjectId.IsNullOrEmpty(id));
        }

        [TestMethod]
        public void IsNullOrEmpty_ForNull_IsTrue()
        {
            Assert.IsTrue(SimoObjectId.IsNullOrEmpty(null));
        }
    }
}