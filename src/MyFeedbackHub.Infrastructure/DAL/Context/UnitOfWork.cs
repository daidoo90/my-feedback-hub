using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Infrastructure.DAL.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly FeedbackHubDbContext _dbContext;

    public UnitOfWork(FeedbackHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _dbContext.Dispose();

    public ValueTask DisposeAsync() => _dbContext.DisposeAsync();

    public IFeedbackHubDbContext DbContext => _dbContext;
}