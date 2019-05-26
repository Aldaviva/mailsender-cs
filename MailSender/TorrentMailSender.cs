using MailKit.Security;
using MailSender.Properties;
using MimeKit;
using MimeKit.Text;

namespace MailSender
{
    public class TorrentMailSender
    {
        public TorrentMailSender()
        {
            ValidateProperties();
        }

        public void SendEmail(string torrentName)
        {
            Settings settings = Settings.Default;

            var mailSender = new MailSender(
                settings.smtpHost,
                settings.smtpPort,
                SecureSocketOptions.StartTls,
                settings.smtpUsername,
                settings.smtpPassword);

            mailSender.SendEmail(
                settings.fromName,
                settings.fromAddress,
                settings.toName,
                settings.toAddress,
                GetSubject(torrentName),
                GetBody(torrentName));
        }

        private static void ValidateProperties()
        {
            Settings settings = Settings.Default;

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

        private static bool HasText(string str)
        {
            return (str?.Trim().Length ?? 0) > 0;
        }

        private static TextPart GetBody(string torrentName)
        {
            return new TextPart(TextFormat.Plain)
            {
                Text = $"Downloaded {torrentName}.\r\nEnjoy!"
            };
        }

        private static string GetSubject(string torrentName)
        {
            return $"Downloaded {torrentName}";
        }
    }
}
