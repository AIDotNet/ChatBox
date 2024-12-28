using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace ChatBox.Controls;

public partial class WorkspaceHeader : UserControl
{
    private readonly ChatMessageRepository chatMessageRepository;

    public WorkspaceHeader()
    {
        InitializeComponent();

        chatMessageRepository = HostApplication.Services.GetService<ChatMessageRepository>();
    }

    private ChatViewModel ViewModel => (ChatViewModel)DataContext;

    private async void ClearConversation_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.Messages.Clear();

        await chatMessageRepository.DeleteSession("default");
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