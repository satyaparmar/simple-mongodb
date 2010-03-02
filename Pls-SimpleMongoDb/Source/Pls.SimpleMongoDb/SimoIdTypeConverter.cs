using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pls.SimpleMongoDb
{
    public class SimoIdTypeConverter : TypeConverter
    {
        private static HashSet<Type> _canConvertFrom = new HashSet<Type> { typeof(byte[]), typeof(string) };
        private static HashSet<Type> _canConvertTo = new HashSet<Type> { typeof(byte[]), typeof(string) };

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return _canConvertFrom.Contains(sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return _canConvertTo.Contains(destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is byte[])
                return (SimoId)(value as byte[]);

            if(value is string)
                return (SimoId)(value as string);

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var mongoId = (SimoId)value;

            if (destinationType == typeof(byte[]))
                return mongoId.Value;

            if (destinationType == typeof(string))
                return mongoId.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}