using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetById;

public sealed record GetByIdQueryRequest(string Username);

public sealed class GetByIdQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetByIdQueryRequest, UserDomain?>
{
    public async Task<ServiceDataResult<UserDomain?>> HandleAsync(GetByIdQueryRequest request, CancellationToken cancellationToken = default)
    {
        var user = await feedbackHubDbContext
            .Users
            .SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        return ServiceDataResult<UserDomain?>.WithData(user);
    }
}
