using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailSender;

internal class MailSender(
    string              host,
    ushort              port,
    SecureSocketOptions options,
    string?             username,
    string?             password
) {

    public async Task sendEmail(string fromName, string fromAddress, string toName, string toAddress, string subject, MimeEntity body) {
        MimeMessage message = new() {
            Subject = subject,
            Body    = body
        };
        message.From.Add(new MailboxAddress(fromName, fromAddress));
        message.To.Add(new MailboxAddress(toName, toAddress));

        using ISmtpClient client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput(), true));

        try {
            Console.WriteLine($"Connecting to SMTP server {host}:{port}...");
            await client.ConnectAsync(host, port, options);
        } catch (Exception e) {
            showError("Failed to connect to SMTP server", e);
            throw;
        }

        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)) {
            try {
                Console.WriteLine($"Logging in as {username}...");
                await client.AuthenticateAsync(new SaslMechanismLogin(username, password));
            } catch (Exception e) {
                showError("Failed to log in to SMTP server", e);
                throw;
            }
        }

        try {
            Console.WriteLine("Sending message...");
            await client.SendAsync(message);
        } catch (Exception e) {
            showError("Failed to send message", e);
            throw;
        }

        Console.WriteLine("Sent.\nDisconnecting...");
        await client.DisconnectAsync(true);
        Console.WriteLine("Disconnected.");
    }

    private static void showError(string message, Exception e) {
        MessageBox.Show($"{e.GetType().Name}: {e.Message}", message, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

}