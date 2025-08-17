using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class OrganizationService(IUnitOfWork unitOfWork) : IOrganizationService
{
    public async Task<IEnumerable<ProjectDomain>> GetAllProjectsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await unitOfWork
            .DbContext
            .Projects
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<OrganizationDomain?> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        return await unitOfWork
            .DbContext
            .Organizations
            .AsNoTracking()
            .Where(p => p.Name.ToLower().Equals(name.ToLower()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OrganizationDomain?> GetAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await unitOfWork
            .DbContext
            .Organizations
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<IEnumerable<Guid>> GetProjectsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await unitOfWork
            .DbContext
            .Projects
            .Where(p => p.OrganizationId == organizationId)
            .AsNoTracking()
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);
    }
}
