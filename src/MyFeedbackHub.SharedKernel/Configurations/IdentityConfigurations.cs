using System.ComponentModel.DataAnnotations;

namespace MyFeedbackHub.SharedKernel.Configurations;

public sealed class IdentityConfigurations
{
    public const string ConfigurationName = "Identity";

    [Required]
    public string SecretKey { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;
}
