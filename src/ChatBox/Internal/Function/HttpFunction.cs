using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.SemanticKernel;

namespace ChatBox.Internal.Function;

/// <summary>
/// HTTP 函数
/// </summary>
public class HttpFunction
{
    private static readonly HttpClient Client = new();

    /// <summary>
    /// 发起 GET 请求并返回响应结果
    /// </summary>
    /// <param name="url">请求的 URL</param>
    /// <returns>响应的结果</returns>
    [KernelFunction, Description("Execute a GET request to the specified URL and return the response as a string.")]
    public async Task<string> Get([Description("Request URL")] string url)
    {
        var response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起带有请求头的 GET 请求并返回响应结果
    /// </summary>
    /// <param name="url">请求的 URL</param>
    /// <param name="headers">请求头</param>
    /// <returns>响应的结果</returns>
    [KernelFunction, Description("Execute a GET request with headers and return the response.")]
    public async Task<string> GetSetHeader([Description("Request URL")] string url,
        [Description("Request headers")] HttpRequestHeaders headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起 POST 请求并返回响应结果
    /// </summary>
    /// <param name="url">请求的 URL</param>
    /// <param name="content">请求的内容</param>
    /// <returns>响应的结果</returns>
    [KernelFunction,
     Description("Execute a POST request to the specified URL with the given content and return the response.")]
    public async Task<string> Post([Description("Request URL")] string url,
        [Description("POST content")] string content)
    {
        var response = await Client.PostAsync(url, new StringContent(content, null, "application/json"));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起带有请求头的 POST 请求并返回响应结果
    /// </summary>
    /// <param name="url">请求的 URL</param>
    /// <param name="content">请求的内容</param>
    /// <param name="headers">请求头</param>
    /// <returns>响应的结果</returns>
    [KernelFunction, Description("Execute a POST request with headers and return the response.")]
    public async Task<string> PostSetHeader([Description("Request URL")] string url,
        [Description("POST content")] string content, [Description("Request headers")] HttpRequestHeaders headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Content = new StringContent(content, null, "application/json");

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起 PUT 请求并返回响应结果
    /// </summary>
    /// <param name="url">请求的 URL</param>
    /// <param name="content">请求的内容</param>
    /// <returns>响应的结果</returns>
    [KernelFunction,
     Description("Execute a PUT request to the specified URL with the given content and return the response.")]
    public async Task<string> Put([Description("Request URL")] string url,
        [Description("PUT content")] string content)
    {
        var response = await Client.PutAsync(url, new StringContent(content, null, "application/json"));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起 DELETE 请求并返回响应结果
    /// </summary>
    /// <param name="url">请求的 URL</param>
    /// <returns>响应的结果</returns>
    [KernelFunction, Description("Execute a DELETE request to the specified URL and return the response.")]
    public async Task<string> Delete([Description("Request URL")] string url)
    {
        var response = await Client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}