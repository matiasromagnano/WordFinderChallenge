using System.Collections.Concurrent;
using WordFinderChallenge.Core.Enums;
using WordFinderChallenge.Core.Extensions;
using WordFinderChallenge.Core.Models;

namespace WordFinderChallenge.Core.Services;

public class WordFinder
{
    private readonly List<string> _matrixData;
    private readonly int _matrixWidth;
    private readonly int _topWordsCount;

    public WordFinder(IEnumerable<string> matrix, int maxSize = 64, int topWordsCount = 10)
    {
        var validationResult = ValidateMatrix(matrix, maxSize);
        if (validationResult.IsValid is false) 
            throw new ArgumentException(validationResult.ResultDetails); 

        _matrixWidth = matrix.FirstOrDefault()!.Length; // We know the matrix is not null here since we already did the ValidateMatrix that do this check
        _matrixData = GetHorizontalAndVerticalStrings(matrix);
        _topWordsCount = topWordsCount;
    }

    public async Task<IEnumerable<WordOccurrences>> FindAsync(IEnumerable<string> wordStream)
    {
        //We are using concurrent dictonary since it was designed for multi-threaded processing.
        var wordMatches = new ConcurrentDictionary<string, int>();

        await Task.Run(() =>
        {
            Parallel.ForEach(wordStream.Distinct(), word =>
            {
                word = word.Trim().ToLower();

                // Count the total occurrences of the word across all matrix strings
                var count = _matrixData.Sum(matrixString => matrixString.CountOccurrences(word));

                if (count > 0)
                {
                    if (wordMatches.TryAdd(word, count) is false)
                    {
                        wordMatches[word] += count;
                    }
                }
            });
        });

        // Sort top results
        var orderedwordMatches = wordMatches
            .OrderByDescending(keyValuePair => keyValuePair.Value)
            .ThenBy(keyValuePair => keyValuePair.Key) //We order it alphabetically too in case we get more than one word with the same time of appearance
            .Take(_topWordsCount);
            
        return orderedwordMatches.ToWordOccurrences();
    }

    /// <summary>
    /// This method transforms the matrix columns to strings and adds them to the wors into a List<string>
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    private List<string> GetHorizontalAndVerticalStrings(IEnumerable<string> matrix)
    {
        var wordSet = new List<string>();

        // Horizontal strings
        foreach (var row in matrix)
        {
            wordSet.Add(row);
        }

        // Getting the vertical text for the future lookups
        for (int col = 0; col < _matrixWidth; col++)
        {
            var verticalWord = string.Concat(matrix.Select(row => row[col]));
            wordSet.Add(verticalWord);
        }

        return wordSet;
    }

    /// <summary>
    /// Checks the validity of the Matrix ands return the error type and message details
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="maxSize"></param>
    /// <returns></returns>
    public static (bool IsValid, MatrixValidationResult MatrixValidationResult, string ResultDetails) ValidateMatrix(IEnumerable<string> matrix, int maxSize = 64)
    {
        var baseMessage = "Matrix is invalid. Reason:";

        if (matrix is null || matrix.Any() is false)
            return (false, MatrixValidationResult.NullOrEmpty, $"{baseMessage} {MatrixValidationResult.NullOrEmpty}.");

        if (matrix.Count() > maxSize)
            return (false, MatrixValidationResult.ExceedsMaxSize, $"{baseMessage} {MatrixValidationResult.ExceedsMaxSize}. Max size is: {maxSize}x{maxSize}.");

        // Get row lengths for next checks
        var rowLengths = matrix.Select(row => row.Length).ToArray();

        if (rowLengths.Distinct().Count() > 1)
            return (false, MatrixValidationResult.InconsistentRowLengths, $"{baseMessage} {MatrixValidationResult.InconsistentRowLengths}. All rows must have the same length.");

        if (rowLengths.Any(length => length > maxSize))
            return (false, MatrixValidationResult.RowExceedsMaxLength, $"{baseMessage} {MatrixValidationResult.RowExceedsMaxLength}. Rows must not exceed {maxSize} characters.");

        return (true, MatrixValidationResult.Valid, string.Empty);
    }

}
