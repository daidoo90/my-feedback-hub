using MyFeedbackHub.Api.Features.Users.GetById;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.GetAll;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users.GetAll;

public sealed class UsersResponseDto
{
    public int TotalCount { get; init; }

    public IEnumerable<UserResponseDto> Users { get; init; }

    internal static UsersResponseDto? From(GetAllUsersResponse usersResponse)
    {
        return new UsersResponseDto
        {
            TotalCount = usersResponse.TotalCount,
            Users = usersResponse
                .Users
                .Select(UserResponseDto.From)
                .ToList(),
        };
    }
}


public sealed class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (
           int? pageNumber,
           int? pageSize,
           IQueryHandler<GetAllUsersQuery, GetAllUsersResponse> queryHandler,
           IUserContext currentUser,
           IUserService userService,
           CancellationToken cancellationToken) =>
        {
            if (currentUser.Role == UserRoleType.Customer
                || currentUser.Role == UserRoleType.TeamMember)
            {
                return Results.Forbid();
            }

            var projectIds = currentUser.Role == UserRoleType.ProjectAdmin
                            ? await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken)
                            : [];

            var queryRequest = new GetAllUsersQuery(pageNumber, pageSize, currentUser.OrganizationId, projectIds);
            var result = await queryHandler.HandleAsync(queryRequest, cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get all users failure");
            }

            return Results.Ok(UsersResponseDto.From(result!.Data));
        })
       .WithName("GetUsers")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get users")
        .WithDescription("Get users")
        .WithTags("User")
        .RequireAuthorization();
    }
}