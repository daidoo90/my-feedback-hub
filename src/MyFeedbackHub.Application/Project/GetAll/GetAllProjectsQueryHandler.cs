using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project;

public sealed record GetAllProjectsQuery(
    int? PageNumber,
    int? PageSize,
    Guid OrganizationId,
    IEnumerable<Guid> ProjectIds);

public sealed record GetAllProjectsResponse(
    int TotalCount,
    IEnumerable<ProjectDomain> Projects);

public sealed class GetAllProjectsQueryHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext userContext) : IQueryHandler<GetAllProjectsQuery, GetAllProjectsResponse>
{
    public async Task<ServiceDataResult<GetAllProjectsResponse>> HandleAsync(GetAllProjectsQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query!.PageNumber.HasValue ? query.PageNumber.Value : 1;
        var pageSize = query!.PageSize.HasValue ? query.PageSize.Value : 10;

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var allProjects = dbContext
            .Projects
            .Where(p => p.OrganizationId == userContext.OrganizationId
                        && (!query.ProjectIds.Any() || query.ProjectIds.Contains(p.ProjectId)));

        var totalCount = await allProjects.CountAsync(cancellationToken);
        var projects = await allProjects
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ServiceDataResult<GetAllProjectsResponse>.WithData(new GetAllProjectsResponse(totalCount, projects));
    }
}