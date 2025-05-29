using FluentValidation;
using MyFeedbackHub.Api.Services;
using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Project;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Shared.Domains;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Application.Users.SendWelcomeEmail;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Infrastructure.Services;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class ApplicationDomain
{
    internal static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICommandHandler<CreateNewOrganizationCommand>, CreateNewOrganizationCommandHandler>();
        services.AddScoped<IQueryHandler<GetOrganizationByIdQuery, OrganizationDomain?>, GetOrganizationByIdQueryHandler>();
        services.AddScoped<ICommandHandler<UpdateOrganizationCommand>, UpdateOrganizationCommandHandler>();

        services.AddScoped<IQueryHandler<GetByUserIdQuery, UserDomain>, GetByUserIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllUsersQuery, GetAllUsersResponse>, GetAllUsersQueryHandler>();
        services.AddScoped<ICommandHandler<CreateNewUserCommand, CreateNewUserCommandResult>, CreateNewUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
        services.AddScoped<ICommandHandler<SetPasswordCommand>, SetPasswordCommandHandler>();

        services.AddScoped<ICommandHandler<CreateNewProjectCommand>, CreateNewProjectCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateProjectCommand>, UpdateProjectCommandHandler>();
        services.AddScoped<IQueryHandler<GetProjectByIdQuery, ProjectDomain>, GetProjectByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllProjectsQuery, GetAllProjectsResponse>, GetAllProjectsQueryHandler>();

        services.AddScoped<ICommandHandler<CreateNewFeedbackCommand>, AddFeedbackCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateFeedbackCommand>, UpdateFeedbackCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteFeedbackCommand>, DeleteFeedbackCommandHandler>();
        services.AddScoped<IQueryHandler<GetFeedbackByIdQuery, GetFeedbackByIdResponse>, GetFeedbackByIdQueryHandler>();

        services.AddScoped<ICommandHandler<CreateNewCommentCommand>, AddCommentToFeedbackCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCommentCommand>, UpdateCommentCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCommentCommand>, DeleteCommentCommandHandler>();
        services.AddScoped<IQueryHandler<GetCommentsQuery, IEnumerable<CommentResponse>>, GetCommentsQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllFeedbacksQuery, GetAllFeedbacksResponse>, GetAllFeedbacksQueryHandler>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserInvitationService, UserInvitationService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        services.AddValidators();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IDomainEventHandler<UserCreatedDomainEvent>, SendWelcomeEmailEventHandler>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        var applicationAssembly = typeof(CreateNewUserCommandValidator).Assembly;
        var validatorTypes = applicationAssembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Select(type => new
            {
                Implementation = type,
                ValidatorInterface = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(IValidator<>))
            })
            .Where(x => x.ValidatorInterface != null);

        foreach (var validator in validatorTypes)
        {
            services.AddScoped(validator.ValidatorInterface, validator.Implementation);
        }

        return services;
    }
}
