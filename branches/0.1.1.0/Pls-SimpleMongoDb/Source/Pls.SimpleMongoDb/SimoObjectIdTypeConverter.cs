using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pls.SimpleMongoDb
{
    public class SimoObjectIdTypeConverter : TypeConverter
    {
        private static readonly HashSet<Type> TypesConvertibleFrom = new HashSet<Type> { typeof(byte[]), typeof(string) };
        private static readonly HashSet<Type> TypesConvertibleTo = new HashSet<Type> { typeof(byte[]), typeof(string) };

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return TypesConvertibleFrom.Contains(sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return TypesConvertibleTo.Contains(destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is byte[])
                return (SimoObjectId)(value as byte[]);

            if (value is string)
                return (SimoObjectId)(value as string);

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var mongoId = (SimoObjectId)value;

            if (destinationType == typeof(byte[]))
                return mongoId.Value;

            if (destinationType == typeof(string))
                return mongoId.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}