using System.Text.RegularExpressions;

namespace WordFinderChallenge.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Counts how many times a word appears in a given text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="word"></param>
    /// <returns></returns>
    public static int CountOccurrences(this string text, string word)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(word))
            return 0;

        var matches = Regex.Matches(text, Regex.Escape(word));

        return matches.Count;
    }
}
