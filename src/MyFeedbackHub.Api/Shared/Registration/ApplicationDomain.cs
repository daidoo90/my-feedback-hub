using MyFeedbackHub.Api.Services;
using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Application.Business.Create;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Create;
using MyFeedbackHub.Application.Users.GetById;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class ApplicationDomain
{
    internal static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICommandHandler<CreateNewBusinessCommand>, CreateNewBusinessCommandHandler>();
        services.AddScoped<IQueryHandler<GetByIdQueryRequest, UserDomain?>, GetByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateNewUserCommand>, CreateNewUserCommandHandler>();

        return services;
    }
}
