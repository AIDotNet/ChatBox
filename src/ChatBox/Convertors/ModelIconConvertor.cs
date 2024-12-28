using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ChatBox.Convertors;

public class ModelIconConvertor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        if (value is string modelId)
        {
            if (modelId.Equals("OpenAI", StringComparison.OrdinalIgnoreCase))
            {
                var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/openai.png"));

                return new Bitmap(favicon.stream);
            }

            if (modelId.Equals("google", StringComparison.OrdinalIgnoreCase))
            {
                var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/google.png"));

                return new Bitmap(favicon.stream);
            }

            if (modelId.Equals("qwen", StringComparison.OrdinalIgnoreCase))
            {
                var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/qwen.png"));

                return new Bitmap(favicon.stream);
            }

            if (modelId.Equals("mistral", StringComparison.OrdinalIgnoreCase))
            {
                var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/mistral.png"));

                return new Bitmap(favicon.stream);
            }

            if (modelId.Equals("ollama", StringComparison.OrdinalIgnoreCase))
            {
                var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/ollama.png"));

                return new Bitmap(favicon.stream);
            }

            if (modelId.Equals("deepseek", StringComparison.OrdinalIgnoreCase))
            {
                var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/deepseek.png"));

                return new Bitmap(favicon.stream);
            }
        }

        var stream = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/openai.png"));

        return new Bitmap(stream.stream);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}