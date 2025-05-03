using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.GetById;

public sealed record GetOrganizationByIdQuery(
    Guid OrganizationId);

public sealed class GetOrganizationByIdQueryHandler(
    IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetOrganizationByIdQuery, OrganizationDomain?>
{
    public async Task<ServiceDataResult<OrganizationDomain?>> HandleAsync(GetOrganizationByIdQuery query, CancellationToken cancellationToken = default)
    {
        var project = await feedbackHubDbContext
            .Organizations
            .Include(o => o.Projects)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.OrganizationId == query.OrganizationId, cancellationToken);

        return ServiceDataResult<OrganizationDomain?>.WithData(project);
    }
}