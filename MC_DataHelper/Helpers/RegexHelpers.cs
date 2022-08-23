using System.Text.RegularExpressions;

namespace MC_DataHelper.Helpers;

public class RegexHelpers
{
    private static readonly Regex SWhitespace = new Regex(@"\s+", RegexOptions.Compiled);

    public static string ReplaceWhitespace(string input, string replacement)
    {
        return SWhitespace.Replace(input, replacement);
    }
}