using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Domain.Feedback;

public sealed class FeedbackDomain
{
    public Guid FeedbackId { get; set; }

    public required string Title { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Vote { get; set; }

    public int Status { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public UserDomain? CreatedBy { get; set; }

    public int BoardId { get; set; }
}
