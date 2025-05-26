using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
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
            ICommandHandler<UpdateUserCommand> commandHandler,
            IUserContext currentUser,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.Role == UserRoleType.Customer && currentUser.UserId != id)
            {
                return Results.Forbid();
            }

            if (currentUser.Role == UserRoleType.TeamMember && currentUser.UserId != id)
            {
                return Results.Forbid();
            }

            if (currentUser.Role == UserRoleType.ProjectAdmin)
            {
                var currentUserProjectIds = await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);
                var updatedUserProjectIds = await userService.GetProjectIdsAsync(id, cancellationToken);

                if (!updatedUserProjectIds.Any(p => currentUserProjectIds.Contains(p)))
                {
                    return Results.Forbid();
                }
            }

            var result = await commandHandler.HandleAsync(new UpdateUserCommand(id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber),
                cancellationToken);

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
