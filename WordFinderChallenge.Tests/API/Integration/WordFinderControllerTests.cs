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
    public HttpClient Client { get; private set; }
    public JsonSerializerOptions JsonOptions { get; private set; }
    //private readonly HttpClient _client;
    //private readonly JsonSerializerOptions _jsonOptions;

    private const string Success = nameof(Success);
    public const string BaseUrl = "api/WordFinder/";

    public WordFinderControllerTests(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        JsonOptions = factory.JsonOptions;
    }

    [Theory]
    [InlineData(nameof(CharacterMatricesRepository.Matrix16x16))]
    [InlineData(nameof(CharacterMatricesRepository.Matrix64x64))]
    public async Task SearchOnValidMatrices_ShouldReturnOk(string matrixType)
    {
        // Arrange
        var wordStreamRequest = CustomHelpers.GenerateWords(1);
        wordStreamRequest.Add("partner"); //We add this word because we know it's on the Matrix

        var content = new StringContent(
            JsonSerializer.Serialize(wordStreamRequest),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var result = await Client.PostAsync($"{BaseUrl}{matrixType}", content);

        // Assert
        var responseJson = await result.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<ApiResponse<List<WordOccurrences>>>(responseJson, JsonOptions);
        response?.StatusCode.Should().Be(StatusCodes.Status200OK);
        response?.Data.Should().NotBeNull();
        response?.Data?.Count.Should().BeGreaterThan(0);
        response?.Message.Should().Be(Success);
    }

    [Theory]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrixEmpty))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrixDifferentLengths))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrix65x2))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrix2x65))]
    [InlineData(nameof(CharacterMatricesRepository.InvalidMatrix65x65))]
    public async Task SearchOnInvalidMatrices_ShouldReturnOk(string matrixType)
    {
        // Arrange
        var wordStreamRequest = CustomHelpers.GenerateWords(40);
        wordStreamRequest.Add("partner"); //We add this word because we know it's on the Matrix

        var content = new StringContent(
            JsonSerializer.Serialize(wordStreamRequest),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var result = await Client.PostAsync($"{BaseUrl}{matrixType}", content);

        // Assert
        var responseJson = await result.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<ApiResponse<List<WordOccurrences>>>(responseJson, JsonOptions);
        response?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        response?.Data.Should().BeNull();
        response?.Message.Should().Contain("Matrix is invalid");
    }
}
