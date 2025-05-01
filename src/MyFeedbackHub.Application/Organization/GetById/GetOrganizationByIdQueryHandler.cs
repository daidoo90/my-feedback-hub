using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.GetById;

public sealed record GetOrganizationByIdQueryRequest(
    Guid OrganizationId);

public sealed class GetOrganizationByIdQueryHandler(
    IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetOrganizationByIdQueryRequest, OrganizationDomain?>
{
    public async Task<ServiceDataResult<OrganizationDomain?>> HandleAsync(GetOrganizationByIdQueryRequest request, CancellationToken cancellationToken = default)
    {
        var project = await feedbackHubDbContext
            .Organizations
            .Include(o => o.Projects)
            .SingleOrDefaultAsync(p => p.OrganizationId == request.OrganizationId, cancellationToken);

        return ServiceDataResult<OrganizationDomain?>.WithData(project);
    }
}