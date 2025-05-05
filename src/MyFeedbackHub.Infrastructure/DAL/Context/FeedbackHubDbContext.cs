using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Infrastructure.DAL.Configurators;

namespace MyFeedbackHub.Infrastructure.DAL.Context;

public class FeedbackHubDbContext : DbContext, IFeedbackHubDbContext
{
    public FeedbackHubDbContext()
    {

    }

    public FeedbackHubDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OrganizationDomain> Organizations { get; set; }
    public DbSet<ProjectDomain> Projects { get; set; }
    public DbSet<UserDomain> Users { get; set; }
    public DbSet<ProjectAccess> ProjectAccess { get; set; }
    public DbSet<FeedbackDomain> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
    }
}
