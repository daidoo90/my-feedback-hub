using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetByUsername;

public sealed record GetUserByUsernameQuery(string Username);

public class GetUserByUsernameQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetUserByUsernameQuery, UserDomain?>
{
    public async Task<ServiceDataResult<UserDomain?>> HandleAsync(GetUserByUsernameQuery query, CancellationToken cancellationToken = default)
    {
        var user = await feedbackHubDbContext
            .Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == query.Username, cancellationToken);

        return ServiceDataResult<UserDomain?>.WithData(user);
    }
}
