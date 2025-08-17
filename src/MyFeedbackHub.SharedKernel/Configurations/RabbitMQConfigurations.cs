using System.ComponentModel.DataAnnotations;

namespace MyFeedbackHub.SharedKernel.Configurations;
public sealed record RabbitMQConfigurations
{
    public const string ConfigurationName = "RabbitMQ";

    [Required]
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string QueueName { get; set; } = string.Empty;
}
