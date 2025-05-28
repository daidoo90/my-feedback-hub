using MyFeedbackHub.Domain.Shared.Exceptions;

namespace MyFeedbackHub.Domain.Feedback;

public sealed class CommentDomain
{
    public Guid CommentId { get; private set; }

    public string Text { get; private set; } = string.Empty;

    public Guid FeedbackId { get; private set; }

    public FeedbackDomain Feedback { get; private set; } = null!;

    public Guid? ParentCommentId { get; private set; }

    //public CommentDomain? ParentComment { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public Guid CreatedBy { get; private set; }

    public DateTimeOffset? UpdatedOn { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedOn { get; private set; }

    public Guid? DeletedBy { get; private set; }

    public static CommentDomain Create(
        string text,
        DateTimeOffset createdOn,
        Guid createdBy,
        Guid feedbackId,
        Guid? parentCommentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);

        return new CommentDomain
        {
            CommentId = Guid.NewGuid(),
            Text = text,
            FeedbackId = feedbackId,
            ParentCommentId = parentCommentId,
            CreatedOn = createdOn,
            CreatedBy = createdBy
        };
    }

    public void Update(
        string text,
        DateTimeOffset updatedOn,
        Guid updatedBy)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        if (updatedBy != CreatedBy)
        {
            throw new DomainException($"Comment can't be deleted by user: {updatedBy}");
        }

        if (IsDeleted)
        {
            throw new DomainException("Can't update deleted comment.");
        }

        Text = text;
        UpdatedOn = updatedOn;
        UpdatedBy = updatedBy;
    }

    public void Delete(
        DateTimeOffset deletedOn,
        Guid deletedBy)
    {
        if (deletedBy != CreatedBy)
        {
            throw new DomainException($"Comment can't be deleted by user: {deletedBy}");
        }

        if (IsDeleted)
        {
            throw new DomainException($"Comment is already deleted.");
        }

        IsDeleted = true;
        DeletedOn = deletedOn;
        DeletedBy = deletedBy;
    }
}
