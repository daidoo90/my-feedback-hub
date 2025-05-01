using MyFeedbackHub.Api.Services;
using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Application.Organization.Create;
using MyFeedbackHub.Application.Organization.GetById;
using MyFeedbackHub.Application.Organization.Update;
using MyFeedbackHub.Application.Project.Create;
using MyFeedbackHub.Application.Project.GetAll;
using MyFeedbackHub.Application.Project.GetById;
using MyFeedbackHub.Application.Project.Update;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Create;
using MyFeedbackHub.Application.Users.Delete;
using MyFeedbackHub.Application.Users.GetAll;
using MyFeedbackHub.Application.Users.GetById;
using MyFeedbackHub.Application.Users.GetByUsername;
using MyFeedbackHub.Application.Users.Update;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class ApplicationDomain
{
    internal static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICommandHandler<CreateNewOrganizationCommand>, CreateNewOrganizationCommandHandler>();
        services.AddScoped<IQueryHandler<GetOrganizationByIdQueryRequest, OrganizationDomain?>, GetOrganizationByIdQueryHandler>();
        services.AddScoped<ICommandHandler<UpdateOrganizationCommand>, UpdateOrganizationCommandHandler>();

        services.AddScoped<IQueryHandler<GetByUserIdQueryRequest, UserDomain?>, GetByUserIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserByUsernameRequest, UserDomain?>, GetUserByUsernameQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllUsersQueryRequest, GetAllUsersResponse>, GetAllUsersQueryHandler>();
        services.AddScoped<ICommandHandler<CreateNewUserCommand>, CreateNewUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();

        services.AddScoped<ICommandHandler<CreateNewProjectCommand>, CreateNewProjectCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateProjectCommand>, UpdateProjectCommandHandler>();
        services.AddScoped<IQueryHandler<GetProjectByIdQueryRequest, ProjectDomain?>, GetProjectByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllProjectsQueryRequest, GetAllProjectsResponse>, GetAllProjectsQueryHandler>();

        return services;
    }
}
