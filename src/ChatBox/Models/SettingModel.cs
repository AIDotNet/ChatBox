namespace ChatBox.Models;

public class SettingModel : ViewModelBase
{
    /// <summary>
    /// 当前对话模式
    /// OpenAI Ollama
    /// </summary>
    public string Type { get; set; } = "OpenAI";

    public string ApiKey { get; set; }

    public string ModelId { get; set; }

    private int _maxToken = 2048;

    /// <summary>
    /// The maximum number of tokens that can be processed in a single request.
    /// </summary>
    public int MaxToken
    {
        get => _maxToken;
        set
        {
            if (value < 1)
            {
                SetProperty(ref _maxToken, 1);
            }
            else
            {
                SetProperty(ref _maxToken, value);
            }
        }
    }

    public string? Language { get; set; }
}