using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization;

public sealed record GetOrganizationByIdQuery(Guid OrganizationId);

public sealed class GetOrganizationByIdQueryHandler(
    IUnitOfWork unitOfWork) : IQueryHandler<GetOrganizationByIdQuery, OrganizationDomain?>
{
    public async Task<ServiceDataResult<OrganizationDomain?>> HandleAsync(GetOrganizationByIdQuery query, CancellationToken cancellationToken = default)
    {
        var project = await unitOfWork
            .DbContext
            .Organizations
            .Include(o => o.Projects)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.OrganizationId == query.OrganizationId, cancellationToken);

        return ServiceDataResult<OrganizationDomain?>.WithData(project);
    }
}