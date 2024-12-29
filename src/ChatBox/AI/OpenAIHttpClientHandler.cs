using System.Net;
using System.Net.Http;
using System.Threading;
using ChatBox.Service;

namespace ChatBox.AI;

public class OpenAIHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // 如果返回401，清空ApiKey
            var settings = HostApplication.Services.GetRequiredService<SettingService>().LoadSetting();

            settings.ApiKey = string.Empty;
            HostApplication.Services.GetRequiredService<SettingService>().SaveSetting(settings);

            HostApplication.Logout?.Invoke();
        }

        return response;
    }
}