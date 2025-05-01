using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Api.Shared.Utils;

internal static class ServiceResultExtensions
{
    internal static IResult ToBadRequest(this ServiceResult serviceResult, string title)
    {
        return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: title,
                    extensions: new Dictionary<string, object?>()
                    {
                        ["errorCode"] = serviceResult.ErrorCode
                    });
    }
}
