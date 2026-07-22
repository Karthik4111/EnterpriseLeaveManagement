using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EnterpriseLeaveManagement.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        Console.WriteLine($"Host: '{_settings.Host}'");
        Console.WriteLine($"Port: {_settings.Port}");
        Console.WriteLine($"Username: '{_settings.Username}'");
        Console.WriteLine($"SenderEmail: '{_settings.SenderEmail}'");

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _settings.SenderName,
            _settings.SenderEmail));

        message.To.Add(MailboxAddress.Parse(toEmail));

        message.Subject = subject;

        message.Body = new BodyBuilder
        {
            HtmlBody = body
        }.ToMessageBody();

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls);

            Console.WriteLine("✅ Connected");

            await client.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            Console.WriteLine("✅ Authenticated");

            await client.SendAsync(message);

            Console.WriteLine("✅ Mail Sent");

            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}