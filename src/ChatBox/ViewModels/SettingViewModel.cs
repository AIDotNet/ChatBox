using System.Collections.ObjectModel;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class SettingViewModel : ViewModelBase
{
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
}