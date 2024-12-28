using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ChatBox.ViewModels;

namespace ChatBox.Pages;

public partial class Chat : UserControl
{
    public Chat(ChatViewModel model)
    {
        InitializeComponent();

        DataContext = model;
    }
}