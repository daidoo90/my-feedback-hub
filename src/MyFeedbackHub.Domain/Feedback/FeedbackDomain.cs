using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Domain.Feedback;

public sealed class FeedbackDomain
{
    public Guid FeedbackId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public Guid? AssigneeId { get; private set; }

    public UserDomain? Assignee { get; private set; }

    public FeedbackStatusType Status { get; private set; }

    public FeedbackType Type { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public Guid? CreatedBy { get; private set; }

    public DateTimeOffset UpdatedOn { get; private set; }

    public Guid UpdatedBy { get; private set; }

    protected FeedbackDomain()
    { }

    public static FeedbackDomain Create(
        string title,
        string description,
        FeedbackType type,
        DateTimeOffset createdOn)
    {
        return new FeedbackDomain
        {
            FeedbackId = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = FeedbackStatusType.New,
            Type = type,
            CreatedOn = createdOn
        };
    }

    public void SetAsInReview(Guid byUserId) => SetStatus(FeedbackStatusType.InReview, byUserId);

    public void SetAsPlanned(Guid byUserId) => SetStatus(FeedbackStatusType.Planned, byUserId);

    public void SetAsInProgress(Guid byUserId) => SetStatus(FeedbackStatusType.InProgress, byUserId);

    public void SetAsReleased(Guid byUserId) => SetStatus(FeedbackStatusType.Released, byUserId);

    public void SetAsRejected(Guid byUserId) => SetStatus(FeedbackStatusType.Released, byUserId);

    private void SetStatus(FeedbackStatusType newStatus, Guid byUserId)
    {
        Status = newStatus;
        UpdatedOn = DateTimeOffset.UtcNow;
        UpdatedBy = byUserId;
    }
}
