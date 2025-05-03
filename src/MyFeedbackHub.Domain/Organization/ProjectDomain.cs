namespace MyFeedbackHub.Domain.Organization;

public sealed class ProjectDomain
{
    public Guid ProjectId { get; set; }

    public required string Name { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

    public Guid OrganizationId { get; set; }

    public OrganizationDomain Organization { get; set; } = null!;

    public ICollection<ProjectAccess> ProjectAccess { get; set; } = [];

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedOnByUserId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public Guid? DeletedByUserId { get; set; }

}
