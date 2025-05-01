using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IUserContext
{
    public Guid UserId { get; }

    public Guid OrganizationId { get; }

    public UserRoleType Role { get; }
}
