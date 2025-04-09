using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Update;

namespace MyFeedbackHub.Api.Features.Users.Update;

public sealed record UpdateUserRequestDto(
    string Email,
    string FirstName,
    string LastName);

public sealed class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{id}", async (
            [FromQuery] Guid id,
            [FromBody] UpdateUserRequestDto request,
            ICommandHandler<UpdateUserCommand> command) =>
        {
            var result = await command.HandleAsync(new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email));
            if (result.HasFailed)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "User update failed",
                    extensions: new Dictionary<string, object?>()
                    {
                        ["errorCode"] = result.ErrorCode
                    });
            }

            return Results.Ok();
        })
        .WithName("UpdateUser")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Update user")
        .WithDescription("Update user")
        .WithTags("User")
        .RequireAuthorization();
    }
}
