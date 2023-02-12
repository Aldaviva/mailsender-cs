using System.Linq;
using System.Windows.Forms;

namespace MailSender;

public static class MailSenderMain {

    public static int Main(string[] args) {
        if (!args.Any()) {
            MessageBox.Show(@"Usage:

MailSender.exe ""Torrent name""", "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return 1;
        }

        try {
            string torrentName = args[0];
            TorrentMailSender.sendEmail(torrentName);
            return 0;
        } catch (SettingsValidationError e) {
            MessageBox.Show($"""
                Invalid settings in file MailSender.exe.config
                
                Setting name: {e.settingName}
                Setting value: {e.invalidValue}
                
                {e.Message}
                """, "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return 1;
        } catch {
            return 1;
        }
    }

}