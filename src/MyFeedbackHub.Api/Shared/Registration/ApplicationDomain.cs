using FluentValidation;
using MyFeedbackHub.Api.Services;
using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Application.Feedback.Create;
using MyFeedbackHub.Application.Feedback.CreateComment;
using MyFeedbackHub.Application.Feedback.DeleteComment;
using MyFeedbackHub.Application.Feedback.DeleteFeedback;
using MyFeedbackHub.Application.Feedback.GetAllFeedbacks;
using MyFeedbackHub.Application.Feedback.GetComments;
using MyFeedbackHub.Application.Feedback.GetFeedbackById;
using MyFeedbackHub.Application.Feedback.Services;
using MyFeedbackHub.Application.Feedback.Update;
using MyFeedbackHub.Application.Feedback.UpdateComment;
using MyFeedbackHub.Application.Organization.Create;
using MyFeedbackHub.Application.Organization.GetById;
using MyFeedbackHub.Application.Organization.Services;
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
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Application.Users.SetPassword;
using MyFeedbackHub.Application.Users.Update;
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

        services.AddScoped<ICommandHandler<CreateNewFeedbackCommand>, CreateNewFeedbackCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateFeedbackCommand>, UpdateFeedbackCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteFeedbackCommand>, DeleteFeedbackCommandHandler>();
        services.AddScoped<IQueryHandler<GetFeedbackByIdQuery, GetFeedbackByIdResponse>, GetFeedbackByIdQueryHandler>();

        services.AddScoped<ICommandHandler<CreateNewCommentCommand>, CreateNewCommentCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCommentCommand>, UpdateCommentCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCommentCommand>, DeleteCommentCommandHandler>();
        services.AddScoped<IQueryHandler<GetCommentsQuery, IEnumerable<CommentResponse>>, GetCommentsQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllFeedbacksQuery, GetAllFeedbacksResponse>, GetAllFeedbacksQueryHandler>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserInvitationService, UserInvitationService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        services.AddScoped<IValidator<CreateNewOrganizationCommand>, CreateNewOrganizationCommandValidator>();
        services.AddScoped<IValidator<UpdateOrganizationCommand>, UpdateOrganizationCommandValidator>();

        services.AddScoped<IValidator<CreateNewProjectCommand>, CreateNewProjectCommandValidator>();
        services.AddScoped<IValidator<UpdateProjectCommand>, UpdateProjectCommandValidator>();

        services.AddScoped<IValidator<CreateNewUserCommand>, CreateNewUserCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();

        return services;
    }
}
