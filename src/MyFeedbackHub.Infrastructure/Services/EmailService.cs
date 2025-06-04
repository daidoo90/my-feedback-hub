using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Configurations;

namespace MyFeedbackHub.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailConfigurations> _emailConfigurations;

    public EmailService(IOptions<EmailConfigurations> emailConfigurations)
        => _emailConfigurations = emailConfigurations;

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailConfigurations.Value.FromEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; // DEV ONLY
        await smtp.ConnectAsync(
            _emailConfigurations.Value.Host,
            _emailConfigurations.Value.Port,
            MailKit.Security.SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _emailConfigurations.Value.Username,
            _emailConfigurations.Value.Password);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
