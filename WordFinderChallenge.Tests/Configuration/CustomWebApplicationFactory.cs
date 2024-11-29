using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace WordFinderChallenge.Tests.Configuration;

/// <summary>
/// This class allow us to get the json options that the API is actually using
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public JsonSerializerOptions JsonOptions { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<JsonOptions>>();
            JsonOptions = options.Value.JsonSerializerOptions;
        });
    }
}
