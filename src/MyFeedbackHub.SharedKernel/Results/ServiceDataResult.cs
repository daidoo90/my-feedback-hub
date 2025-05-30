namespace MyFeedbackHub.SharedKernel.Results;

public sealed class ServiceDataResult<TData> : ServiceResult
{
    private ServiceDataResult(TData data, string errorCode)
        : base(errorCode)
    {
        Data = data;
    }

    private ServiceDataResult(TData data)
    {
        Data = data;
    }

    public TData Data { get; }

    public static ServiceDataResult<TData> WithData(TData data) => new(data);

    public static new ServiceDataResult<TData> WithError(string errorCode) => new(default, errorCode);
}
