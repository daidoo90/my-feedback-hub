using MyFeedbackHub.Domain.Types;
using System.Security.Claims;

namespace MyFeedbackHub.Api.Shared.Utils;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new ApplicationException("User id is unavailable");
    }

    public static Guid GetOrganizationid(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue("OrganizationId");

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new ApplicationException("User id is unavailable");
    }

    public static UserRoleType GetRoleId(this ClaimsPrincipal? principal)
    {
        string? roleClaim = principal?.FindFirstValue(ClaimTypes.Role);

        if (!Enum.TryParse<UserRoleType>(roleClaim, out var role))
        {
            throw new ApplicationException("User id is unavailable");
        }

        return role;
    }
}
