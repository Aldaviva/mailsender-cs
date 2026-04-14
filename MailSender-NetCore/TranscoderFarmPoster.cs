using System.Collections;
using System.Net;
using System.Net.Http.Json;
using Unfucked.HTTP;
using Unfucked.HTTP.Exceptions;

namespace MailSender;

public sealed class TranscoderFarmPoster(Uri baseUri, HttpClient http) {

    private readonly IWebTarget target = http.Target(baseUri);

    public async Task postNewJob(string filePath) {
        try {
            await target.Path("jobs").Post<string>(JsonContent.Create(IDictionary.Singleton("filePath", filePath)));
        } catch (ClientErrorException e) when (e.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.NotFound) {
            // Already added, or torrent was downloaded and transcoded multiple times and a file extension changed during transcoding (like .mov → .mp4), so ignore
        }
    }

}