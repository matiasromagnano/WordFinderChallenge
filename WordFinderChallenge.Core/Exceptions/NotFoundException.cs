using System.Net;

namespace WordFinderChallenge.Core.Exceptions;

public class NotFoundException : CustomException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public NotFoundException(string message)
        : base(message)
    {
    }
}
