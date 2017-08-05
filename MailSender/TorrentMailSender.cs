using MailKit.Security;
using MimeKit;

namespace MailSender
{
    class TorrentMailSender
    {
        public TorrentMailSender()
        {
            ValidateProperties();
        }

        public void SendEmail(string torrentName)
        {
            Properties.Settings settings = Properties.Settings.Default;

            var mailsender = new MailSender(
                host: settings.smtpHost,
                port: settings.smtpPort,
                options: SecureSocketOptions.StartTls,
                username: settings.smtpUsername,
                password: settings.smtpPassword);

            mailsender.SendEmail(
                fromName: settings.fromName,
                fromAddress: settings.fromAddress,
                toName: settings.toName,
                toAddress: settings.toAddress,
                subject: GetSubject(torrentName),
                body: GetBody(torrentName));
        }

        private void ValidateProperties()
        {
            Properties.Settings settings = Properties.Settings.Default;

            if (!HasText(settings.smtpHost))
            {
                throw new SettingsValidationError("smtpHost", settings.smtpHost, "smtpHost must be the hostname of your SMTP server, like mail.server.com");
            }

            if (settings.smtpPort < 1)
            {
                throw new SettingsValidationError("smtpPort", settings.smtpPort, "smtpPort must be the TCP port of your SMTP server, like 25");
            }

            if (!HasText(settings.smtpUsername))
            {
                throw new SettingsValidationError("smtpUsername", settings.smtpUsername, "smtpUsername must be the username you use to log in to your SMTP server, like user@server.com");
            }

            if (!HasText(settings.smtpPassword))
            {
                throw new SettingsValidationError("smtpPassword", settings.smtpPassword, "smtpPassword must be the password you use to log in to your SMTP server");
            }

            if (!HasText(settings.fromName))
            {
                throw new SettingsValidationError("fromName", settings.fromName, "fromName must be the name that will appear in the From: field of the email, such as µTorrent");
            }

            if (!HasText(settings.fromAddress) || !settings.fromAddress.Contains("@"))
            {
                throw new SettingsValidationError("fromAddress", settings.fromAddress, "fromAddress must be the email address that will appear in the From: field of the email, such as utorrent@server.com");
            }

            if (!HasText(settings.toName))
            {
                throw new SettingsValidationError("toName", settings.toName, "toName must be the name that will appear in the To: field of the email, such as Ben");
            }

            if (!HasText(settings.toAddress) || !settings.toAddress.Contains("@"))
            {
                throw new SettingsValidationError("toAddress", settings.toAddress, "toAddress must be the email address that will appear in the To: field of the email, such as ben@server.com");
            }
        }

        private bool HasText(string str)
        {
            return (str?.Trim().Length ?? 0) > 0;
        }

        private TextPart GetBody(string torrentName)
        {
            return new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"Downloaded {torrentName}.\r\nEnjoy!"
            };
        }

        private string GetSubject(string torrentName)
        {
            return $"Downloaded {torrentName}";
        }
    }
}
