using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Shared.Domains;

namespace MyFeedbackHub.Infrastructure.DAL.Context;

public class FeedbackHubDbContextFactory : IDbContextFactory<FeedbackHubDbContext>, IFeedbackHubDbContextFactory
{
    private readonly IServiceProvider _provider;

    public FeedbackHubDbContextFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public FeedbackHubDbContext CreateDbContext()
    {
        var options = _provider.GetRequiredService<DbContextOptions<FeedbackHubDbContext>>();
        var dispatcher = _provider.GetRequiredService<IDomainEventDispatcher>();
        return new FeedbackHubDbContext(options);
    }

    public IFeedbackHubDbContext Create()
    {
        var options = _provider.GetRequiredService<DbContextOptions<FeedbackHubDbContext>>();
        var dispatcher = _provider.GetRequiredService<IDomainEventDispatcher>();
        return new FeedbackHubDbContext(options);
    }
}