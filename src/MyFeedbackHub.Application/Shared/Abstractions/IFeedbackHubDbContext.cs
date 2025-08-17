using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

    DatabaseFacade Database { get; }
}

public interface IFeedbackHubDbContextFactory
{
    IFeedbackHubDbContext Create();
}

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IFeedbackHubDbContext DbContext { get; }
}