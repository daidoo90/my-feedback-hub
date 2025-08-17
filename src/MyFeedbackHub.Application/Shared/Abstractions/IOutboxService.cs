namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IOutboxService
{
    Task<Guid> AddAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}
