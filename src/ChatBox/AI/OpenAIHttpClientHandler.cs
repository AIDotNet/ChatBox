using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;

namespace ChatBox.AI;

public class OpenAIHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // var json = JsonSerializer.Deserialize<object>(await request.Content.ReadAsStringAsync(cancellationToken));
        //
        // request.Content = new StringContent(JsonSerializer.Serialize(json, new JsonSerializerOptions
        // {
        //     Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        // }), Encoding.UTF8, "application/json");

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}