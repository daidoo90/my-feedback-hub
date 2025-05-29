using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Infrastructure.DAL.Context;

namespace MyFeedbackHub.Infrastructure.Common;

public static class ServiceCollectionRegistration
{
    public static IServiceCollection AddInfra(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IFeedbackHubDbContextFactory, FeedbackHubDbContextFactory>();

        services.AddDbContextFactory<FeedbackHubDbContext>(options =>
        {
            options.UseNpgsql(connectionString, options =>
            {
                options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        return services;
    }
}
