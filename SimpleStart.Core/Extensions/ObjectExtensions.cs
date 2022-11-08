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
                decimal d => d > 0,
                int i => i > 0,
                _ => value != null
            };
        }
        public static string TryToString(this object value)
        {
            try
            {
                if (value == null) return string.Empty;
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static double TryToDouble(this object value)
        {
            try
            {
                if (value == null) return 0;
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
            
        }
        public static decimal TryToDecimal(this object value)
        {
            try
            {
                return value == null ? 0 : Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
            
        }
        public static int TryToInt(this object value)
        {
            try
            {
                return value == null ? 0 : Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }
        public static bool TryToBool(this object value)
        {
            return value != null && value.TryToString().ToBool();
        }
        public static DateTime TryToDateTime(this object value)
        {
            try
            {
                return value == null ? DateTime.MinValue : Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}