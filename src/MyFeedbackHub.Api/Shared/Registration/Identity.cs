using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyFeedbackHub.SharedKernel.Configurations;
using System.Text;

namespace MyFeedbackHub.Api.Shared.Registration;

internal static class Identity
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var identitySettings = new IdentityConfigurations();
        var identityConfigurationSection = builder.Configuration.GetSection(nameof(IdentityConfigurations));
        identityConfigurationSection.Bind(identitySettings);

        builder.Services.AddSingleton(identitySettings);

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
