using System;

namespace MailSender
{
    class MailSenderMain
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"usage: MailSender.exe ""Torrent name""");
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
                    Console.WriteLine("Invalid settings in file MailSender.exe.config");
                    Console.WriteLine($"Setting name: {e.SettingName}");
                    Console.WriteLine($"Setting value: {e.InvalidValue}");
                    Console.WriteLine(e.Message);
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
