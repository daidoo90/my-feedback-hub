using System.ComponentModel.DataAnnotations;

namespace MyFeedbackHub.SharedKernel.Configurations;

public sealed class EmailConfigurations
{
    public const string ConfigurationName = "Email";

    [Required]
    public string Host { get; init; } = string.Empty;

    [Required]
    public int Port { get; init; }

    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    [Required]
    public string FromEmail { get; init; } = string.Empty;
}
