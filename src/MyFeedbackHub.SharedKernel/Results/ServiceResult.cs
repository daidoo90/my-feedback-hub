namespace MyFeedbackHub.SharedKernel.Results;

public class ServiceResult
{
    protected ServiceResult() => IsSuccessful = true;

    protected ServiceResult(string errorCode)
    {
        ErrorCode = errorCode;
    }

    public bool IsSuccessful { get; }

    public string ErrorCode { get; }

    public static ServiceResult Success => new();

    public static ServiceResult WithError(string error) => new(error);

    public bool HasFailed => !IsSuccessful;
}
