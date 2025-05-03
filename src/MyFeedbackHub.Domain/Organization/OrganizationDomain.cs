namespace MyFeedbackHub.Domain.Organization;

public sealed class OrganizationDomain
{
    public Guid OrganizationId { get; set; }

    public string? Name { get; set; }

    public Address Address { get; set; } = new Address();

    public string? TaxID { get; set; }

    public ICollection<ProjectDomain> Projects { get; set; } = [];

    public ICollection<UserDomain> Users { get; set; } = [];

    public DateTimeOffset CreatedOn { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedOnByUserId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public Guid? DeletedByUserId { get; set; }
}
