using System;

namespace SimpleStart.Core.Extensions;

/// <summary>
/// Provides extension methods for string operations and conversions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a string to an integer. Returns 0 if conversion fails.
    /// </summary>
    /// <param name="value">The string to convert</param>
    /// <returns>The converted integer or 0 if conversion fails</returns>
    public static int ToInt(this string? value)
    {
        return string.IsNullOrEmpty(value) ? 0 : int.TryParse(value, out var output) ? output : 0;
    }
    
    /// <summary>
    /// Converts a string to an integer. Returns the specified default value if conversion fails.
    /// </summary>
    /// <param name="value">The string to convert</param>
    /// <param name="defaultValue">The default value to return if conversion fails</param>
    /// <returns>The converted integer or the default value if conversion fails</returns>
    public static int ToInt(this string? value, int defaultValue)
    {
        return string.IsNullOrEmpty(value) ? defaultValue : int.TryParse(value, out var output) ? output : defaultValue;
    }
    
    /// <summary>
    /// Safely trims a string, handling null values.
    /// </summary>
    /// <param name="value">The string to trim</param>
    /// <returns>The trimmed string or empty string if null</returns>
    public static string SafeTrim(this string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
    
    /// <summary>
    /// Truncates a string to the specified maximum length.
    /// </summary>
    /// <param name="value">The string to truncate</param>
    /// <param name="maxLength">The maximum length</param>
    /// <param name="suffix">Optional suffix to append when truncated</param>
    /// <returns>Truncated string</returns>
    public static string Truncate(this string? value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value != null && value.Length <= maxLength ? value : value?.Substring(0, maxLength) + suffix;
    }
    
    
    public static double ToDouble(this string? value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            double.TryParse(value, out var output);
            return output;

        }
    public static decimal ToDecimal(this string? value)
    {
        if (string.IsNullOrEmpty(value)) return 0;
        decimal.TryParse(value, out var output);
        return output;

    }
    public static bool ToBool(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            value = value?.ToLower();
            value = value?.Replace(" ", "");
            return value == "true" || value == "yes";
        }
    public static DateTime ToDateTime(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return DateTime.MinValue;
            DateTime.TryParse(value, out DateTime output);
            return output;
        }
    public static double ConvertFractionToDouble(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;
            if (input != null && input.Contains("\"")) input = input.Replace("\"", "");

            input = (input ?? String.Empty).Trim();
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            // standard decimal number (e.g. 1.125)
            if ((input.IndexOf('.') != -1 || (input.IndexOf(' ') == -1 && input.IndexOf('/') == -1 && input.IndexOf('\\') == -1)))
            {
                if (Double.TryParse(input, out var result))
                {
                    return result;
                }
            }

            String[] parts = input.Split(new[] { ' ', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            // stand-off fractional (e.g. 7/8)
            if (input.IndexOf(' ') == -1 && parts.Length == 2)
            {
                if (Double.TryParse(parts[0], out var num) && Double.TryParse(parts[1], out var den))
                {
                    return num / den;
                }
            }

            // Number and fraction (e.g. 2 1/2)
            if (parts.Length == 3)
            {
                if (Double.TryParse(parts[0], out var whole) && Double.TryParse(parts[1], out var num) && Double.TryParse(parts[2], out var den))
                {
                    return whole + (num / den);
                }
            }

            // Bogus / unable to parse
            return Double.NaN;
        }
    
    
    public static bool IsNullOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static bool IsNullOrEmpty(this string? input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static bool IsNotNullOrEmpty(this string? input)
    {
        return !string.IsNullOrEmpty(input);
    }

    public static bool IsNotNullOrWhiteSpace(this string? input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }
}