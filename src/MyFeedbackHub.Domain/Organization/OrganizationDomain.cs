namespace MyFeedbackHub.Domain.Organization;

public sealed class OrganizationDomain
{
    public Guid OrganizationId { get; private set; }

    public string Name { get; private set; }

    public Address Address { get; private set; } = new Address();

    public string? TaxID { get; private set; }

    public ICollection<ProjectDomain> Projects { get; private set; } = [];

    public ICollection<UserDomain> Users { get; private set; } = [];

    public DateTimeOffset CreatedOn { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTimeOffset? UpdatedOn { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedOn { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    protected OrganizationDomain()
    { }

    public static OrganizationDomain Create(
        string name,
        DateTimeOffset createdOn)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return new OrganizationDomain
        {
            OrganizationId = Guid.NewGuid(),
            Name = name,
            CreatedOn = createdOn
        };
    }

    public void Update(
        string name,
        string taxId,
        DateTimeOffset updatedOn,
        Guid byUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
        TaxID = taxId;
        UpdatedOn = updatedOn;
        UpdatedByUserId = byUserId;
    }

    public void SetCreatedBy(Guid userId)
    {
        CreatedByUserId = userId;
    }
}
