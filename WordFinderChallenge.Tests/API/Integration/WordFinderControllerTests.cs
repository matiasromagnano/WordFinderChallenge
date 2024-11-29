using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using WordFinderChallenge.API.Models;
using WordFinderChallenge.Core.Models;
using WordFinderChallenge.Tests.Configuration;
using WordFinderChallenge.Tests.Helpers;
using WordFinderChallenge.Utilities;

namespace WordFinderChallenge.Tests.API.Integration;

/// <summary>
/// API Integration Testing
/// </summary>
public class WordFinderControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string Success = nameof(Success);
    public const string BaseUrl = "api/WordFinder/";
    public const string PartnerWord = "partner";
    public const string DigitalWord = "digital";

    public WordFinderControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = factory.JsonOptions;
    }

    [Theory]
    [InlineData(nameof(CharacterMatricesRepository.Matrix16x16))]
    [InlineData(nameof(CharacterMatricesRepository.Matrix64x64))]
    public async Task SearchOnValidMatrices_ShouldReturnOk(string matrixType)
    {
        // Arrange
        var wordStreamRequest = PrepareWordStream(40);

        var content = new StringContent(
            JsonSerializer.Serialize(wordStreamRequest),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var result = await _client.PostAsync($"{BaseUrl}{matrixType}", content);

        // Assert
        var responseJson = await result.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<ApiResponse<List<WordOccurrences>>>(responseJson, _jsonOptions);
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Data.Should().NotBeNull();
        response?.Data?.Count.Should().BeGreaterThan(0);
        response?.Data?.Any(w => w.Word == PartnerWord).Should().BeTrue();
        response?.Data?.Any(w => w.Word == DigitalWord).Should().BeTrue();
        response?.Message.Should().Be(Success);
    }

    [Theory]
    [InlineData(nameof(CharacterMatricesRepository.Matrix16x16))]
    [InlineData(nameof(CharacterMatricesRepository.Matrix64x64))]
    public async Task SearchOnValidMatrices_ShouldReturnBadRequest(string matrixType)
    {
        // Arrange
        var invalidWordStreamRequest = "partner";

        var content = new StringContent(
            invalidWordStreamRequest,
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var result = await _client.PostAsync($"{BaseUrl}{matrixType}", content);

        // Assert
        var responseJson = await result.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<ApiResponse<List<WordOccurrences>>>(responseJson, _jsonOptions);
        response?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        response?.Data.Should().BeNull();
        response?.Details?.Should().NotBeNull();
        response?.Message.Should().Be("One or more validation errors occurred.");
    }

    [Theory]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrixEmpty))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrixDifferentLengths))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrix65x2))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrix2x65))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrix65x65))]
    public async Task SearchOnInvalidMatrices_ShouldReturnInternalServerError(string matrixType)
    {
        // Arrange
        // Arrange
        var wordStreamRequest = PrepareWordStream(40);

        var content = new StringContent(
            JsonSerializer.Serialize(wordStreamRequest),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var result = await _client.PostAsync($"{BaseUrl}{matrixType}", content);

        // Assert
        var responseJson = await result.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<ApiResponse<List<WordOccurrences>>>(responseJson, _jsonOptions);
        response?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        response?.Data.Should().BeNull();
        response?.Message.Should().Contain("Matrix is invalid");
    }

    private static List<string> PrepareWordStream(int wordCount)
    {
        var wordStreamRequest = CustomHelpers.GenerateWords(wordCount);
        wordStreamRequest.Add(PartnerWord); //We add this word because we know it's (horizontally) on the Matrix
        wordStreamRequest.Add(DigitalWord); //We add this word because we know it's (vertically) on the Matrix
        return wordStreamRequest;
    }
}
