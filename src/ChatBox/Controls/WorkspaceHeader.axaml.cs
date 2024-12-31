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

    private double _previousWidth;
    private double _previousHeight;
    private PixelPoint _previousPosition;

    private void DockButton_Click(object? sender, RoutedEventArgs e)
    {
        if (this.GetVisualRoot() is not Window window) return;

        if (ViewModel.IsRight)
        {
            // Restore the window to its previous size and position
            window.Position = _previousPosition;
            window.Width = _previousWidth;
            
            window.Topmost = false;
            window.Activate();
            window.WindowState = WindowState.Normal;
            ViewModel.IsRight = false;
            
            
        }
        else
        {
            // Save the current size and position
            _previousWidth = window.Width;
            _previousHeight = window.Height;
            _previousPosition = window.Position;

            // Dock the window to the right side of the screen
            var screen = window.Screens.ScreenFromVisual(window);
            if (screen != null)
            {
                // 宽度跟随屏幕宽度，计算。如果是 16:9 屏幕，可以直接设置宽度为 屏幕的 1/4 ，如果是 4:3 屏幕，可以设置为 1/3
                if (screen.Bounds.Width / screen.Bounds.Height == 16 / 9)
                {
                    window.Width = (double)((decimal)screen.Bounds.Width / 4); // 1/4 of the screen width
                }
                else
                {
                    window.Width = (double)((decimal)screen.Bounds.Width / 5); // 1/3 of the screen width
                }

                window.Height = screen.Bounds.Height; // Full screen height
                // Grid失效

                window.Position = new PixelPoint((int)(screen.Bounds.Width - window.Width), 0); // Right side
                window.Topmost = true;
            }

            ViewModel.IsRight = true;
        }
    }
}