using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChatBox.Models;

namespace ChatBox;

public class AppJsonSerialize
{
    private static JsonSerializerOptions? _serializerOptions;

    public static JsonSerializerOptions SerializerOptions =>
        _serializerOptions ??= new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolverChain = { AppJsonSerializerContext.Default },
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
}

[JsonSerializable(typeof(SettingModel))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(ChatMessage))]
[JsonSerializable(typeof(ChatMessagePlugin))]
[JsonSerializable(typeof(List<ModelDto>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}