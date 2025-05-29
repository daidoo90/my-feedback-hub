using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Shared.Domains;

namespace MyFeedbackHub.Application.Users.SendWelcomeEmail;

public sealed class UserCreatedDomainEvent : BaseDomainEvent
{
    public UserDomain User { get; }

    public UserCreatedDomainEvent(UserDomain user) => User = user;
}

public sealed class SendWelcomeEmailEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    public Task HandleAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
