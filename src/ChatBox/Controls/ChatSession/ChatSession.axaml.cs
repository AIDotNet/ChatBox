using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ChatBox.Models;

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
        if (sender is ChatSessionItem chatSessionItem && chatSessionItem.DataContext is Session session)
        {
            ViewModel.Session = session;
        }
    }
}