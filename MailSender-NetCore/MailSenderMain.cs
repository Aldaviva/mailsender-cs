namespace MailSender;

public static class MailSenderMain {

    public static int Main(string[] args) {
        if (!args.Any()) {
            string processName = Path.GetFileName(Environment.ProcessPath ?? "MailSender.exe");
            MessageBox.Show($"""
                Usage:
                
                {processName} "Torrent name" "Body addition (optional)"
                """, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return 1;
        }

        try {
            string  torrentName  = args[0];
            string? bodyAddition = args.ElementAtOrDefault(1);
            TorrentMailSender.sendEmail(torrentName, bodyAddition);
            return 0;
        } catch (SettingsValidationError e) {
            MessageBox.Show($"""
                Invalid settings in file {TorrentMailSender.CONFIG_FILENAME}
                
                Setting name: {e.settingName}
                Setting value: {e.invalidValue}
                
                {e.Message}
                """, "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return 1;
        } catch (Exception e) when (e is not OutOfMemoryException) {
            return 1;
        }
    }

}