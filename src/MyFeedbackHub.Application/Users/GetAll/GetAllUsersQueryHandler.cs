using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetAll;

public sealed record GetAllUsersQueryRequest(
    int? PageNumber,
    int? PageSize);

public sealed record GetAllUsersResponse(
    int TotalCount,
    IEnumerable<UserDomain> Users);

public sealed class GetAllUsersQueryHandler(
    IFeedbackHubDbContext feedbackHubDbContext,
    IUserContext userContext) : IQueryHandler<GetAllUsersQueryRequest, GetAllUsersResponse>
{
    public async Task<ServiceDataResult<GetAllUsersResponse>> HandleAsync(GetAllUsersQueryRequest request, CancellationToken cancellationToken = default)
    {
        var pageNumber = request!.PageNumber.HasValue ? request.PageNumber.Value : 1;
        var pageSize = request!.PageSize.HasValue ? request.PageSize.Value : 10;

        var allUsers = feedbackHubDbContext
            .Users
            .Where(p => p.OrganizationId == userContext.OrganizationId);

        var totalCount = await allUsers.CountAsync(cancellationToken);
        var users = await allUsers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return ServiceDataResult<GetAllUsersResponse>.WithData(new GetAllUsersResponse(totalCount, users));
    }
}