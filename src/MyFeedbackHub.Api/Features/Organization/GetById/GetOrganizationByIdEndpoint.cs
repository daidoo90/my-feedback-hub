using MyFeedbackHub.Api.Features.Project.GetById;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Organization.GetById;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Api.Features.Organization.GetById;

public sealed class OrganizationResponseDto
{
    public Guid OrganizationId { get; init; }
    public string Name { get; init; }
    public string TaxId { get; init; }
    public IEnumerable<ProjectResponseDto> Projects { get; init; }

    internal static OrganizationResponseDto? From(OrganizationDomain? organizationDomain)
    {
        if (organizationDomain == null)
        {
            return default;
        }

        return new OrganizationResponseDto
        {
            OrganizationId = organizationDomain.OrganizationId,
            Name = organizationDomain!.Name,
            TaxId = organizationDomain!.TaxID,
            Projects = organizationDomain!.Projects.Select(ProjectResponseDto.From)
        };
    }
}

public sealed class GetOrganizationByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/organizations", async (
            IQueryHandler<GetOrganizationByIdQueryRequest, OrganizationDomain?> handler,
            IUserContext userContext,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetOrganizationByIdQueryRequest(userContext.OrganizationId), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get organization failure");
            }

            return Results.Ok(OrganizationResponseDto.From(result.Data));
        })
        .WithName("Get organization by id")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get organization")
        .WithDescription("Get organization")
        .WithTags("Organization")
        .RequireAuthorization();
    }
}