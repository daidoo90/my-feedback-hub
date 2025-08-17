using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record GetAllFeedbacksQuery(
    int? PageNumber,
    int? PageSize,
    IEnumerable<Guid>? projectIds);

public sealed record GetAllFeedbacksResponse(
    int TotalCount,
    IEnumerable<FeedbackDomain> Feedbacks);


public sealed class GetAllFeedbacksQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetAllFeedbacksQuery, GetAllFeedbacksResponse>
{
    public async Task<ServiceDataResult<GetAllFeedbacksResponse>> HandleAsync(
        GetAllFeedbacksQuery query,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = query!.PageNumber.HasValue ? query.PageNumber.Value : 1;
        var pageSize = query!.PageSize.HasValue ? query.PageSize.Value : 10;

        var allFeedbacks = unitOfWork
            .DbContext
            .Feedbacks
            .Where(f => query.projectIds.Contains(f.ProjectId) && !f.IsDeleted);

        var totalCount = await allFeedbacks.CountAsync(cancellationToken);
        var feedbacks = await allFeedbacks
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ServiceDataResult<GetAllFeedbacksResponse>.WithData(new GetAllFeedbacksResponse(totalCount, feedbacks));
    }
}
