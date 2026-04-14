using MailKit.Security;
using MailSender.Exceptions;
using MimeKit;
using MimeKit.Text;
using ThrottleDebounce.Retry;

namespace MailSender;

public sealed class TorrentMailSender(Settings settings): IDisposable {

    private readonly MailSender mailSender = new(settings.smtpHost, settings.smtpPort, SecureSocketOptions.StartTls, settings.smtpUsername, settings.smtpPassword);

    /// <exception cref="SettingsValidationError"></exception>
    public async Task sendEmail(string torrentName, string? bodyAddition) {
        string subject = getSubject(torrentName);
        try {
            await Retrier.Attempt(async _ => await mailSender.sendEmail(settings.fromName, settings.fromAddress, settings.toName, settings.toAddress, subject, getBody(torrentName, bodyAddition)),
                new RetryOptions { MaxAttempts = 16, Delay = Delays.Linear(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)) });
        } catch (MailException e) {
            e.subject = subject;
            throw;
        }
    }

    private static TextPart getBody(string torrentName, string? bodyAddition) => new(TextFormat.Plain) {
        Text = $"""
            Downloaded {torrentName}.

            {bodyAddition.EmptyToNull ?? "No tags"}

            Enjoy!
            """
    };

    private static string getSubject(string torrentName) =>
        $"Downloaded {torrentName}";

    public void Dispose() =>
        mailSender.Dispose();

}