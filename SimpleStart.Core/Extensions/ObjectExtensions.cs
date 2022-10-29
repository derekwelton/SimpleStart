using System;

namespace SimpleStart.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object? value)
        {
            return value is null;
        }
        public static bool IsNotNull(this object? value)
        {
            return value switch
            {
                string s => !string.IsNullOrWhiteSpace(s),
                double d => d > 0,
                int i => i > 0,
                _ => value != null
            };
        }
        public static string ToStringValue(this object value)
        {
            try
            {
                if (value == null) return string.Empty;
                return value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public static double ToDoubleValue(this object value)
        {
            return value.ToStringValue().ToDouble();
        }
        public static int ToIntValue(this object value)
        {
            return value.ToStringValue().ToInt();
        }
        public static bool ToBoolValue(this object value)
        {
            return value.ToStringValue().ToBool();
        }
        public static DateTime ToDateTimeValue(this object value)
        {
            return value.ToStringValue().ToDateTime();
        }
    }
}