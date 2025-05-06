using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Services;

public interface IUserService
{
    Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<UserDomain>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
