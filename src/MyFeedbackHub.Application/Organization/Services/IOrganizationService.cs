﻿using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Application.Organization.Services;

public interface IOrganizationService
{
    Task<IEnumerable<Guid>> GetProjectsAsync(Guid organizationId, CancellationToken cancellationToken = default);

    Task<OrganizationDomain?> GetAsync(string name, CancellationToken cancellationToken = default);
}
