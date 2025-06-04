using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization;

public sealed record GetOrganizationByIdQuery(Guid OrganizationId);

public sealed class GetOrganizationByIdQueryHandler(
    IFeedbackHubDbContextFactory dbContextFactory) : IQueryHandler<GetOrganizationByIdQuery, OrganizationDomain?>
{
    public async Task<ServiceDataResult<OrganizationDomain?>> HandleAsync(GetOrganizationByIdQuery query, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();
        var project = await dbContext
            .Organizations
            .Include(o => o.Projects)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.OrganizationId == query.OrganizationId, cancellationToken);

        return ServiceDataResult<OrganizationDomain?>.WithData(project);
    }
}