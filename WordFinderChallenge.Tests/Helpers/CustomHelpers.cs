using Bogus;

namespace WordFinderChallenge.Tests.Helpers;

public static class CustomHelpers
{
    public static List<string> GenerateWords(int count)
    {
        return new Faker<string>()
            .CustomInstantiator(f => f.Lorem.Word())
            .Generate(count);
    }
}
