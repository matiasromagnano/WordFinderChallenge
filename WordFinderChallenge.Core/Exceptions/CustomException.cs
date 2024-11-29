using System.Diagnostics.CodeAnalysis;

namespace WordFinderChallenge.Core.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class CustomException : Exception
{

    public virtual int StatusCode { get; set; }

    public CustomException(string message)
        : base(message)
    {
    }
}
