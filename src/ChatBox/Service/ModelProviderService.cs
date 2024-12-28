using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ChatBox.Models;

namespace ChatBox.Service;

public class ModelProviderService
{
    public List<ModelProvider> LoadModelProviders()
    {
        var modelProvidersPath = new List<ModelProvider>();
        
        modelProvidersPath.Add(new ModelProvider
        {
            Id = "OpenAI",
            Name = "OpenAI",
            Icon = "OpenAI"
        });
        
        modelProvidersPath.Add(new ModelProvider
        {
            Id = "Ollama",
            Name = "Ollama",
            Icon = "Ollama"
        });
        
        return modelProvidersPath;
    }
}