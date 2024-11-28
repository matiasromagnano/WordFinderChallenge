using System.Net;

namespace WordFinderChallenge.Core.Exceptions;

public class BadRequestException : CustomException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public BadRequestException(string message)
        : base(message)
    {
    }
}
