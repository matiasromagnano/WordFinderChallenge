using WordFinderChallenge.Core.Models;

namespace WordFinderChallenge.Core.Extensions;

public static class ModelExtensions
{
    public static IEnumerable<WordOccurrences> ToWordOccurrences(this IEnumerable<KeyValuePair<string, int>> source)
    {
        return source.Select(kvp => new WordOccurrences
        {
            Word = kvp.Key,
            Ocurrences = kvp.Value
        });
    }
}
