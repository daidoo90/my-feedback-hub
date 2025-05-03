using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.GetById;

public sealed record GetProjectByIdQuery(Guid ProjectId);

public sealed class GetProjectByIdQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetProjectByIdQuery, ProjectDomain?>
{
    public async Task<ServiceDataResult<ProjectDomain?>> HandleAsync(GetProjectByIdQuery query, CancellationToken cancellationToken = default)
    {
        var project = await feedbackHubDbContext
            .Projects
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ProjectId == query.ProjectId, cancellationToken);

        return ServiceDataResult<ProjectDomain?>.WithData(project);
    }
}