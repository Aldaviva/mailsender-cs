using MailSender;
using MailSender.Exceptions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using qBittorrent.Client;
using qBittorrent.Client.Data;
using System.Collections.Frozen;
using Unfucked.HTTP;

const string CONFIG_FILENAME = "settings.json";

CommandLineApplication app = new();
app.Conventions.UseDefaultConventions();
app.VersionOptionFromAssemblyAttributes(typeof(Program).Assembly);
app.Description      = "Send email after a torrent finishes downloading.";
app.ExtendedHelpText = $"\nExample: {app.Name} --name \"Torrent name\" --info-hash \"e403e61e6c8cdd2ed8333145d500e048d4d60107\" --tags \"Tag1,Tag2,Tag3\"";

CommandOption<string> torrentName = app.Option<string>("-n|--name", "Name of the torrent (%N in qBittorrent, required)", CommandOptionType.SingleValue).IsRequired();
CommandOption<string> infoHash    = app.Option<string>("-i|--info-hash", "Torrent ID (Info Hash v1 or v2, %K in qBittorrent, optional)", CommandOptionType.SingleValue);
CommandOption<string> tags        = app.Option<string>("-t|--tags", "Comma-delimited list of torrent tags (%G in qBittorrent, optional)", CommandOptionType.SingleValue);

bool exit = true;
app.OnExecute(() => exit = false);
app.OnValidationError(result => MessageBox.Show(result.ErrorMessage, $"{app.Name} error", MessageBoxButtons.OK, MessageBoxIcon.Error));
app.Execute(args);
if (app.OptionHelp!.HasValue()) {
    MessageBox.Show(app.GetHelpText(), $"{app.Name} help", MessageBoxButtons.OK, MessageBoxIcon.Information);
}
if (exit) return 1;

using HttpClient        http        = new UnfuckedHttpClient(new HttpClientHandler { UseDefaultCredentials = true });
using qBittorrentClient qBittorrent = new qBittorrentApiClient(new qBittorrentHttpTransport { httpClient   = http });

FrozenSet<string> videoFileExtensions = FrozenSet.Create(".avi", ".mov", ".mpg", ".mp4", ".wmv", ".flv", ".mkv", ".mpeg", ".asf", ".m4v", ".webm", ".ts", ".mp4v", ".3gpp", ".bik", ".divx", ".dv",
    ".f4v", ".m1v", ".vob");

try {
    Settings settings = new ConfigurationBuilder().AddJsonFile(CONFIG_FILENAME).Build().Get<Settings>()!;
    settings.validate();
    using TorrentMailSender torrentMailSender = new(settings);

    ISet<string> tagsSet = tags.Value()?.Split(',').Select(static tag => tag.ToLowerInvariant()).ToHashSet() ?? [];
    if (settings.transcoderFarmApiBase != null && infoHash.Value() is {} torrentId && tagsSet.Contains("transcode")) {
        TranscoderFarmPoster poster = new(settings.transcoderFarmApiBase, http);

        TorrentInfo                torrent        = (await qBittorrent.getTorrent(torrentId))!;
        IReadOnlyList<TorrentFile> filesInTorrent = await qBittorrent.listFilesInTorrent(torrent);
        if (filesInTorrent.Count > 1) {
            IEnumerable<string> downloadedVideoFiles = from downloadedFile in filesInTorrent
                where downloadedFile is { progress: 1, priority: not TorrentFile.FilePriority.DO_NOT_DOWNLOAD }
                where videoFileExtensions.Contains(Path.GetExtension(downloadedFile.filePathRelativeToSavePath).ToLowerInvariant())
                select downloadedFile.filePathAbsolute;

            foreach (string downloadedVideoFile in downloadedVideoFiles) {
                await poster.postNewJob(downloadedVideoFile);
            }
        } else {
            await poster.postNewJob(Path.GetFullPath(torrent.contentPath));
        }
    }

    while (true) {
        try {
            await torrentMailSender.sendEmail(torrentName.ParsedValue, tags.ParsedValue);
            return 0;
        } catch (MailException e) {
            DialogResult dialogResult = MessageBox.Show($"Subject: {e.subject}\nError: {e.GetType().Name}: {e.Message}", "Error while sending email", MessageBoxButtons.RetryCancel,
                MessageBoxIcon.Error);
            if (dialogResult != DialogResult.Retry) {
                return 1;
            }
        }
    }
} catch (SettingsValidationError e) {
    MessageBox.Show($"""
        Invalid settings in file {Path.GetFullPath(CONFIG_FILENAME)}

        Setting name: {e.settingName}
        Setting value: {e.invalidValue}

        {e.Message}
        """, "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
    return 1;
} catch (Exception e) when (e is not OutOfMemoryException) {
    MessageBox.Show($"{e.Message}\r\n\r\n{e.StackTrace}", "Unhandled exception while sending email", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return 1;
}