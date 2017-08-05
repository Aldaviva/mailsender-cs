using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;

namespace MailSender
{
    class MailSender
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
            var message = new MimeMessage()
            {
                Subject = subject,
                Body = body
            };
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));

            using (var client = new SmtpClient())
            {
                try
                {
                    Console.WriteLine($"Connecting to SMTP server {host}:{port}...");
                    client.Connect(host, port, options);

                    Console.WriteLine($"Logging in as {username}...");
                    client.Authenticate(username, password);

                    Console.WriteLine("Sending message...");
                    client.Send(message);

                    Console.WriteLine("Sent.\nDisconnecting...");
                    client.Disconnect(true);
                    Console.WriteLine("Disconnected.");
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine("Failed to send: " + e.Message);
                    throw e;
                }
            }
        }
    }
}
