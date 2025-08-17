using System.Net;
using System.Net.Http.Json;
using Unfucked;
using Unfucked.HTTP;
using Unfucked.HTTP.Exceptions;

namespace MailSender;

public class TranscoderFarmPoster(HttpClient http, Uri baseUri) {

    private readonly WebTarget target = http.Target(baseUri);

    public async Task postNewJob(string filePath) {
        try {
            await target.Path("jobs").Post<string>(JsonContent.Create(Singleton.Dictionary("filePath", filePath)));
        } catch (ClientErrorException e) when (e.StatusCode == HttpStatusCode.Conflict) { /* Already added, ignore */
        }
    }

}