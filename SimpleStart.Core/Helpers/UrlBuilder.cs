using System;

namespace SimpleStart.Core.Helpers;

public static class UrlBuilder
{
    internal static string AppendPathSegment(this string basePath, string value)
    {
        return $"{basePath}/{value}";
    }
    internal static string SetQueryParam(this string basePath, string propertyName, string? value)
    {
        return basePath.Contains("?")
            ? $"{basePath}&{propertyName}={value}"
            : $"{basePath}?{propertyName}={value}";
    }
    internal static string SetQueryParam(this string basePath, string propertyName, DateTime value)
    {
        return basePath.Contains("?")
            ? $"{basePath}&{propertyName}={value}"
            : $"{basePath}?{propertyName}={value}";
    }
    internal static string SetQueryParam(this string basePath, string propertyName, bool? value)
    {
        return basePath.Contains("?")
            ? $"{basePath}&{propertyName}={value}"
            : $"{basePath}?{propertyName}={value}";
    }
    internal static string SetQueryParam(this string basePath, string propertyName, int value)
    {
        return basePath.Contains("?")
            ? $"{basePath}&{propertyName}={value}"
            : $"{basePath}?{propertyName}={value}";
    }
    internal static string SetQueryParam(this string basePath, string propertyName, double value)
    {
        return basePath.Contains("?")
            ? $"{basePath}&{propertyName}={value}"
            : $"{basePath}?{propertyName}={value}";
    }
}