using System.Collections.Concurrent;
using System.Net.Http;
using ChatBox.Internal.Function;
using Microsoft.SemanticKernel;
using OpenAI.Chat;

#pragma warning disable SKEXP0070

#pragma warning disable SKEXP0010

namespace ChatBox.AI;

public static class KernelFactory
{
	private static readonly ConcurrentDictionary<string, Lazy<Kernel>> Kernels = new();

	public static Kernel GetKernel(string apiKey, string model, string type = "OpenAI")
	{
		return Kernels.GetOrAdd(apiKey + model , _ => new Lazy<Kernel>(() =>
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
}