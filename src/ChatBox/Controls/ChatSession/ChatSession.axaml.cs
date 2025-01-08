using Avalonia.Interactivity;

namespace ChatBox.Controls.ChatSession;

public partial class ChatSession : UserControl
{
    public ChatSession()
    {
        InitializeComponent();
    }

    private ChatViewModel ViewModel => (ChatViewModel)DataContext;

    private void ChatSessionItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is ChatSessionItem { DataContext: SessionsViewModel session })
        {
            ViewModel.Session = session;
        }
    }
}