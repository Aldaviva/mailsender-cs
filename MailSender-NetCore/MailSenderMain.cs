using MailSender;

if (args.Length == 0) {
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
    await TorrentMailSender.sendEmail(torrentName, bodyAddition);
    return 0;
} catch (SettingsValidationError e) {
    MessageBox.Show($"""
                     Invalid settings in file {Path.GetFullPath(TorrentMailSender.CONFIG_FILENAME)}

                     Setting name: {e.settingName}
                     Setting value: {e.invalidValue}

                     {e.Message}
                     """, "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
    return 1;
} catch (Exception e) when (e is not OutOfMemoryException) {
    MessageBox.Show($"{e.Message}\r\n\r\n{e.StackTrace}", "Unhandled exception while sending email", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return 1;
}