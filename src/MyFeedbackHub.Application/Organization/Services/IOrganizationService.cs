namespace MyFeedbackHub.Application.Organization.Services;

public interface IOrganizationService
{
    Task<IEnumerable<Guid>> GetProjectsAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
