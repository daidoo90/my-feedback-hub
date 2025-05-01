using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFeedbackHub.SharedKernel.Configurations;
using System.Text;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class Identity
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<IdentityConfigurations>()
            .BindConfiguration(IdentityConfigurations.ConfigurationName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var provider = services.BuildServiceProvider();
        var identitySettings = provider.GetRequiredService<IOptions<IdentityConfigurations>>().Value;

        builder.Services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(bearerOptions =>
               {
                   bearerOptions.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = identitySettings.Issuer,
                       ValidAudience = identitySettings.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.SecretKey))
                   };
               });

        return services;
    }
}
