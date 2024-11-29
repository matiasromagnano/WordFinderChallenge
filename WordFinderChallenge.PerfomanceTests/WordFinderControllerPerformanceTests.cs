using BenchmarkDotNet.Attributes;
using Bogus;
using System.Text;
using System.Text.Json;

namespace WordFinderChallenge.PerfomanceTests;

[MemoryDiagnoser]
public class WordFinderControllerPerformanceTests
{
    private HttpClient? _client;
    private JsonSerializerOptions? _jsonOptions;

    [Params(10, 100, 1000, 10000, 100000, 1000000)] // Word stream sizes for benchmarking
    public int WordStreamSize;

    private IEnumerable<string>? _wordStream;

    [GlobalSetup]
    public void Setup()
    {
        _client = new HttpClient { BaseAddress = new Uri("https://localhost:7277/") };
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _wordStream = GenerateWords(WordStreamSize);
    }

    [Benchmark]
    public async Task SearchOnValidMatrices()
    {
        // Arrange
        var content = new StringContent(
            JsonSerializer.Serialize(_wordStream, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        // We'll be testing against the larger matrix 
        var result = await _client.PostAsync("/api/wordfinder/matrix64x64", content);

        // Ensure success for the benchmark
        result.EnsureSuccessStatusCode();
    }

    private static List<string> GenerateWords(int count)
    {
        var words = new Faker<string>()
            .CustomInstantiator(f => f.Lorem.Word())
            .Generate(count);
        words.Add("partner");

        return words;
    }
}
