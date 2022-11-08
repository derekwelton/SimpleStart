using System;

namespace SimpleStart.Core.Extensions;

public static class StringExtensions
{
    public static int ToInt(this string value)
        {
            int.TryParse(value, out var output);
            return output;
        }
    public static double ToDouble(this string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            double.TryParse(value, out var output);
            return output;

        }
    public static decimal ToDecimal(this string value)
    {
        if (string.IsNullOrEmpty(value)) return 0;
        decimal.TryParse(value, out var output);
        return output;

    }
    public static bool ToBool(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            value = value.ToLower();
            value = value.Replace(" ", "");
            return value == "true" || value == "yes";
        }
    public static DateTime ToDateTime(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return DateTime.MinValue;
            DateTime.TryParse(value, out DateTime output);
            return output;
        }
    public static double ConvertFractionToDouble(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;
            if (input.Contains("\"")) input = input.Replace("\"", "");

            input = (input ?? String.Empty).Trim();
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            // standard decimal number (e.g. 1.125)
            if (input.IndexOf('.') != -1 || (input.IndexOf(' ') == -1 && input.IndexOf('/') == -1 && input.IndexOf('\\') == -1))
            {
                Double result;
                if (Double.TryParse(input, out result))
                {
                    return result;
                }
            }

            String[] parts = input.Split(new[] { ' ', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            // stand-off fractional (e.g. 7/8)
            if (input.IndexOf(' ') == -1 && parts.Length == 2)
            {
                Double num, den;
                if (Double.TryParse(parts[0], out num) && Double.TryParse(parts[1], out den))
                {
                    return num / den;
                }
            }

            // Number and fraction (e.g. 2 1/2)
            if (parts.Length == 3)
            {
                Double whole, num, den;
                if (Double.TryParse(parts[0], out whole) && Double.TryParse(parts[1], out num) && Double.TryParse(parts[2], out den))
                {
                    return whole + (num / den);
                }
            }

            // Bogus / unable to parse
            return Double.NaN;
        }
}