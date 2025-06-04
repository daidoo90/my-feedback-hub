namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
