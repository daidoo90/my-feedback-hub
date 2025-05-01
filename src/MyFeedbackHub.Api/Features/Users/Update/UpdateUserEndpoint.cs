using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Update;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users.Update;

public sealed record UpdateUserRequestDto(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber);

public sealed class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{id}", async (
            Guid id,
            [FromBody] UpdateUserRequestDto request,
            ICommandHandler<UpdateUserCommand> command,
            IUserContext userContext) =>
        {
            if (userContext.Role == UserRoleType.Customer)
            {
                return Results.Forbid();
            }

            if (userContext.Role == UserRoleType.TeamMember
                && userContext.UserId != id)
            {
                return Results.Forbid();
            }

            var result = await command.HandleAsync(new UpdateUserCommand(id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber));
            if (result.HasFailed)
            {
                return result.ToBadRequest("User update failure");
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
