using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.GetById;
using MyFeedbackHub.Domain;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users.GetById;

public sealed class UserResponseDto
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Username { get; set; } = null!;

    public UserRoleType Role { get; set; }

    public UserStatusType Status { get; set; }

    internal static UserResponseDto? From(UserDomain? userDomain)
    {
        if (userDomain == null)
        {
            return default;
        }

        return new UserResponseDto()
        {
            UserId = userDomain!.UserId,
            FirstName = userDomain!.FirstName,
            LastName = userDomain!.LastName,
            Username = userDomain!.Username,
            PhoneNumber = userDomain!.PhoneNumber,
            Role = userDomain.Role,
            Status = userDomain.Status
        };
    }
}

public sealed class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", async (
             Guid id,
             IQueryHandler<GetByUserIdQueryRequest, UserDomain?> handler,
             IUserContext userContext,
             CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetByUserIdQueryRequest(id), cancellationToken);
            if (result.HasFailed
                || result.Data == null
                || result.Data.OrganizationId != userContext.OrganizationId)
            {
                return Results.BadRequest("Get user by id failure");
            }

            return Results.Ok(UserResponseDto.From(result.Data));
        })
        .WithName("GetUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get user")
        .WithDescription("Get user")
        .WithTags("User")
        .RequireAuthorization();
    }
}