using MyFeedbackHub.Domain.Shared.Domains;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IDomainEventHandler<TDomain>
    where TDomain : BaseDomainEvent
{
    Task HandleAsync(TDomain domainEvent, CancellationToken cancellationToken = default);
}
