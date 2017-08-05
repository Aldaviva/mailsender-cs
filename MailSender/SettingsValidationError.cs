using System;

namespace MailSender
{
    [Serializable]
    internal class SettingsValidationError : Exception
    {
        public string SettingName { get; }
        public object InvalidValue { get; }

        public SettingsValidationError(string settingName, object invalidValue, string message) : base(message)
        {
            SettingName = settingName;
            InvalidValue = invalidValue;
        }
    }
}