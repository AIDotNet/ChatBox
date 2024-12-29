using System.Collections.ObjectModel;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class ModelListViewModel : ViewModelBase
{
    private ModelDto _modelId;

    public ModelDto ModelId
    {
        get => _modelId;
        set
        {
            SetProperty(ref _modelId, value);
            ModelIdChanged?.Invoke(value);
        }
    }

    public Action<ModelDto> ModelIdChanged { get; set; }


    private ObservableCollection<ModelDto> _models = new();


    public ObservableCollection<ModelDto> Models
    {
        get => _models;
        set => SetProperty(ref _models, value);
    }
}