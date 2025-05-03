using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetById;

public sealed record GetByUserIdQuery(Guid userId);

public sealed class GetByUserIdQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetByUserIdQuery, UserDomain?>
{
    public async Task<ServiceDataResult<UserDomain?>> HandleAsync(GetByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        var user = await feedbackHubDbContext
            .Users
            .Include(u => u.ProjectAccess)
            .ThenInclude(pa => pa.Project)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserId == query.userId, cancellationToken);

        return ServiceDataResult<UserDomain?>.WithData(user);
    }
}
