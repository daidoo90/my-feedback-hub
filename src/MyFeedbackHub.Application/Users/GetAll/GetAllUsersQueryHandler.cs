using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetAll;

public sealed record GetAllUsersQuery(
    int? PageNumber,
    int? PageSize,
    IEnumerable<Guid>? projectIds);

public sealed record GetAllUsersResponse(
    int TotalCount,
    IEnumerable<UserDomain> Users);

public sealed class GetAllUsersQueryHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : IQueryHandler<GetAllUsersQuery, GetAllUsersResponse>
{
    public async Task<ServiceDataResult<GetAllUsersResponse>> HandleAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query!.PageNumber.HasValue ? query.PageNumber.Value : 1;
        var pageSize = query!.PageSize.HasValue ? query.PageSize.Value : 10;

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var allUsers = dbContext
            .Users
            .Include(u => u.ProjectAccess)
            .ThenInclude(pa => pa.Project)
            .Where(p => p.OrganizationId == currentUser.OrganizationId &&
                (query.projectIds == null ||
                 !query.projectIds.Any() ||
                 p.ProjectAccess.Any(pa => query.projectIds.Contains(pa.ProjectId))));

        var totalCount = await allUsers.CountAsync(cancellationToken);
        var users = await allUsers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ServiceDataResult<GetAllUsersResponse>.WithData(new GetAllUsersResponse(totalCount, users));
    }
}