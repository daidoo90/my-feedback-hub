using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
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

    public DbSet<UserDomain> Users { get; set; }

    public DbSet<BusinessDomain> Businesses { get; set; }

    public DbSet<BoardDomain> Boards { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
    }
}
