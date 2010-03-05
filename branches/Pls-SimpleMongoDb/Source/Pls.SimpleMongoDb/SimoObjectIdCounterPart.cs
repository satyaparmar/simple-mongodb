using System;
using System.Linq;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    internal class SimoObjectIdCounterPart
    {
        private const int BytesLen = 3;

        private static readonly object CounterLock;
        private static int _counter;

        internal byte[] Value { get; private set; }

        static SimoObjectIdCounterPart()
        {
            CounterLock = new object();
        }

        internal SimoObjectIdCounterPart(byte[] value = null)
        {
            if (value != null && value.Length != BytesLen)
                throw new ArgumentException(string.Format(Exceptions.SimoObjectIdCounterPart_InvalidBytesLen, BytesLen));

            Value = value ?? Generate();
        }

        private byte[] Generate()
        {
            lock (CounterLock)
            {
                _counter += 1;

                return BitConverter.GetBytes(_counter).Take(BytesLen).ToArray();
            }
        }
    }
}