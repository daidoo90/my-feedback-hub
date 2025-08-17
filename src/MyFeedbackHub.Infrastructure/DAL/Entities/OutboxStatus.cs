namespace MyFeedbackHub.Infrastructure.DAL.Entities;

public enum OutboxStatus
{
    New = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4
}
