using LeveGestao.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LeveGestao.Infrastructure.Services;

public class EmailSettings
{
    public string SmtpHost { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FromName { get; set; } = "";
    public string FromEmail { get; set; } = "";
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task EnviarAsync(string destinatario, string nomeDestinatario, string assunto, string corpo)
    {
        var mensagem = new MimeMessage();
        mensagem.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        mensagem.To.Add(new MailboxAddress(nomeDestinatario, destinatario));
        mensagem.Subject = assunto;
        mensagem.Body = new TextPart("plain") { Text = corpo };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(mensagem);
        await client.DisconnectAsync(true);
    }
}
