using System.Collections.ObjectModel;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class ToolViewModel : ModelListViewModel
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
    private TranslatedLanguageModel _translatedLanguageModel;

    public TranslatedLanguageModel TranslatedLanguageModel
    {
        get => _translatedLanguageModel;
        set => SetProperty(ref _translatedLanguageModel, value);
    }

    private ObservableCollection<TranslatedLanguageModel> _translatedLanguages = new();

    public ObservableCollection<TranslatedLanguageModel> TranslatedLanguages
    {
        get => _translatedLanguages;
        set => SetProperty(ref _translatedLanguages, value);
    }
    
    private bool _isLoading;
    
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }
}