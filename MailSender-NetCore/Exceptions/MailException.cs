namespace MailSender.Exceptions;

public abstract class MailException(string message, Exception? cause): ApplicationException(message, cause) {

    public string? subject { get; set; }

}

public sealed class ConnectionException(string message, Exception cause): MailException(message, cause);

public sealed class AuthenticationException(string message, Exception cause): MailException(message, cause);

public sealed class SendingException(string message, Exception cause): MailException(message, cause);

public sealed class SettingsValidationError(string settingName, object? invalidValue, string message): MailException(message, null) {

    public string settingName { get; } = settingName;
    public object? invalidValue { get; } = invalidValue;

}