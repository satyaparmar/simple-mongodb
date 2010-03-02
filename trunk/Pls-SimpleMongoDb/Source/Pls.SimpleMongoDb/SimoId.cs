using System;
using System.Collections.Generic;
using System.ComponentModel;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Represents the ObjectId found in MongoDb.
    /// Currently Simple-MongoDb does not generate
    /// any Ids. Instead these are assigned in the
    /// MongoDb-database.
    /// </summary>
    [Serializable, TypeConverter(typeof(SimoIdTypeConverter))]
    public class SimoId
        : IEquatable<SimoId>
    {
        private byte[] _value;
        private string _valueAsString;

        public static SimoId Empty
        {
            get
            {
                return new SimoId();
            }
        }

        public virtual byte[] Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _valueAsString = BitConverter.ToString(Value).Replace("-", "").ToLower();
            }
        }

        public SimoId()
        {
            Value = new byte[12];
        }

        public SimoId(string id)
        {
            Value = ConvertStringToIdBytes(id);
        }

        private byte[] ConvertStringToIdBytes(string id)
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

        public override bool Equals(object obj)
        {
            return Equals(obj as SimoId);
        }

        public virtual bool Equals(SimoId other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return _valueAsString;
        }

        public static implicit operator string(SimoId id)
        {
            return id.ToString();
        }

        public static implicit operator byte[](SimoId id)
        {
            return id.Value;
        }

        public static implicit operator SimoId(byte[] value)
        {
            return new SimoId { Value = value };
        }

        public static implicit operator SimoId(string id)
        {
            return new SimoId(id);
        }
    }
}
