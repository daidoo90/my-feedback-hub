using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IFeedbackHubDbContext
{
    DbSet<OrganizationDomain> Organizations { get; }
    DbSet<ProjectDomain> Projects { get; }
    DbSet<UserDomain> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
