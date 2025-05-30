namespace MailSender.Exceptions;

public abstract class MailException(string message, Exception? cause): ApplicationException(message, cause);

public class ConnectionException(string message, Exception cause): MailException(message, cause);

public class AuthenticationException(string message, Exception cause): MailException(message, cause);

public class SendingException(string message, Exception cause): MailException(message, cause);

public class SettingsValidationError(string settingName, object? invalidValue, string message): MailException(message, null) {

    public string settingName { get; } = settingName;
    public object? invalidValue { get; } = invalidValue;

}