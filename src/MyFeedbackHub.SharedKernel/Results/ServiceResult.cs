namespace MyFeedbackHub.SharedKernel.Results;

public class ServiceResult
{
    public ServiceResult()
    {
        IsSuccessful = true;
    }

    public ServiceResult(string errorCode)
    {
        ErrorCode = errorCode;
        IsSuccessful = false;
    }

    public bool IsSuccessful { get; }

    public string ErrorCode { get; }

    public static ServiceResult Success => new();

    public static ServiceResult WithError(string error) => new(error);

    public bool HasFailed => !IsSuccessful;
}
