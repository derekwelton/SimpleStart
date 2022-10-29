using System.Text.RegularExpressions;

namespace SimpleStart.Core.Extensions;

public static class RegexExtensions
{
    private static readonly Regex Whitespace = new Regex(@"\s+");
    private static readonly Regex SpecialCharacters = new Regex(@"[^a-zA-Z0-9_.]+");
    private static readonly Regex NumbersDashOnly = new Regex(@"^[0-9-]*$");
    private static readonly Regex PhoneNumber = new Regex(@"\(?\b\d{3}\)?[-.)]?\d{3}[-.]?\d{4}\b");

    public static string ReplaceWhitespace(this string input, string replacement)
    {
        return Whitespace.Replace(input, replacement);
    }
    public static string RemoveWhitespace(this string str)
    {
        return Regex.Replace(str, Whitespace.ToString(), string.Empty, RegexOptions.Compiled);
    }
    public static string RemoveSpecialCharacters(this string str)
    {
        return Regex.Replace(str, SpecialCharacters.ToString(), string.Empty, RegexOptions.Compiled);
    }

    public static bool IsValidPhoneNumber(this string str)
    {
        var match = PhoneNumber.Match(str);
        return match.Success;
    }
}