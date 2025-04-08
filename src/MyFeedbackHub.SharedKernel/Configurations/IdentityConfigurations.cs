namespace MyFeedbackHub.SharedKernel.Configurations;

public class IdentityConfigurations
{
    public string SecretKey { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
}
