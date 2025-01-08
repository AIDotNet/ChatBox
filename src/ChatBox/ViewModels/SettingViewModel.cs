using System.Collections.ObjectModel;
using AvaloniaXmlTranslator;
using System.Linq;
using System.Threading;
using AvaloniaXmlTranslator.Models;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class SettingViewModel : ViewModelBase
{
    public SettingViewModel()
    {
        InitLanguage();
    }

    private SettingModel setting;

    public SettingModel Setting
    {
        get => setting;
        set => SetProperty(ref setting, value);
    }

    private ModelProvider _selectedModelProvider;

    public ModelProvider SelectedModelProvider
    {
        get => _selectedModelProvider;
        set => SetProperty(ref _selectedModelProvider, value);
    }

    private ObservableCollection<ModelProvider> _modelProvider = new();

    public ObservableCollection<ModelProvider> ModelProvider
    {
        get => _modelProvider;
        set => SetProperty(ref _modelProvider, value);
    }
    
    /// <summary>
    /// 是否存在开机启动
    /// </summary>
    private bool _isAutoStart;
    
    public bool IsAutoStart
    {
        get => _isAutoStart;
        set => SetProperty(ref _isAutoStart, value);
    }

    public ObservableCollection<LocalizationLanguage> Languages { get; private set; }

    private LocalizationLanguage? _selectedLanguage;

    public LocalizationLanguage? SelectedLanguage
    {
        get => _selectedLanguage;
        set => SetProperty(ref _selectedLanguage, value);
    }


    private void InitLanguage()
    {
        var languages = I18nManager.Instance.GetLanguages();
        Languages = new ObservableCollection<LocalizationLanguage>(languages);

        var language = string.Empty;
        if (string.IsNullOrWhiteSpace(Setting?.Language))
        {
            language = Thread.CurrentThread.CurrentCulture.Name;
        }

        if (string.IsNullOrWhiteSpace(language)
            || !I18nManager.Instance.Resources.ContainsKey(language))
        {
            language = "zh-CN";
        }
        _selectedLanguage = Languages.FirstOrDefault(l => l.CultureName == language);
    }
}