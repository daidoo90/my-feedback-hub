using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Application.Organization;

public interface IOrganizationService
{
    Task<IEnumerable<Guid>> GetProjectsAsync(Guid organizationId, CancellationToken cancellationToken = default);

    Task<OrganizationDomain?> GetAsync(string name, CancellationToken cancellationToken = default);

    Task<OrganizationDomain?> GetAsync(Guid organizationId, CancellationToken cancellationToken = default);

    Task<IEnumerable<ProjectDomain>> GetAllProjectsAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
