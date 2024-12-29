using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChatBox.Models;
using ChatBox.Service;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable SKEXP0110

namespace ChatBox.AI;

public class ChatCompleteService(SettingService settingService, TokenService tokenService)
{
    public ChatMessagePlugin? GetPlugin(string modelId, string name)
    {
        var setting = settingService.LoadSetting();

        var kernel = KernelFactory.GetKernel(setting.ApiKey, modelId, setting.Type);

        var plugin = kernel.Plugins.Where(x => x.Any(a => (a.PluginName + "-" + a.Name).Equals(name)))
            .Select(x => x.FirstOrDefault(a => (a.PluginName + "-" + a.Name).Equals(name)))
            .FirstOrDefault();

        if (plugin == null)
        {
            return null;
        }

        return new ChatMessagePlugin()
        {
            Name = plugin?.Name ?? string.Empty,
            Description = plugin?.Description ?? string.Empty,
            Arguments = string.Empty
        };
    }

    /// <summary>
    /// 翻译内容
    /// </summary>
    /// <returns></returns>
    public async IAsyncEnumerable<StreamingChatMessageContent> TranslateContent(
        string content, string modelId, string targetLanguage)
    {
        var setting = settingService.LoadSetting();
        var model = tokenService.LoadModels().FirstOrDefault(x => x.Id == modelId);

        var kernel = KernelFactory.GetKernel(setting.ApiKey, modelId, setting.Type);

        var chatComplete = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        chatHistory.AddUserMessage(
            @"
是一名精通全世界语言的语言专家，你需要识别用户输入的内容，以国际标准 locale 进行输出，你需要将用户输入的内容翻译成目标语言，目标语言是 {targetLanguage}，请将翻译后的内容输出到聊天框中。
".Replace("{targetLanguage}", targetLanguage));
        chatHistory.AddUserMessage(content);

        await foreach (var item in chatComplete.GetStreamingChatMessageContentsAsync(chatHistory))
        {
            yield return item;
        }
    }

    public async Task ApplyCode(string path, string code)
    {
        var setting = settingService.LoadSetting();

        var kernel = KernelFactory.GetKernel(setting.ApiKey, setting.ModelId, setting.Type);

        var chatComplete = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        chatHistory.AddUserMessage(
            @"
You're a smart programmer, powered by gpt-40. I will give you an existing code file and an optimized code block, you need to insert the optimized code block into the existing code file, you need to write the optimized code block correctly into the correct location of the complete file, please write the modified code file to {path}
Here is the optimized code block
```
{newContent}
```

This is the complete code
```{path}
{content}
```
".Replace("{newContent}", code).Replace("{path}", path).Replace("{content}", await File.ReadAllTextAsync(path)));

        if (setting.Type.Equals("Ollama", StringComparison.OrdinalIgnoreCase))
        {
            var items = await chatComplete.GetChatMessageContentAsync(chatHistory,
                new OpenAIPromptExecutionSettings()
                {
                    MaxTokens = setting.MaxToken,
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                }, kernel);
        }
        else
        {
            await foreach (var item in chatComplete.GetStreamingChatMessageContentsAsync(chatHistory,
                               new OpenAIPromptExecutionSettings()
                               {
                                   ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                               }, kernel))
            {
            }
        }
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetChatComplete(
        IEnumerable<ChatMessageListViewModel> messages,
        string modelId, bool autoCallTool, FileModel[] files)
    {
        var setting = settingService.LoadSetting();
        var model = tokenService.LoadModels().FirstOrDefault(x => x.Id == modelId);

        var kernel = KernelFactory.GetKernel(setting.ApiKey, modelId, setting.Type);

        var chatComplete = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        if (files.Length > 0)
        {
            chatHistory.AddSystemMessage(
                "You are an intelligent programmer, powered by GPT-4o. You are happy to help answer any questions that the user has (usually they will be about coding).\\n\\n1. When the user is asking for edits to their code, please output a simplified version of the code block that highlights the changes necessary and adds comments to indicate where unchanged code has been skipped. For example:\\n```language:path/to/file\\n// ... existing code ...\\n{{ edit_1 }}\\n// ... existing code ...\\n{{ edit_2 }}\\n// ... existing code ...\\n```\\nThe user can see the entire file, so they prefer to only read the updates to the code. Often this will mean that the start/end of the file will be skipped, but that's okay! Rewrite the entire file only if specifically requested. Always provide a brief explanation of the updates, unless the user specifically requests only the code.\\n\\n2. Do not lie or make up facts.\\n\\n3. If a user messages you in a foreign language, please respond in that language.\\n\\n4. Format your response in markdown.\\n\\n5. When writing out new code blocks, please specify the language ID after the initial backticks, like so: \\n```python\\n{{ code }}\\n```\\n\\n6. When writing out code blocks for an existing file, please also specify the file path after the initial backticks and restate the method / class your codeblock belongs to, like so:\\n```language:some/other/file\\nfunction AIChatHistory() {\\n    ...\\n    {{ code }}\\n    ...\\n}\\n``");

            var prompt =
                "# Inputs\n\n## Current File\nHere is the file I'm looking at. It might be truncated from above and below and, if so, is centered around my cursor.\n";

            foreach (var file in files)
            {
                prompt += $"{await GetFileMarkdown(file.FullName)}";
            }

            chatHistory.AddUserMessage(prompt);
        }

        foreach (var message in messages)
        {
            switch (message.Role)
            {
                case "user":
                    chatHistory.AddUserMessage(message.Content);
                    break;
                case "assistant":
                    chatHistory.AddAssistantMessage(message.Content);
                    break;
                case "system":
                    chatHistory.AddSystemMessage(message.Content);
                    break;
                case "tool":
                    chatHistory.AddMessage(AuthorRole.Tool, message.Content);
                    break;
            }
        }

        if (model?.FunctionCall == false && autoCallTool)
        {
            autoCallTool = false;
        }

        await foreach (var item in chatComplete.GetStreamingChatMessageContentsAsync(chatHistory,
                           new OpenAIPromptExecutionSettings()
                           {
                               MaxTokens = setting.MaxToken,
                               ToolCallBehavior = autoCallTool ? ToolCallBehavior.AutoInvokeKernelFunctions : null
                           }, kernel))
        {
            yield return item;
        }
    }


    /// <summary>
    /// 根据文件名后缀名获取文件markdown格式
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetFileMarkdown(string fileName)
    {
        var content = await File.ReadAllTextAsync(fileName, Encoding.UTF8);
        return
            $@"```{fileName}
{content}
```";
    }
}