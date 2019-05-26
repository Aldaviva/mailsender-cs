using System;
using System.Windows.Forms;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailSender
{
    internal class MailSender
    {
        private readonly string host;
        private readonly int port;
        private readonly SecureSocketOptions options;
        private readonly string username;
        private readonly string password;

        public MailSender(string host, int port, SecureSocketOptions options, string username, string password)
        {
            this.host = host;
            this.port = port;
            this.options = options;
            this.username = username;
            this.password = password;
        }

        public void SendEmail(string fromName, string fromAddress, string toName, string toAddress, string subject, MimeEntity body)
        {
            var message = new MimeMessage
            {
                Subject = subject,
                Body = body
            };
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));

            using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput(), true)))
            {
                try
                {
                    Console.WriteLine($"Connecting to SMTP server {host}:{port}...");
                    client.Connect(host, port, options);
                }
                catch (Exception e)
                {
                    ShowError("Failed to connect to SMTP server", e);
                    throw;
                }

                try
                {
                    Console.WriteLine($"Logging in as {username}...");
                    client.Authenticate(username, password);
                }
                catch (Exception e)
                {
                    ShowError("Failed to log in to SMTP server", e);
                    throw;
                }

                try
                {
                    Console.WriteLine("Sending message...");
                    client.Send(message);
                }
                catch (Exception e)
                {
                    ShowError("Failed to send message", e);
                    throw;
                }

                Console.WriteLine("Sent.\nDisconnecting...");
                client.Disconnect(true);
                Console.WriteLine("Disconnected.");
            }
        }

        private static void ShowError(string message, Exception e)
        {
            MessageBox.Show($"{e.GetType().Name}: {e.Message}", message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}