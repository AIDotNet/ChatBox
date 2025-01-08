using System.Linq;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ChatBox.AI;
using ChatBox.Models;

namespace ChatBox.Controls.ChatSession;

public partial class ChatSessionItem : UserControl
{
    public event EventHandler<RoutedEventArgs>? Click;
    private readonly ChatMessageRepository _chatMessageRepository;
    private readonly SessionRepository _sessionRepository;
    private readonly ChatCompleteService _chatCompleteService;
    private WindowNotificationManager _notificationManager;

    public ChatSessionItem()
    {
        InitializeComponent();

        _chatMessageRepository = HostApplication.Services.GetService<ChatMessageRepository>();
        _chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();
        _sessionRepository = HostApplication.Services.GetService<SessionRepository>();

        // 监听IsSelected属性变化

        IsSelectedProperty.Changed.AddClassHandler<ChatSessionItem>((sender, e) =>
        {
            sender.UpdateIsSelected((bool)e.NewValue);
        });

        // 监听ChatViewModel的Session属性变化
        var chatViewModel = HostApplication.Services.GetService<ChatViewModel>();
        chatViewModel.SessionChanged += (sender, args) =>
        {
            if (DataContext is SessionsViewModel session && session.Id == args)
            {
                IsSelected = true;
            }
            else
            {
                IsSelected = false;
            }
        };
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (this.GetVisualRoot() is Window window)
        {
            _notificationManager = new WindowNotificationManager(window)
            {
                Position = NotificationPosition.TopRight,
                MaxItems = 3
            };
        }
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

    private void RenameSession_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SessionsViewModel session)
        {
            _ = Task.Run(async () =>
            {
                // 获取最后一条消息
                var lastMessage = await _chatMessageRepository.GetLastMessageAsync(session.Id);

                if (lastMessage == null)
                {
                    return;
                }

                var newName = await _chatCompleteService.RenameAsync(lastMessage.Content, session.TopicModel);

                if (!string.IsNullOrEmpty(newName))
                {
                    await Dispatcher.UIThread.InvokeAsync(() => session.Name = newName);
                }

                // 保存会话
                await _chatMessageRepository.UpdateSessionAsync(session.Id, session.Name);
            });
        }
    }

    private async void DeleteSession_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SessionsViewModel session)
        {
            // 如果只有一个会话，不允许删除
            var chatViewModel = HostApplication.Services.GetService<ChatViewModel>();
            if (chatViewModel?.Sessions.Count == 1)
            {
                _notificationManager.Show(new Notification("错误", "至少保留一个会话", NotificationType.Error));
                return;
            }

            // 删除会话
            await _chatMessageRepository.DeleteSession(session.Id);

            // 删除会话下的消息
            await _sessionRepository.DeleteAsync(session.Id);

            chatViewModel.Sessions.Remove(session);

            // 如果删除的是当前会话，切换到第一个会话
            if (chatViewModel.Session.Id == session.Id)
            {
                chatViewModel.Session = chatViewModel.Sessions.FirstOrDefault();
            }
        }
    }
}