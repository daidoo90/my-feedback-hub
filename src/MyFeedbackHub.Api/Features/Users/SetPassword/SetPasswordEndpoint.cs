using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Api.Features.Users;

public sealed record SetNewPasswordRequestDto(
    string InvitationToken,
    string Username,
    string Password);

public sealed class SetPasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/set-password", async (
            SetNewPasswordRequestDto request,
            ICommandHandler<SetPasswordCommand> commandHandler,
            IUserContext currentUser,
            IUserInvitationService userInvitationService,
            CancellationToken cancellationToken = default) =>
        {
            var token = await userInvitationService.GetInvitationTokenAsync(request.InvitationToken);
            if (string.IsNullOrEmpty(token) || !token.Equals(request.InvitationToken))
            {
                return ServiceResult.WithError(ErrorCodes.User.InvitationTokenInvalid).ToBadRequest("Set password failure");
            }

            var result = await commandHandler.HandleAsync(
                new SetPasswordCommand(
                    request.Username,
                    request.Password),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Set password failure");
            }

            return Results.Ok();
        })
        .WithName("SetPassword")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Set password")
        .WithDescription("Set password")
        .WithTags("User")
        .AllowAnonymous();
    }
}