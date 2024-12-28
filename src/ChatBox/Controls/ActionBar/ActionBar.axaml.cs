using Avalonia.Interactivity;
using ChatBox.Internal;

namespace ChatBox.Controls;

public partial class ActionBar : UserControl
{
    public event EventHandler<RoutedEventArgs>? DeleteClick;
    
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
}