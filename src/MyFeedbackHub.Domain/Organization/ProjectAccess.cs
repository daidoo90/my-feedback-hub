namespace MyFeedbackHub.Domain.Organization;

public sealed class ProjectAccess
{
    public Guid UserId { get; set; }

    public UserDomain User { get; set; } = null!;

    public Guid ProjectId { get; set; }

    public ProjectDomain Project { get; set; } = null!;
}
