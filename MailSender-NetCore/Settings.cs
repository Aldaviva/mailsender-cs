using MailSender.Exceptions;

namespace MailSender;

public class Settings {

    public string smtpHost { get; set; } = "localhost";
    public ushort smtpPort { get; set; } = 25;
    public string? smtpUsername { get; set; }
    public string? smtpPassword { get; set; }
    public string fromName { get; set; } = "µTorrent";
    public string fromAddress { get; set; } = "utorrent@localhost";
    public string toName { get; set; } = "Recipient";
    public string toAddress { get; set; } = "recipient@localhost";
    public Uri? transcoderFarmApiBase { get; set; }

    public override string ToString() {
        return
            $"{nameof(smtpHost)}: {smtpHost}, {nameof(smtpPort)}: {smtpPort}, {nameof(smtpUsername)}: {smtpUsername}, {nameof(smtpPassword)}: {smtpPassword}, {nameof(fromName)}: {fromName}, {nameof(fromAddress)}: {fromAddress}, {nameof(toName)}: {toName}, {nameof(toAddress)}: {toAddress}, {nameof(transcoderFarmApiBase)}: {transcoderFarmApiBase}";
    }

    /// <exception cref="SettingsValidationError"></exception>
    public void validate() {
        if (string.IsNullOrWhiteSpace(smtpHost)) {
            throw new SettingsValidationError("smtpHost", smtpHost, "smtpHost must be the hostname of your SMTP server, like mail.server.com");
        }

        if (smtpPort < 1) {
            throw new SettingsValidationError("smtpPort", smtpPort, "smtpPort must be the TCP port of your SMTP server, like 25");
        }

        if (string.IsNullOrWhiteSpace(smtpUsername) && !string.IsNullOrWhiteSpace(smtpPassword)) {
            throw new SettingsValidationError("smtpUsername", smtpUsername,
                "When smtpPassword is set, smtpUsername must be the username you use to log in to your SMTP server, like user@server.com");
        } else if (!string.IsNullOrWhiteSpace(smtpUsername) && string.IsNullOrWhiteSpace(smtpPassword)) {
            throw new SettingsValidationError("smtpPassword", smtpPassword, "When smtpUsername is set, smtpPassword must be the password you use to log in to your SMTP server");
        }

        if (string.IsNullOrWhiteSpace(fromName)) {
            throw new SettingsValidationError("fromName", fromName, "fromName must be the name that will appear in the From: field of the email, such as µTorrent");
        }

        if (string.IsNullOrWhiteSpace(fromAddress) || !fromAddress.Contains('@')) {
            throw new SettingsValidationError("fromAddress", fromAddress,
                "fromAddress must be the email address that will appear in the From: field of the email, such as utorrent@server.com");
        }

        if (string.IsNullOrWhiteSpace(toName)) {
            throw new SettingsValidationError("toName", toName, "toName must be the name that will appear in the To: field of the email, such as Ben");
        }

        if (string.IsNullOrWhiteSpace(toAddress) || !toAddress.Contains('@')) {
            throw new SettingsValidationError("toAddress", toAddress, "toAddress must be the email address that will appear in the To: field of the email, such as ben@server.com");
        }
    }

}