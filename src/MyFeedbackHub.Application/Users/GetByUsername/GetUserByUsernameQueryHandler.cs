using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetByUsername;

public sealed record GetUserByUsernameQuery(string Username);

public class GetUserByUsernameQueryHandler(IFeedbackHubDbContextFactory dbContextFactory)
    : IQueryHandler<GetUserByUsernameQuery, UserDomain?>
{
    public async Task<ServiceDataResult<UserDomain?>> HandleAsync(GetUserByUsernameQuery query, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var user = await dbContext
            .Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == query.Username, cancellationToken);

        return ServiceDataResult<UserDomain?>.WithData(user);
    }
}
