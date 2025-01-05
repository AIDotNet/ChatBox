using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using ChatBox.AI;
using ChatBox.Models;

namespace ChatBox.Controls.ChatSession;

public partial class ChatSessionItem : UserControl
{
    public event EventHandler<RoutedEventArgs>? Click;
    private readonly ChatMessageRepository _chatMessageRepository;
    private readonly ChatCompleteService _chatCompleteService;

    public ChatSessionItem()
    {
        InitializeComponent();

        _chatMessageRepository = HostApplication.Services.GetService<ChatMessageRepository>();
        _chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();

        // 监听IsSelected属性变化

        IsSelectedProperty.Changed.AddClassHandler<ChatSessionItem>((sender, e) =>
        {
            sender.UpdateIsSelected((bool)e.NewValue);
        });

        // 监听ChatViewModel的Session属性变化
        var chatViewModel = HostApplication.Services.GetService<ChatViewModel>();
        chatViewModel.SessionChanged += (sender, args) =>
        {
            if (DataContext is Session session && session.Id == args)
            {
                IsSelected = true;
            }
            else
            {
                IsSelected = false;
            }
        };
    }

    /// <summary>
    /// 是否选中
    /// </summary>
    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<ChatSessionItem, bool>(nameof(IsSelected));

    public void UpdateIsSelected(bool isSelected)
    {
        IsSelected = isSelected;

        if (isSelected)
        {
            this.FindControl<Border>("Border").Classes.Add("isSelected");
        }
        else
        {
            this.FindControl<Border>("Border").Classes.Remove("isSelected");
        }
    }

    private void Border_OnTapped(object? sender, TappedEventArgs e)
    {
        Click?.Invoke(this, e);
    }

    private async void RenameSession_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is Session session)
        {
            // 获取最后一条消息
            var lastMessage = await _chatMessageRepository.GetLastMessageAsync(session.Id);
            
            if(lastMessage == null)
            {
                return;
            }

            var newName = await _chatCompleteService.RenameAsync(lastMessage.Content, session.TopicModel);
            
            if (!string.IsNullOrEmpty(newName))
            {
                session.Name = newName;
            }
            
            // 保存会话
            await _chatMessageRepository.UpdateSessionAsync(session);
        }
    }
}