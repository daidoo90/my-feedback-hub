using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Infrastructure.DAL.Context;

public sealed class FeedbackHubDbContextFactoryAdapter(IDbContextFactory<FeedbackHubDbContext> dbContextFactory)
    : IFeedbackHubDbContextFactory
{
    public async Task<IFeedbackHubDbContext> CreateAsync(CancellationToken cancellationToken = default)
        => await dbContextFactory.CreateDbContextAsync(cancellationToken);
}