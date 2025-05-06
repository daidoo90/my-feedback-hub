using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IFeedbackHubDbContext
{
    DbSet<OrganizationDomain> Organizations { get; }
    DbSet<ProjectDomain> Projects { get; }
    DbSet<UserDomain> Users { get; }
    DbSet<ProjectAccess> ProjectAccess { get; }
    DbSet<FeedbackDomain> Feedbacks { get; }
    DbSet<CommentDomain> Comments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IFeedbackHubDbContextFactory
{
    Task<IFeedbackHubDbContext> CreateAsync(CancellationToken cancellationToken = default);
}

