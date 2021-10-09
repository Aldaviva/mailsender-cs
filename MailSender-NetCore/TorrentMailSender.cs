using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace MailSender {

    public class TorrentMailSender {

        /// <exception cref="SettingsValidationError"></exception>
        public void sendEmail(string torrentName) {
            Settings settings = new ConfigurationBuilder().AddJsonFile("settings.json").Build().Get<Settings>();

            validateSettings(settings);

            MailSender mailSender = new(settings.smtpHost, settings.smtpPort, SecureSocketOptions.StartTls, settings.smtpUsername, settings.smtpPassword);

            mailSender.sendEmail(settings.fromName, settings.fromAddress, settings.toName, settings.toAddress, getSubject(torrentName), getBody(torrentName));
        }

        private static TextPart getBody(string torrentName) {
            return new(TextFormat.Plain) {
                Text = $"Downloaded {torrentName}.\r\n\r\nEnjoy!"
            };
        }

        private static string getSubject(string torrentName) {
            return $"Downloaded {torrentName}";
        }

        /// <exception cref="SettingsValidationError"></exception>
        private static void validateSettings(Settings settings) {
            if (string.IsNullOrWhiteSpace(settings.smtpHost)) {
                throw new SettingsValidationError("smtpHost", settings.smtpHost, "smtpHost must be the hostname of your SMTP server, like mail.server.com");
            }

            if (settings.smtpPort < 1) {
                throw new SettingsValidationError("smtpPort", settings.smtpPort, "smtpPort must be the TCP port of your SMTP server, like 25");
            }

            if (string.IsNullOrWhiteSpace(settings.smtpUsername) && !string.IsNullOrWhiteSpace(settings.smtpPassword)) {
                throw new SettingsValidationError("smtpUsername", settings.smtpUsername,
                    "When smtpPassword is set, smtpUsername must be the username you use to log in to your SMTP server, like user@server.com");
            } else if (!string.IsNullOrWhiteSpace(settings.smtpUsername) && string.IsNullOrWhiteSpace(settings.smtpPassword)) {
                throw new SettingsValidationError("smtpPassword", settings.smtpPassword, "When smtpUsername is set, smtpPassword must be the password you use to log in to your SMTP server");
            }

            if (string.IsNullOrWhiteSpace(settings.fromName)) {
                throw new SettingsValidationError("fromName", settings.fromName, "fromName must be the name that will appear in the From: field of the email, such as µTorrent");
            }

            if (string.IsNullOrWhiteSpace(settings.fromAddress) || !settings.fromAddress.Contains('@')) {
                throw new SettingsValidationError("fromAddress", settings.fromAddress,
                    "fromAddress must be the email address that will appear in the From: field of the email, such as utorrent@server.com");
            }

            if (string.IsNullOrWhiteSpace(settings.toName)) {
                throw new SettingsValidationError("toName", settings.toName, "toName must be the name that will appear in the To: field of the email, such as Ben");
            }

            if (string.IsNullOrWhiteSpace(settings.toAddress) || !settings.toAddress.Contains('@')) {
                throw new SettingsValidationError("toAddress", settings.toAddress, "toAddress must be the email address that will appear in the To: field of the email, such as ben@server.com");
            }
        }

    }

}