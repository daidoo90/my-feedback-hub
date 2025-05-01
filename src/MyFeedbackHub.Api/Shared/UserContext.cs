using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Shared;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");

    public Guid OrganizationId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetOrganizationid() ??
        throw new ApplicationException("User context is unavailable");

    public UserRoleType Role =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetRoleId() ??
        throw new ApplicationException("User context is unavailable");
}
