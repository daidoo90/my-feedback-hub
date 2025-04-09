namespace MyFeedbackHub.Domain;

public sealed class FeedbackCommentDomain
{
    public Guid CommentId { get; set; }

    public Guid? ParentCommentId { get; set; }

    public required string Text { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public UserDomain? CreatedBy { get; set; }

    public FeedbackDomain? Feedback { get; set; }
}
