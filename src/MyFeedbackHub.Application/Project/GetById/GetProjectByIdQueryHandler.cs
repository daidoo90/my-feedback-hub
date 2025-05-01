using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.GetById;

public sealed record GetProjectByIdQueryRequest(Guid ProjectId);

public sealed class GetProjectByIdQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetProjectByIdQueryRequest, ProjectDomain?>
{
    public async Task<ServiceDataResult<ProjectDomain?>> HandleAsync(GetProjectByIdQueryRequest request, CancellationToken cancellationToken = default)
    {
        var project = await feedbackHubDbContext
            .Projects
            .SingleOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

        return ServiceDataResult<ProjectDomain?>.WithData(project);
    }
}