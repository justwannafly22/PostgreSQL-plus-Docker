using System.Text.RegularExpressions;

namespace WebAggregator.Infrastructure.Helpers;

/// <summary>
/// Specially created String Helper cause don`t want to extend base string class.
/// </summary>
public static class StringHelper
{
    public static string RemoveNonAlphanumeric(string input)
    {
        var pattern = @"[^\p{L}\p{N},.!?\s]";
        var result = Regex.Replace(input, pattern, " ");

        return Regex.Replace(result, @"\s+", " ").Trim();
    }

    public static string ExtractUrl(string input)
    {
        var pattern = @"<a\s+[^>]*\bhref\s*=\s*['""]?([^'"">\s]+)['""]?[^>]*>";
        Match match = Regex.Match(input, pattern);

        if (match.Success && match.Groups.Count > 1)
        {
            Uri uri;
            if (Uri.TryCreate(match.Groups[1].Value, UriKind.Relative, out uri!))
            {
                return uri.OriginalString;
            }
        }

        return string.Empty;
    }

    public static string ReturnBaseUrl(string url)
    {
        if (IsValidUrl(url))
        {
            return ReplaceString(url);
        }
        else return url;

        static string ReplaceString(string inputString)
        {
            var patternToKeep = @"https?://\w+\.\w+";
            var match = Regex.Match(inputString, patternToKeep);
            var resultString = match.Success ? match.Value : inputString;
            return resultString;
        }

        static bool IsValidUrl(string url)
        {
            var combinedPattern = $"^(https?|ftp):\\/\\/[^\\s/$.?#].[^\\s]*news[^\\s]*\\/?$|https?://\\w+\\.\\w+";
            var regex = new Regex(combinedPattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(url);
        }
    }
}
