using Avalonia.Interactivity;
using ChatBox.Internal;

namespace ChatBox.Controls;

public partial class ActionBar : UserControl
{
    public event EventHandler<RoutedEventArgs>? DeleteClick;
    
    public event EventHandler<RoutedEventArgs>? NewSessionClick;
    
    public ActionBar()
    {
        InitializeComponent();
    }

    protected override async void OnInitialized()
    {
        base.OnInitialized();

        await TokenHelper.InitToken();
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        DeleteClick?.Invoke(this, e);
    }

    private void NewSessionButton_Click(object? sender, RoutedEventArgs e)
    {
        NewSessionClick?.Invoke(this, e);
    }
}