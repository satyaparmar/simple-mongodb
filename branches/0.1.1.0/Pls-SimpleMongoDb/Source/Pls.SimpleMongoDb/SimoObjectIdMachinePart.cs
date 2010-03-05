using System;
using System.Linq;
using System.Security.Cryptography;
using Pls.SimpleMongoDb.Resources;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb
{
    internal class SimoObjectIdMachinePart
    {
        private const int BytesLen = 3;
        private static readonly SHA1 Hasher = new SHA1CryptoServiceProvider();

        internal byte[] Value { get; private set; }

        internal SimoObjectIdMachinePart(byte[] value = null)
        {
            if (value != null && value.Length != BytesLen)
                throw new ArgumentException(string.Format(Exceptions.SimoObjectIdMachinePart_InvalidBytesLen));

            Value = value ?? Generate();
        }

        private byte[] Generate()
        {
            var name = Environment.MachineName;

            return Hasher.ComputeHash(SerializationConsts.DefaultEncoding.GetBytes(name)).Take(BytesLen).ToArray();
        }
    }
}