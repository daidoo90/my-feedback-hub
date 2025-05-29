using Microsoft.Extensions.DependencyInjection;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Shared.Domains;

namespace MyFeedbackHub.Application.Shared.Domains;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IEnumerable<BaseDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                if (handleMethod != null)
                {
                    var task = (Task)handleMethod.Invoke(handler, [domainEvent, cancellationToken])!;
                    await task;
                }
            }
        }
    }
}
