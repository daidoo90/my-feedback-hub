using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Domain.Organization;

public sealed class UserDomain
{
    public Guid UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Salt { get; set; }

    public required UserRoleType Role { get; set; }

    public required UserStatusType Status { get; set; }

    public Guid OrganizationId { get; set; }

    public OrganizationDomain Organization { get; set; } = null!;

    public ICollection<ProjectAccess> ProjectAccess { get; set; } = [];

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedOnByUserId { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public Guid? DeletedByUserId { get; set; }

}
