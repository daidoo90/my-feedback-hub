using MyFeedbackHub.Domain.Feedback;

namespace MyFeedbackHub.Domain.Organization;

public sealed class ProjectDomain
{
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; }

    public string? Url { get; private set; }

    public string? Description { get; private set; }

    public Guid OrganizationId { get; private set; }

    public OrganizationDomain Organization { get; private set; } = null!;

    public ICollection<ProjectAccess> ProjectAccess { get; private set; } = [];

    public ICollection<FeedbackDomain> Feedbacks { get; private set; } = [];

    public DateTimeOffset CreatedOn { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public DateTimeOffset? UpdatedOn { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedOn { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    protected ProjectDomain()
    { }

    public static ProjectDomain Create(
        string name,
        Guid organizationId,
        string? url,
        string? description,
        DateTimeOffset createdOn,
        Guid? byUserId)
    {
        return new ProjectDomain
        {
            Name = name,
            OrganizationId = organizationId,
            Url = url,
            Description = description,
            CreatedOn = createdOn,
            CreatedByUserId = byUserId,
        };
    }

    public static ProjectDomain Create(
        string name,
        OrganizationDomain organization,
        DateTimeOffset createdOn,
        Guid? byUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(organization);

        return new ProjectDomain
        {
            ProjectId = Guid.NewGuid(),
            Name = name,
            Organization = organization,
            CreatedOn = createdOn,
            CreatedByUserId = byUserId,
        };
    }

    public void Update(
        string name,
        string url,
        string description,
        DateTimeOffset updatedOn,
        Guid byUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
        Url = url;
        Description = description;
        UpdatedOn = updatedOn;
        UpdatedByUserId = byUserId;
    }

    public void Delete(Guid byUserId)
    {
        IsDeleted = true;
        DeletedOn = DateTimeOffset.UtcNow;
        DeletedByUserId = byUserId;
    }
}
