using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using ChatBox.AI;

namespace ChatBox.Pages;

public partial class Tool : UserControl
{
    private readonly ChatCompleteService _chatCompleteService;

    public Tool()
    {
        InitializeComponent();

        _chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();
    }

    private async void TranslateButton_Click(object? sender, RoutedEventArgs e)
    {
        await foreach (var item in _chatCompleteService.TranslateContent())
        {
            // do something
        }
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