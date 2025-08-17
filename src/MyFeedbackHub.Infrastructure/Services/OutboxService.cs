using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Infrastructure.DAL.Context;
using MyFeedbackHub.Infrastructure.DAL.Entities;
using System.Text.Json;

namespace MyFeedbackHub.Infrastructure.Services;

public class OutboxService : IOutboxService
{
    private readonly FeedbackHubDbContext _dbContext;

    public OutboxService(FeedbackHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
    {
        var outboxMessage = new OutboxMessage
        {
            MessageId = Guid.NewGuid(),
            Type = typeof(TMessage).Name,
            Status = OutboxStatus.New,
            Payload = JsonSerializer.Serialize(message),
            OccurredOn = DateTimeOffset.UtcNow,
        };

        await _dbContext.AddAsync(outboxMessage, cancellationToken);

        return outboxMessage.MessageId;
    }
}
