# mailsender-cs
Send me an email when torrents finish.

## Building
1. Start [Visual Studio 2017 Community Edition](https://www.visualstudio.com/vs/community/) or something.
2. Open `MailSender.sln`.
3. Build solution.

## Configuring
Edit `MailSender.exe.config` and supply values for all the `<setting>` elements.

## Running
### manually
```
MailSender.exe "My Torrent Name"
```
### from µTorrent
1. Options › Preferences
2. Advanced › Run Program
3. Set "Run this program when a torrent finishes" to
    ```
    "C:\Program Files\MailSender\MailSender.exe" "%N"
    ```
    or wherever `MailSender.exe` is.
