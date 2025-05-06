using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Shared.Exceptions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Domain.Organization;

public sealed class UserDomain
{
    public Guid UserId { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string Username { get; private set; }

    public string? Password { get; private set; }

    public string? Salt { get; private set; }

    public UserRoleType Role { get; private set; }

    public UserStatusType Status { get; private set; }

    public Guid OrganizationId { get; private set; }

    public OrganizationDomain Organization { get; private set; } = null!;

    public ICollection<ProjectAccess> ProjectAccess { get; private set; } = [];

    public ICollection<FeedbackDomain> Feedbacks { get; private set; } = [];

    public DateTimeOffset CreatedOn { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public DateTimeOffset? UpdatedOn { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public DateTimeOffset? DeletedOn { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    protected UserDomain()
    { }

    public static UserDomain Create(
        string username,
        Guid organizationId,
        UserStatusType status,
        UserRoleType role,
        DateTimeOffset createdOn,
        Guid byUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(username);

        return new UserDomain
        {
            UserId = Guid.NewGuid(),
            Username = username,
            OrganizationId = organizationId,
            Status = status,
            Role = role,
            CreatedOn = createdOn,
            CreatedByUserId = byUserId
        };
    }

    public static UserDomain Create(
        string username,
        string password,
        string salt,
        OrganizationDomain organization,
        UserStatusType status,
        UserRoleType role,
        DateTimeOffset createdOn,
        Guid? byUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(username);
        ArgumentException.ThrowIfNullOrEmpty(password);
        ArgumentException.ThrowIfNullOrEmpty(salt);
        ArgumentNullException.ThrowIfNull(organization);

        return new UserDomain
        {
            UserId = Guid.NewGuid(),
            Username = username,
            Password = password,
            Salt = salt,
            Organization = organization,
            Status = status,
            Role = role,
            CreatedOn = createdOn,
            CreatedByUserId = byUserId
        };
    }

    public void GrantAccess(Guid projectId)
    {
        if (ProjectAccess.Any(pa => pa.ProjectId == projectId))
        {
            return;
        }

        ProjectAccess.Add(new ProjectAccess
        {
            ProjectId = projectId
        });
    }

    public void Update(
        string firstName,
        string lastName,
        string phoneNumber,
        DateTimeOffset updatedOn,
        Guid byUserId
        )
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName);
        ArgumentException.ThrowIfNullOrEmpty(lastName);
        ArgumentException.ThrowIfNullOrEmpty(phoneNumber);
        if (Status == UserStatusType.Inactive)
        {
            throw new DomainException("Can't update the user, because it's already deactivated.");
        }

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        UpdatedOn = updatedOn;
        UpdatedByUserId = byUserId;
    }

    public void Delete(Guid byUserId)
    {
        if (Status == UserStatusType.Inactive)
        {
            throw new DomainException("Can't deactivate the user, because it's already deactivated.");
        }

        Status = UserStatusType.Inactive;
        DeletedOn = DateTimeOffset.UtcNow;
        DeletedByUserId = byUserId;
    }

    public void SetFirstPassword(
        string password,
        string salt)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(password);
        ArgumentNullException.ThrowIfNullOrEmpty(salt);
        if (Status != UserStatusType.PendingInvitation)
        {
            throw new ArgumentException();
        }

        Password = password;
        Salt = salt;
        Status = UserStatusType.Active;
        UpdatedOn = DateTimeOffset.UtcNow;
        UpdatedByUserId = UserId;
    }
}
