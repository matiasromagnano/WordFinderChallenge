using FluentAssertions;
using WordFinderChallenge.Core.Enums;
using WordFinderChallenge.Core.Services;

namespace WordFinderChallenge.Tests.Core.Services;

public class WordFinderTests
{
    [Theory]
    [InlineData(new[] { "fgh", "ddd", "aoi" }, true, MatrixValidationResult.Valid, "")]
    [InlineData(new[] { "fgh", "ddd", "aoi", "m" }, false, MatrixValidationResult.InconsistentRowLengths, "All rows must have the same length.")]
    [InlineData(new string[] { }, false, MatrixValidationResult.NullOrEmpty, "Matrix is invalid. Reason: NullOrEmpty.")]
    [InlineData(null, false, MatrixValidationResult.NullOrEmpty, "Matrix is invalid. Reason: NullOrEmpty.")]
    [InlineData(new string[] { "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" }, false, MatrixValidationResult.RowExceedsMaxLength, "Rows must not exceed 64 characters.")]
    public void ValidateMatrix_ShouldHandleDifferentMatrices(IEnumerable<string>? matrix, bool isValid, MatrixValidationResult expectedResult, string expectedDetails)
    {
        // Act
        var result = WordFinder.ValidateMatrix(matrix);

        // Assert
        result.IsValid.Should().Be(isValid);
        result.MatrixValidationResult.Should().Be(expectedResult);
        if (!string.IsNullOrEmpty(expectedDetails))
        {
            result.ResultDetails.Should().Contain(expectedDetails);
        }
    }

    [Theory]
    [InlineData(new[] { "partnerasd", "hhgrfllbgf", "partnerhhh" }, new string[] { "partner", "asd", "hhh", "notfound" }, 3)]
    [InlineData(new[] { "boy", "def", "eat" }, new string[] { "boy", "eat", "fff" }, 2)]
    [InlineData(new[] { "aaaa", "bbbb", "cccc" }, new string[] { "aaaa", "bbbb", "cccc", "ddd" }, 3)]
    public async Task FindAsync_ShouldReturnExpectedResults(IEnumerable<string> matrix, IEnumerable<string> wordStream, int expectedCount)
    {
        // Arrange
        var wordFinder = new WordFinder(matrix);

        // Act
        var result = await wordFinder.FindAsync(wordStream);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedCount);
    }
}
