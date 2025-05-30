using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailSender.Exceptions;
using MimeKit;
using Unfucked;
using AuthenticationException = MailSender.Exceptions.AuthenticationException;

namespace MailSender;

internal class MailSender(string host, ushort port, SecureSocketOptions options, string? username, string? password): IDisposable {

    private readonly ISmtpClient smtpClient = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput(), true));

    public async Task sendEmail(string fromName, string fromAddress, string toName, string toAddress, string subject, MimeEntity body) {
        MimeMessage message = new() {
            Subject = subject,
            Body    = body,
            From    = { new MailboxAddress(fromName, fromAddress) },
            To      = { new MailboxAddress(toName, toAddress) }
        };

        try {
            Console.WriteLine($"Connecting to SMTP server {host}:{port}...");
            await smtpClient.ConnectAsync(host, port, options);
        } catch (Exception e) {
            throw new ConnectionException($"Failed to connect to SMTP server {host}:{port}", e);
        }

        if (username.HasText() && password.HasText()) {
            try {
                Console.WriteLine($"Logging in as {username}...");
                await smtpClient.AuthenticateAsync(new SaslMechanismLogin(username, password));
            } catch (Exception e) {
                throw new AuthenticationException($"Failed to log in to SMTP server {host}:{port} as {username}", e);
            }
        }

        try {
            Console.WriteLine("Sending message...");
            await smtpClient.SendAsync(message);
        } catch (Exception e) {
            throw new SendingException($"Failed to send message \"{subject}\" to {toAddress}", e);
        }

        Console.WriteLine("Sent.\nDisconnecting...");
        await smtpClient.DisconnectAsync(true);
        Console.WriteLine("Disconnected.");
    }

    public void Dispose() {
        smtpClient.Dispose();
        GC.SuppressFinalize(this);
    }

}