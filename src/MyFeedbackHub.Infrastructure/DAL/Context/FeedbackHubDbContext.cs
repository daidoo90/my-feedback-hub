using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Shared.Domains;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Shared.Domains;
using MyFeedbackHub.Infrastructure.DAL.Configurators;

namespace MyFeedbackHub.Infrastructure.DAL.Context;

public class FeedbackHubDbContext : DbContext, IFeedbackHubDbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public FeedbackHubDbContext()
    {

    }

    public FeedbackHubDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public FeedbackHubDbContext(
        DbContextOptions options,
        IDomainEventDispatcher domainEventDispatcher)
        : this(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public DbSet<OrganizationDomain> Organizations { get; set; }
    public DbSet<ProjectDomain> Projects { get; set; }
    public DbSet<UserDomain> Users { get; set; }
    public DbSet<ProjectAccess> ProjectAccess { get; set; }
    public DbSet<FeedbackDomain> Feedbacks { get; set; }
    public DbSet<CommentDomain> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseDomain>()
            .Where(e => e.Entity.DomainEvents.Any())
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Sending domain events
        await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);

        // Clear domain events
        foreach (var entry in ChangeTracker.Entries<BaseDomain>())
            entry.Entity.ClearDomainEvents();

        return result;
    }
}
