using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Create;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users.Create;

public sealed record CreateNewUserRequestDto(
    string Username,
    string Password,
    UserRoleType Role);

public sealed class CreateNewUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            CreateNewUserRequestDto request,
            ICommandHandler<CreateNewUserCommand> command,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var result = await command.HandleAsync(
                new CreateNewUserCommand(
                    request.Username,
                    request.Password,
                    userContext.OrganizationId,
                    userContext.Role),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("User creation failure");
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
