using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetByUsername;

public sealed record GetUserByUsernameRequest(string Username);

public class GetUserByUsernameQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetUserByUsernameRequest, UserDomain?>
{
    public async Task<ServiceDataResult<UserDomain?>> HandleAsync(GetUserByUsernameRequest request, CancellationToken cancellationToken = default)
    {
        var user = await feedbackHubDbContext
            .Users
            .SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        return ServiceDataResult<UserDomain?>.WithData(user);
    }
}
