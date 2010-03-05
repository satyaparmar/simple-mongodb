using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Represents and is compatible with the ObjectId found in MongoDb.
    /// If you want you can use another Id that is unique.
    /// </summary>
    [Serializable, JsonConverter(typeof(SimoObjectIdJsonConverter))]
    public class SimoObjectId
        : IEquatable<SimoObjectId>
    {
        private byte[] _value;
        private string _valueAsString = string.Empty;
        private static readonly SimoObjectIdBytesGenerator Generator = new SimoObjectIdBytesGenerator();

        public static SimoObjectId Empty
        {
            get
            {
                return new SimoObjectId(new byte[12], emptyBytesAreValid: true);
            }
        }

        public virtual DateTime TimeStamp { get; private set; }

        public virtual byte[] Value
        {
            get
            {
                return _value;
            }
            private set
            {
                _value = value;
                _valueAsString = BitConverter.ToString(Value).Replace("-", "").ToLower();
                TimeStamp = new SimoObjectIdTimePart(_value.Take(4).ToArray()).ConvertToDateTime();
            }
        }

        public SimoObjectId(string id)
            : this(ConvertStringToIdBytes(id), emptyBytesAreValid: false)
        {
        }

        public SimoObjectId(byte[] value)
            : this(value, emptyBytesAreValid: false)
        {}

        private SimoObjectId(byte[] value, bool emptyBytesAreValid)
        {
            EnsureValidByteArray(value, emptyBytesAreValid);
            Value = value;
        }

        public static SimoObjectId NewId()
        {
            return new SimoObjectId(Generator.New());
        }

        private static byte[] ConvertStringToIdBytes(string id)
        {
            id = id.Replace("-", "").ToLower();
            if (id.Length != 24)
                throw new ArgumentException(Exceptions.MongoId_FromStringInvalidLen);

            var valueBbytes = new List<byte>(12);
            for (var c = 0; c < id.Length; c += 2)
            {
                var hexString = id.Substring(c, 2);
                var num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                valueBbytes.Add((byte)num);
            }

            return valueBbytes.ToArray();
        }

        private static void EnsureValidByteArray(byte[] bytes, bool emptyBytesAreValid)
        {
            if (bytes == null || bytes.Length != 12)
                throw new SimoException(Exceptions.SimoObjectId_InvalidBytes);

            if (emptyBytesAreValid)
                return;

            if (IdBytesAreEmpty(bytes))
                throw new SimoException(Exceptions.SimoObjectId_InvalidBytes);
        }

        private static bool IdBytesAreEmpty(byte[] value)
        {
            var emptyByte = (byte)0;

            return value.Where(b => b.Equals(emptyByte)).Count() == value.Length;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SimoObjectId);
        }

        public virtual bool Equals(SimoObjectId other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Value.Length != other.Value.Length)
                return false;

            for (var c = 0; c < Value.Length; c++)
            {
                var bThis = Value[c];
                var bOther = other.Value[c];

                if (!bThis.Equals(bOther))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return _valueAsString;
        }

        public static implicit operator string(SimoObjectId objectId)
        {
            return objectId.ToString();
        }

        public static implicit operator byte[](SimoObjectId objectId)
        {
            return objectId.Value;
        }

        public static implicit operator SimoObjectId(byte[] value)
        {
            return new SimoObjectId(value);
        }

        public static implicit operator SimoObjectId(string id)
        {
            return new SimoObjectId(id);
        }

        public static bool IsNullOrEmpty(SimoObjectId id)
        {
            return IsNull(id) || IsEmpty(id);
        }

        public static bool IsNull(SimoObjectId id)
        {
            return id == null || id.Value == null;
        }

        public static bool IsEmpty(SimoObjectId id)
        {
            return id.Equals(Empty);
        }
    }
}
