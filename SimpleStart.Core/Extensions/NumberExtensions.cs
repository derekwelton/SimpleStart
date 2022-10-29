using System;

namespace SimpleStart.Core.Extensions
{
    public static class NumberExtensions
    {
        public static double GetValidAngle(this double value)
        {
            return Math.Round(value * (180.0 / Math.PI), 3);
        }
        public static bool IsAngleBetween(this double value, double minAngle, double maxAngle)
        {
            return value >= minAngle && value <= maxAngle;
        }
        public static bool IsWholeNumber(this double value)
        {
            return Math.Abs(value % 1) <= (Double.Epsilon * 100);
        }
        public static bool IsWholeNumber(this float value)
        {
            return Math.Abs(value % 1) <= (Double.Epsilon * 100);
        }
        public static double GetDecimalPortion(this double value)
        {
            return value - Math.Truncate(value);
        }
        public static bool HasMoreThan3DecimalPlaces(this double value)
        {
            value = value * 1000;
            return !value.IsWholeNumber();
        }
        public static double RoundTo3Decimals(this double value)
        {
            return Math.Round(value, 3);
        }
    }
}