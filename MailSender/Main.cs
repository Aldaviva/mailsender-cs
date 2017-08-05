using System;
using System.Windows.Forms;

namespace MailSender
{
    class MailSenderMain
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show(@"usage:

MailSender.exe ""Torrent name""", "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return 1;
            }
            else
            {
                try
                {
                    var torrentName = args[0];
                    new TorrentMailSender().SendEmail(torrentName);
                    return 0;
                }
                catch (SettingsValidationError e)
                {
                    MessageBox.Show($@"Invalid settings in file MailSender.exe.config

Setting name: {e.SettingName}
Setting value: {e.InvalidValue}

{e.Message}", "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return 1;
                }
                catch (Exception)
                {
                    return 1;
                }
            }
        }
    }
}
