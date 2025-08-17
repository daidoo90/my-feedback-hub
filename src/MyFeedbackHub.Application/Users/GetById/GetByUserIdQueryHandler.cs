using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record GetByUserIdQuery(Guid userId);

public sealed class GetByUserIdQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetByUserIdQuery, UserDomain>
{
    public async Task<ServiceDataResult<UserDomain>> HandleAsync(GetByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork
            .DbContext
            .Users
            .Include(u => u.ProjectAccess)
            .ThenInclude(pa => pa.Project)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserId == query.userId, cancellationToken);

        if (user == null)
        {
            return ServiceDataResult<UserDomain>.WithError(ErrorCodes.User.NotFound);
        }

        return ServiceDataResult<UserDomain>.WithData(user);
    }
}
