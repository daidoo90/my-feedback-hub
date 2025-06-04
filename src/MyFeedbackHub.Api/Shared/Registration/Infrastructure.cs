using Microsoft.Extensions.Options;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Infrastructure.Common;
using MyFeedbackHub.Infrastructure.Services;
using MyFeedbackHub.SharedKernel.Configurations;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class Infrastructure
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddEmail(builder);

        services.AddDb(builder);

        services.AddRedis(builder);

        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<DatabaseConfigurations>()
            .BindConfiguration(DatabaseConfigurations.ConfigurationName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var provider = services.BuildServiceProvider();
        var dbSettings = provider.GetRequiredService<IOptions<DatabaseConfigurations>>().Value;

        services.AddSingleton<ICryptoService, CryptoService>();

        services.AddInfra(dbSettings.ConnectionString);

        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<RedisConfigurations>()
            .BindConfiguration(RedisConfigurations.ConfigurationName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var provider = services.BuildServiceProvider();
        var redisSettings = provider.GetRequiredService<IOptions<RedisConfigurations>>().Value;

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings.ConnectionString;
            options.InstanceName = redisSettings.InstanceName;
        });

        return services;
    }

    private static IServiceCollection AddEmail(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<EmailConfigurations>()
            .BindConfiguration(EmailConfigurations.ConfigurationName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
