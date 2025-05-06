using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetById;

public sealed record GetByUserIdQuery(Guid userId);

public sealed class GetByUserIdQueryHandler(IFeedbackHubDbContextFactory dbContextFactory)
    : IQueryHandler<GetByUserIdQuery, UserDomain>
{
    public async Task<ServiceDataResult<UserDomain>> HandleAsync(GetByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var user = await dbContext
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
