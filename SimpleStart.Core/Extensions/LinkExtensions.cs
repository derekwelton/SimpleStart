using System;
using System.Linq;

namespace SimpleStart.Core.Extensions;

public static class LinkExtensions
{
    public static string ToPhoneLink(this string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return string.Empty;

        // Remove spaces, parentheses, and hyphens but keep ";" and numbers for extensions
        string cleanedPhone = new string(phone
            .Where(c => char.IsDigit(c) || c == '+' || c == ';')
            .ToArray());

        return $"tel:{cleanedPhone}";
    }
    
    public static string ToEmailLink(this string? email)
    {
        return email.IsNullOrWhiteSpace() ? string.Empty : $"mailto:{email}";
    }
    
    public static string GetDomainName(this string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return string.Empty;
        if (!url.StartsWith("http://") &&
            !url.StartsWith("https://")) url = "https://" + url; // Add "https://" by default

        try
        {
            var uri = new Uri(url);
            var domain = uri.Host;

            if (domain.StartsWith("www.")) domain = domain.Substring(4);

            return domain;
        }

        catch
        {
            return string.Empty;
        }
    }
}