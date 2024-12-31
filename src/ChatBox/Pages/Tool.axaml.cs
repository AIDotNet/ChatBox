using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using ChatBox.AI;
using ChatBox.Models;

namespace ChatBox.Pages;

public partial class Tool : UserControl
{
    private readonly ChatCompleteService _chatCompleteService;

    public Tool()
    {
        InitializeComponent();

        _chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();
    }

    private ToolViewModel ViewModel => (ToolViewModel)DataContext;

    private async void TranslateButton_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.IsLoading = true;
        try
        {
            ViewModel.TranslatedText = string.Empty;
            await foreach (var item in _chatCompleteService.TranslateContent(ViewModel.OriginalText, ViewModel.ModelId.Id,
                               ViewModel.TranslatedLanguageModel.Language))
            {
                ViewModel.TranslatedText += item.Content;
            }
        }
        finally
        {
            ViewModel.IsLoading = false;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "英文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "中文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "日本語",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "韩文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "法文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "德文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "俄文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "西班牙文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "葡萄牙文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "意大利文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "荷兰文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "波兰文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "瑞典文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "丹麦文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "芬兰文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "捷克文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "匈牙利文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "罗马尼亚文",
        });
        ViewModel.TranslatedLanguages.Add(new TranslatedLanguageModel()
        {
            Language = "希腊文",
        });
        
        ViewModel.TranslatedLanguageModel = ViewModel.TranslatedLanguages[0];
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 获取当前窗口并调用 BeginMoveDrag
        var window = this.GetVisualRoot() as Window;
        if (window != null)
        {
            window.BeginMoveDrag(e);
        }
    }
}