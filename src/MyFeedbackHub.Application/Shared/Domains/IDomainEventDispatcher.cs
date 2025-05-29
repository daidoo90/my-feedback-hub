using MyFeedbackHub.Domain.Shared.Domains;

namespace MyFeedbackHub.Application.Shared.Domains;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<BaseDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
