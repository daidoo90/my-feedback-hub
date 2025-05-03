namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IUserService
{
    Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default);
}
