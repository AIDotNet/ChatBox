using Avalonia.Interactivity;

namespace ChatBox.Controls;

public partial class ChatEdit : UserControl
{
    public event EventHandler OkClicked;

    public event EventHandler CancelClicked;

    public ChatEdit()
    {
        InitializeComponent();
    }

    private void OkClick(object? sender, RoutedEventArgs e)
    {
        OkClicked?.Invoke(this, e);
    }

    private void CancelClick(object? sender, RoutedEventArgs e)
    {
        CancelClicked?.Invoke(this, e);
    }
}