namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IFeedbackHubDbContext DbContext { get; }
}