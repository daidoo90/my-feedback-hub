using System.ComponentModel.DataAnnotations;

namespace MyFeedbackHub.SharedKernel.Configurations;

public sealed class DatabaseConfigurations
{
    public const string ConfigurationName = "Database";

    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}
