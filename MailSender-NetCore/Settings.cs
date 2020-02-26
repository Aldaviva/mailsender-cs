namespace MailSender {

    public class Settings {

        public string smtpHost { get; set; } = "localhost";
        public ushort smtpPort { get; set; } = 25;
        public string? smtpUsername { get; set; }
        public string? smtpPassword { get; set; }
        public string fromName { get; set; } = "µTorrent";
        public string fromAddress { get; set; } = "utorrent@localhost";
        public string toName { get; set; } = "Recipient";
        public string toAddress { get; set; } = "recipient@localhost";

        public override string ToString() {
            return $"{nameof(smtpHost)}: {smtpHost}, {nameof(smtpPort)}: {smtpPort}, {nameof(smtpUsername)}: {smtpUsername}, {nameof(smtpPassword)}: {smtpPassword}, {nameof(fromName)}: {fromName}, {nameof(fromAddress)}: {fromAddress}, {nameof(toName)}: {toName}, {nameof(toAddress)}: {toAddress}";
        }

    }

}