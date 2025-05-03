using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetAll;

public sealed record GetAllUsersQuery(
    int? PageNumber,
    int? PageSize,
    Guid OrganizationId,
    IEnumerable<Guid>? projectIds);

public sealed record GetAllUsersResponse(
    int TotalCount,
    IEnumerable<UserDomain> Users);

public sealed class GetAllUsersQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetAllUsersQuery, GetAllUsersResponse>
{
    public async Task<ServiceDataResult<GetAllUsersResponse>> HandleAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query!.PageNumber.HasValue ? query.PageNumber.Value : 1;
        var pageSize = query!.PageSize.HasValue ? query.PageSize.Value : 10;

        var allUsers = feedbackHubDbContext
            .Users
            .Include(u => u.ProjectAccess)
            .ThenInclude(pa => pa.Project)
            .Where(p => p.OrganizationId == query.OrganizationId);
        //&& (request.projectIds == null 
        //    || request.projectIds.Contains(p.ProjectAccess.));
        //TODO: Improve authorization logic

        var totalCount = await allUsers.CountAsync(cancellationToken);
        var users = await allUsers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ServiceDataResult<GetAllUsersResponse>.WithData(new GetAllUsersResponse(totalCount, users));
    }
}