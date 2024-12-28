using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChatBox.Controls;

public partial class Avatar : UserControl
{
    public static readonly StyledProperty<string> SourceProperty =
        AvaloniaProperty.Register<Avatar, string>(nameof(Source));

    public string Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public Avatar()
    {
        InitializeComponent();
        DataContext = this;
    }
}