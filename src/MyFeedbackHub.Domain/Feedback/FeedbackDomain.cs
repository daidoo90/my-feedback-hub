using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Shared.Exceptions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Domain.Feedback;

public sealed class FeedbackDomain
{
    public Guid FeedbackId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; } = null;

    public Guid ProjectId { get; private set; }

    public ProjectDomain Project { get; private set; }

    public Guid? AssigneeId { get; private set; }

    public UserDomain? Assignee { get; private set; }

    public FeedbackStatusType Status { get; private set; }

    public FeedbackType Type { get; private set; }

    public ICollection<CommentDomain> Comments { get; private set; } = [];

    public DateTimeOffset CreatedOn { get; private set; }

    public Guid? CreatedBy { get; private set; }

    public DateTimeOffset? UpdatedOn { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedOn { get; private set; }

    public Guid? DeletedBy { get; private set; }

    protected FeedbackDomain()
    { }

    public static FeedbackDomain Create(
        string title,
        string description,
        FeedbackType type,
        Guid projectId,
        DateTimeOffset createdOn,
        Guid createdBy)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        if (createdBy.Equals(Guid.Empty))
        {
            throw new ArgumentException(nameof(createdBy));
        }

        if (projectId.Equals(Guid.Empty))
        {
            throw new ArgumentException(nameof(projectId));
        }

        return new FeedbackDomain
        {
            FeedbackId = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = FeedbackStatusType.New,
            Type = type,
            ProjectId = projectId,
            CreatedOn = createdOn,
            CreatedBy = createdBy
        };
    }

    public void Update(
        string title,
        string description,
        FeedbackStatusType status,
        DateTimeOffset updatedOn,
        Guid byUser)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        if (IsDeleted)
        {
            throw new DomainException("Can't update deleted feedback.");
        }

        Title = title;
        Description = description;
        Status = status;
        UpdatedOn = updatedOn;
        UpdatedBy = byUser;
    }

    public void Delete(
        DateTimeOffset deletedOn,
        Guid byUser)
    {
        if (IsDeleted)
        {
            throw new DomainException("Can't delete feedback, because it is already deleted.");
        }

        IsDeleted = true;
        DeletedOn = deletedOn;
        DeletedBy = byUser;
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
