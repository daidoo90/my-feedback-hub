using System.Runtime.Serialization;

namespace MyFeedbackHub.Domain.Shared.Exceptions;

public sealed class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string? message) : base(message)
    {
    }

    public DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
