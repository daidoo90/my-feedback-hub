using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IQueryHandler<in TQueryRequest, TResponse> : IQueryHandler
    where TQueryRequest : class
{
    Task<ServiceDataResult<TResponse>> HandleAsync(TQueryRequest query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TResponse> : IQueryHandler
    where TResponse : class
{
    Task<ServiceDataResult<TResponse>> HandleAsync(CancellationToken cancellationToken = default);
}

public interface IQueryHandler
{ }