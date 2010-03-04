using System;
using System.Diagnostics;
using System.Linq;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    internal class SimoObjectIdProcessPart
    {
        private const int BytesLen = 2;

        internal byte[] Value { get; private set; }

        internal SimoObjectIdProcessPart(byte[] value = null)
        {
            if (value != null && value.Length != BytesLen)
                throw new ArgumentException(string.Format(Exceptions.SimoObjectIdProcessPart_InvalidBytesLen, BytesLen));

            Value = value ?? Generate();
        }

        private byte[] Generate()
        {
            return BitConverter.GetBytes(Process.GetCurrentProcess().Id).Take(BytesLen).ToArray();
        }
    }
}