using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.GetById;

public sealed record GetByUserIdQueryRequest(Guid userId);

public sealed class GetByUserIdQueryHandler(IFeedbackHubDbContext feedbackHubDbContext) : IQueryHandler<GetByUserIdQueryRequest, UserDomain?>
{
    public async Task<ServiceDataResult<UserDomain?>> HandleAsync(GetByUserIdQueryRequest request, CancellationToken cancellationToken = default)
    {
        var user = await feedbackHubDbContext
            .Users
            .SingleOrDefaultAsync(u => u.UserId == request.userId, cancellationToken);

        return ServiceDataResult<UserDomain?>.WithData(user);
    }
}
