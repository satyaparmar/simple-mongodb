using System;

namespace Pls.SimpleMongoDb.Utils
{
    public static class StringExtensions
    {
        public static string GetPartOrDefault(this string value, string separator, int index)
        {
            value = value ?? string.Empty;

            var parts = value.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            
            return index > parts.Length ? string.Empty : parts[index];
        }
    }
}