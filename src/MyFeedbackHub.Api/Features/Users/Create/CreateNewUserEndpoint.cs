using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Create;

namespace MyFeedbackHub.Api.Features.Users.Create;

public sealed record CreateNewUserRequestDto(
    string Username,
    string Password,
    Guid BusinessId);

public sealed class CreateNewUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            CreateNewUserRequestDto request,
            ICommandHandler<CreateNewUserCommand> command,
            CancellationToken cancellationToken = default) =>
        {
            var result = await command.HandleAsync(
                new CreateNewUserCommand(request.Username, request.Password, request.BusinessId),
                cancellationToken);

            if (result.HasFailed)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "User creation failure",
                    extensions: new Dictionary<string, object?>()
                    {
                        ["errorCode"] = result.ErrorCode
                    });
            }

            return Results.Ok();
        })
        .WithName("CreateNewUser")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Create new user")
        .WithDescription("Create new user")
        .WithTags("User")
        .RequireAuthorization();
    }
}
