using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Infrastructure.Common;
using MyFeedbackHub.Infrastructure.Services;
using MyFeedbackHub.SharedKernel.Configurations;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class Infrastructure
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDb(builder);

        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var dbSettings = new DatabaseConfigurations();
        var dbConfigurationSection = builder.Configuration.GetSection(nameof(DatabaseConfigurations));
        dbConfigurationSection.Bind(dbSettings);

        services.AddSingleton(dbSettings);
        services.AddSingleton<ICryptoService, CryptoService>();

        services.AddInfra(dbSettings.ConnectionString);

        return services;
    }
}
