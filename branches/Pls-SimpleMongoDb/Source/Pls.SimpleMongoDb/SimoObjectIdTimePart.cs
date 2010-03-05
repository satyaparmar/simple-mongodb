using System;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    internal class SimoObjectIdTimePart
    {
        private const int BytesLen = 4;
        private static readonly DateTime TimeZero = new DateTime(1970, 1, 1);

        internal byte[] Value { get; private set; }

        internal SimoObjectIdTimePart(byte[] value = null)
        {
            if (value != null && value.Length != BytesLen)
                throw new ArgumentException(string.Format(Exceptions.SimoObjectIdTimePart_InvalidBytesLen));

            Value = value ?? Generate();
        }

        private byte[] Generate()
        {
            var diff = (int)Math.Ceiling((DateTime.UtcNow - TimeZero).TotalSeconds);

            return BitConverter.GetBytes(diff);
        }

        internal DateTime ConvertToDateTime()
        {
            var diff = BitConverter.ToInt32(Value, 0);
            return TimeZero.AddSeconds(diff);
        }
    }
}