using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface ICommandHandler<in TCommandRequest, TResponse> : ICommandHandler
    where TCommandRequest : class
    where TResponse : class
{
    Task<ServiceDataResult<TResponse>> HandleAsync(TCommandRequest command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommandRequest> : ICommandHandler
    where TCommandRequest : class
{
    Task<ServiceResult> HandleAsync(TCommandRequest command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler
{
}