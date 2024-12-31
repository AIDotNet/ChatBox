using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ChatBox.Models;

namespace ChatBox.Service;

public class TokenService
{
    public List<ModelDto> LoadModels()
    {
        FileInfo path;
        if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
            path = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Contents", "MonoBundle", "models.json"));
        else
            path = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "models.json"));
        if (!File.Exists(path.FullName))
        {
            return new List<ModelDto>();
        }

        var content = File.ReadAllText(path.FullName);

        var result = JsonSerializer.Deserialize<List<ModelDto>>(content, AppJsonSerialize.SerializerOptions);

        return result;
    }
}