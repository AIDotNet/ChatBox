using System.Collections.Concurrent;
using System.Net.Http;
using AutoGpt;
using ChatBox.Internal.Function;
using Microsoft.SemanticKernel;
using OpenAI.Chat;

#pragma warning disable SKEXP0070

#pragma warning disable SKEXP0010

namespace ChatBox.AI;

public static class KernelFactory
{
    private static readonly ConcurrentDictionary<string, Lazy<Kernel>> Kernels = new();
    private static readonly ConcurrentDictionary<string, Lazy<AutoGpt.AutoGptClient>> Clients = new();

    public static Kernel GetKernel(string apiKey, string model, string type = "OpenAI")
    {
        return Kernels.GetOrAdd(apiKey + model, _ => new Lazy<Kernel>(() =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.Plugins.AddFromType<CommandFunction>();
            kernelBuilder.Plugins.AddFromType<FileFunction>();

            // 开源版本必须使用 TokenAI
            kernelBuilder.AddOpenAIChatCompletion(model, new Uri("https://api.token-ai.cn/v1"), apiKey,
                // orgId 用于区分不同的应用
                httpClient: new HttpClient(new OpenAIHttpClientHandler()), orgId: "chat-box");


            return kernelBuilder.Build();
        })).Value;
    }

    public static AutoGptClient GetClient(string model, string apiKey)
    {
        return Clients.GetOrAdd(model + apiKey, new Lazy<AutoGptClient>(() =>
        {
            var service = new ServiceCollection();
            service.AddOpenAI(options =>
            {
                options.Endpoint = "https://api.token-ai.cn/";
                options.NumOutputs = 6;
            });

            var provider = service.BuildServiceProvider();
            var scope = provider.CreateScope();

            var client = scope.ServiceProvider.GetRequiredService<AutoGptClient>();

            return client;
        })).Value;
    }
}