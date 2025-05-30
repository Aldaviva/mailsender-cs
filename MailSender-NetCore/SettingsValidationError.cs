namespace MailSender;

[Serializable]
public class SettingsValidationError: Exception {

    public string settingName { get; }
    public object? invalidValue { get; }

    public SettingsValidationError(string settingName, object? invalidValue, string message): base(message) {
        this.settingName  = settingName;
        this.invalidValue = invalidValue;
    }

}