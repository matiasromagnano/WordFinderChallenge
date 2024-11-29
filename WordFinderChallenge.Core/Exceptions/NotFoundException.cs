using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace WordFinderChallenge.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException : CustomException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public NotFoundException(string message)
        : base(message)
    {
    }
}
