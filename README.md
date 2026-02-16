<img src="https://github.com/Aldaviva/mailsender-cs/raw/refs/heads/master/MailSender-NetCore/envelope.ico" width="32" height="32" alt="envelope"> MailSender
===
Send me an email when torrents finish.

## Prerequisites
- Windows
- [.NET 10 Desktop Runtime or later](https://dotnet.microsoft.com/en-us/download)
- Access to an SMTP server

## Installation
1. Download [**`MailSender.exe`**](https://github.com/Aldaviva/mailsender-cs/releases/latest/download/MailSender.exe) from the [latest release](https://github.com/Aldaviva/mailsender-cs/releases).
1. Save it to a directory, such as `C:\Program Files\MailSender\`.

## Configuring
1. Download the [example settings file](https://github.com/Aldaviva/mailsender-cs/blob/master/MailSender-NetCore/settings.example.json) and save it as `settings.json` in the same directory as `MailSender.exe`.
2. Fill in values for all the properties.

## Running
### Manually
```
MailSender.exe --name "My Torrent Name"
```
### From qBittorrent
1. Tools › Options › Downloads › Run external program
1. Set "Run on torrent finished" to
    ```
    "C:\Program Files\MailSender\MailSender.exe" --name "%N" --tags "%G" --info-hash "%K"
    ```
    or wherever `MailSender.exe` is.
1. Uncheck "Show console window"

## Building
1. Start [Visual Studio Community Edition](https://www.visualstudio.com/vs/community/) 2022 or later.
2. Open `MailSender.sln`.
3. Build solution.