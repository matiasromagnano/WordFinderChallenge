using System.Net;

namespace WordFinderChallenge.Core.Exceptions;

public class OtherException : CustomException
{
    public override int StatusCode => (int)HttpStatusCode.InternalServerError;

    public OtherException(string message)
        : base(message)
    {
    }
}
