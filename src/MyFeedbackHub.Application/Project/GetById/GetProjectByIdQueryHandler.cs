using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project;

public sealed record GetProjectByIdQuery(Guid ProjectId);

public sealed class GetProjectByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IAuthorizationService authorizationService)
    : IQueryHandler<GetProjectByIdQuery, ProjectDomain>
{
    public async Task<ServiceDataResult<ProjectDomain>> HandleAsync(GetProjectByIdQuery query, CancellationToken cancellationToken = default)
    {
        if (!await authorizationService.CanAccessProjectAsync(query.ProjectId, cancellationToken))
        {
            return ServiceDataResult<ProjectDomain>.WithError(ErrorCodes.Feedback.NotFound);
        }

        var project = await unitOfWork
            .DbContext
            .Projects
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ProjectId == query.ProjectId
                                        && !p.IsDeleted, cancellationToken);

        if (project == null)
        {
            return ServiceDataResult<ProjectDomain>.WithError(ErrorCodes.Project.NotFound);
        }

        return ServiceDataResult<ProjectDomain>.WithData(project);
    }
}