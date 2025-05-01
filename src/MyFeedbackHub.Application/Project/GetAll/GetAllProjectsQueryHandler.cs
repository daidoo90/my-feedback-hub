using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.GetAll;

public sealed record GetAllProjectsQueryRequest(
    int? PageNumber,
    int? PageSize);

public sealed record GetAllProjectsResponse(
    int TotalCount,
    IEnumerable<ProjectDomain> Projects);

public sealed class GetAllProjectsQueryHandler(
    IFeedbackHubDbContext feedbackHubDbContext,
    IUserContext userContext) : IQueryHandler<GetAllProjectsQueryRequest, GetAllProjectsResponse>
{
    public async Task<ServiceDataResult<GetAllProjectsResponse>> HandleAsync(GetAllProjectsQueryRequest request, CancellationToken cancellationToken = default)
    {
        var pageNumber = request!.PageNumber.HasValue ? request.PageNumber.Value : 1;
        var pageSize = request!.PageSize.HasValue ? request.PageSize.Value : 10;

        var allProjects = feedbackHubDbContext
            .Projects
            .Where(p => p.OrganizationId == userContext.OrganizationId);

        var totalCount = await allProjects.CountAsync(cancellationToken);
        var projects = await allProjects
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return ServiceDataResult<GetAllProjectsResponse>.WithData(new GetAllProjectsResponse(totalCount, projects));
    }
}