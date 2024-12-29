namespace ChatBox.ViewModels;

public class ToolViewModel : ViewModelBase
{
    /// <summary>
    /// 原文
    /// </summary>
    /// <returns></returns>
    private string _originalText;
    
    public string OriginalText
    {
        get => _originalText;
        set => SetProperty(ref _originalText, value);
    }
    
    /// <summary>
    /// 翻译
    /// </summary>
    /// <returns></returns>
    private string _translatedText;
    
    public string TranslatedText
    {
        get => _translatedText;
        set => SetProperty(ref _translatedText, value);
    }
    
    /// <summary>
    /// 翻译语种
    /// </summary>
    /// <returns></returns>
    private string _translatedLanguage;
    
    public string TranslatedLanguage
    {
        get => _translatedLanguage;
        set => SetProperty(ref _translatedLanguage, value);
    }
}