using System.ComponentModel.DataAnnotations;

namespace MyFeedbackHub.SharedKernel.Configurations;

public sealed class RedisConfigurations
{
    public const string ConfigurationName = "Redis";

    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    [Required]
    public string InstanceName { get; set; } = string.Empty;
}