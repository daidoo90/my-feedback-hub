using MyFeedbackHub.Application.Organization.Services;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class AuthorizationService(
    IUserContext currentUser,
    IOrganizationService organizationService,
    IUserService userService)
    : IAuthorizationService
{
    public async Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        if (currentUser.Role == UserRoleType.OrganizationAdmin)
        {
            return (await organizationService.GetProjectsAsync(currentUser.OrganizationId)).Contains(projectId);
        }
        else
        {
            return (await userService.GetProjectIdsAsync(currentUser.UserId)).Contains(projectId);
        }
    }
}
