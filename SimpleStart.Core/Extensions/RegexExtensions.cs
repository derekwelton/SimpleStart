using System.Text.RegularExpressions;

namespace SimpleStart.Core.Extensions;

public static class RegexExtensions
{
    private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
    private static readonly Regex SpecialCharactersRegex = new Regex(@"[^a-zA-Z0-9_.]+", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
    private static readonly Regex PhoneNumberRegex = new Regex(@"^(\(\d{3}\)|\d{3})[-.\s]?\d{3}[-.\s]?\d{4}(;\d+)?$", RegexOptions.Compiled);
    private static readonly Regex UrlRegex = new Regex(@"^(http|https)://[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static string ReplaceWhitespace(this string input, string replacement)
    {
        return string.IsNullOrEmpty(input) ? input : WhitespaceRegex.Replace(input, replacement);
    }

    public static bool IsValidEmail(this string email)
    {
        return !string.IsNullOrEmpty(email) && EmailRegex.IsMatch(email);
    }
    
    public static bool IsValidPhoneNumber(this string email)
    {
        return !string.IsNullOrEmpty(email) && PhoneNumberRegex.IsMatch(email);
    }

    public static bool IsValidUrl(this string url)
    {
        return !string.IsNullOrEmpty(url) && UrlRegex.IsMatch(url);
    }
    
    public static string RemoveWhitespace(this string str)
    {
        return Regex.Replace(str, WhitespaceRegex.ToString(), string.Empty, RegexOptions.Compiled);
    }
    public static string RemoveSpecialCharacters(this string str)
    {
        return Regex.Replace(str, SpecialCharactersRegex.ToString(), string.Empty, RegexOptions.Compiled);
    }
    
    
}