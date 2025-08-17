namespace MyFeedbackHub.Infrastructure.DAL.Entities;

public class OutboxMessage
{
    public Guid MessageId { get; set; }

    public required string Type { get; set; }

    public OutboxStatus Status { get; set; }

    public DateTimeOffset OccurredOn { get; set; }

    public DateTimeOffset? ProcessedOn { get; set; }

    public int RetryCount { get; set; }

    public string Error { get; set; } = string.Empty;

    public required string Payload { get; set; }
}
