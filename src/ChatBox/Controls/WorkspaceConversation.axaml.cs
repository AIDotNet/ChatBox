﻿using System.Linq;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using AvaloniaXmlTranslator;
using ChatBox.AI;
using ChatBox.Models;
using ChatBox.Views;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ChatBox.Controls;

public partial class WorkspaceConversation : UserControl
{
    private readonly ChatCompleteService chatCompleteService;
    private readonly ChatMessageRepository _chatMessageRepository;
    private WindowNotificationManager? _notificationManager;


    public WorkspaceConversation()
    {
        InitializeComponent();

        _chatMessageRepository = HostApplication.Services.GetService<ChatMessageRepository>();
        chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();
    }


    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _notificationManager = HostApplication.Services.GetService<WindowNotificationManager>();
    }

    protected override async void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        var viewModel = DataContext as ChatViewModel;
        if (viewModel?.Messages != null)
        {
            viewModel.OnMessageUpdated += ViewModel_OnMessageUpdated;
        }

        // 监听IsSelected属性变化
        ViewModel.SessionChanged += async (sender, args) => { LoadChatMessage(); };
    }

    private void LoadChatMessage(string? sessionId = null)
    {
        ViewModel.Messages.Clear();
        ViewModel.IsSessionLoading = true;
        sessionId ??= ViewModel.Session.Id;

        _ = Task.Run(async () =>
        {
            foreach (var item in await _chatMessageRepository.GetMessagesAsync(sessionId))
            {
                var message = item;
                Dispatcher.UIThread.Post(() =>
                {
                    ViewModel.Messages.Add(new ChatMessageListViewModel()
                    {
                        Id = message.Id,
                        Content = message.Content,
                        CreatedAt = message.CreatedAt,
                        IsEditing = false,
                        Meta = message.Meta,
                        Plugin = message.Plugin,
                        Role = message.Role,
                        SessionId = message.SessionId,
                        UpdatedAt = message.UpdatedAt,
                    });
                }, DispatcherPriority.Render);
            }

            Dispatcher.UIThread.Post(() =>
            {
                ViewModel.IsSessionLoading = false;
                ScrollViewer.ScrollToEnd();
                
                // 如果是新会话，发送欢迎消息
                ViewModel.WelcomeVisible = ViewModel.Messages.Count == 0;
                
            }, DispatcherPriority.Background);
        });
    }

    private void ViewModel_OnMessageUpdated()
    {
        Dispatcher.UIThread.Post(() => { ScrollViewer.ScrollToEnd(); }, DispatcherPriority.Background);
    }

    private ChatViewModel ViewModel => (ChatViewModel)DataContext;

    private async void ChatRender_OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is ChatRender { DataContext: ChatMessageListViewModel message })
        {
            ViewModel.Messages.Remove(message);

            await _chatMessageRepository.DeleteAsync(message.Id);
        }
    }

    private void ChatRender_OnCopyClicked(object? sender, EventArgs e)
    {
        if (sender is ChatRender { DataContext: ChatMessageListViewModel message })
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;

            clipboard?.SetTextAsync(message.Content);

            _notificationManager?.Show(
                new Notification(
                    I18nManager.Instance.GetResource(Localization.Controls.WorkspaceConversation.CopyNotificationTitle),
                    I18nManager.Instance.GetResource(
                        Localization.Controls.WorkspaceConversation.CopyNotificationMessage),
                    NotificationType.Success));
        }
    }

    private void ChatRender_OnEditClicked(object? sender, EventArgs e)
    {
        if (sender is ChatRender { DataContext: ChatMessageListViewModel message })
        {
            message.IsEditing = true;
        }
    }

    private async void ChatEdit_OnOkClicked(object? sender, EventArgs e)
    {
        if (sender is ChatEdit { DataContext: ChatMessageListViewModel message })
        {
            message.IsEditing = false;

            await _chatMessageRepository.UpdateContent(message.Id, message.Content);
        }
    }

    private void ChatEdit_OnCancelClicked(object? sender, EventArgs e)
    {
        if (sender is ChatEdit { DataContext: ChatMessageListViewModel message })
        {
            message.IsEditing = false;
        }
    }

    private async void ChatRender_OnReGenerateClicked(object? sender, EventArgs e)
    {
        if (sender is ChatRender { DataContext: ChatMessageListViewModel message })
        {
            // 找到这个文件的索引
            var index = ViewModel.Messages.IndexOf(message);

            // 找到从这个索引开始的下面所有的对象
            var nextMessages = ViewModel.Messages.ToArray().Skip(index + 1).ToList();

            // 删除这个索引开始的下面所有的对象
            foreach (var nextMessage in nextMessages)
            {
                ViewModel.Messages.Remove(nextMessage);
                await _chatMessageRepository.DeleteAsync(nextMessage.Id);
            }

            ViewModel.Messages.Remove(message);
            await _chatMessageRepository.DeleteAsync(message.Id);

            ViewModel.IsGenerating = true;

            try
            {
                ViewModel.Message = string.Empty;

                var newMessage = ViewModel.Messages.ToList();

                var bot = new ChatMessage
                {
                    Role = AuthorRole.Assistant.ToString(),
                    CreatedAt = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    SessionId = ViewModel.Session.Id
                };
                bot.Meta.Add("avatar", "https://avatars.githubusercontent.com/u/10251060?v=4");
                bot.Meta.Add("name", "Assistant");
                bot.Content = string.Empty;

                var botView = new ChatMessageListViewModel()
                {
                    Id = bot.Id,
                    Content = bot.Content,
                    CreatedAt = bot.CreatedAt,
                    IsEditing = false,
                    Meta = bot.Meta,
                    Plugin = bot.Plugin,
                    Role = bot.Role,
                    SessionId = bot.SessionId,
                    UpdatedAt = bot.UpdatedAt,
                };

                ViewModel.Messages.Add(botView);

                ViewModel.OnMessageUpdated?.Invoke();
                var isfirst = true;
                var autoCallTool = false;
                var isInference = false;
                if (ViewModel.CurrentModel.Key.Equals("Chat", StringComparison.OrdinalIgnoreCase))
                {
                    autoCallTool = false;
                }
                else if (ViewModel.CurrentModel.Key.Equals("Agent", StringComparison.OrdinalIgnoreCase))
                {
                    autoCallTool = true;
                }
                else if (ViewModel.CurrentModel.Key.Equals("inference", StringComparison.OrdinalIgnoreCase))
                {
                    isInference = true;
                }


                var model = ViewModel.ModelId.Id;
                var files = ViewModel.Files.ToArray();

                _ = Task.Run(async () =>
                {
                    int token = 0;

                    try
                    {
                        await foreach (var item in chatCompleteService.GetChatComplete(newMessage, model,
                                           autoCallTool, files, isInference))
                        {
                            if (isfirst)
                            {
                                Dispatcher.UIThread.Post(() =>
                                {
                                    bot.Content = string.Empty;
                                    botView.Content = string.Empty;
                                }, DispatcherPriority.Background);
                            }

                            foreach (var x in item.Items)
                            {
                                if (x is StreamingFunctionCallUpdateContent callUpdateContent)
                                {
                                    var plugin = chatCompleteService.GetPlugin(model, callUpdateContent.Name);
                                    if (plugin != null)
                                    {
                                        bot.Plugin = plugin;
                                        bot.Plugin.Arguments = callUpdateContent.Arguments ?? string.Empty;
                                        Dispatcher.UIThread.Invoke(() =>
                                        {
                                            botView.Plugin = new()
                                            {
                                                Name = bot.Plugin.Name,
                                                Description = bot.Plugin.Description,
                                                Arguments = bot.Plugin.Arguments
                                            };
                                        });
                                    }
                                    else if (plugin == null && bot.Plugin != null)
                                    {
                                        bot.Plugin.Arguments += callUpdateContent.Arguments ?? string.Empty;

                                        Dispatcher.UIThread.Invoke(() =>
                                        {
                                            botView.Plugin = new()
                                            {
                                                Name = bot.Plugin.Name,
                                                Description = bot.Plugin.Description,
                                                Arguments = bot.Plugin.Arguments
                                            };
                                        });
                                    }
                                }
                            }

                            bot.Content += item.Content;

                            token++;
                            if (token == 5 || isfirst)
                            {
                                Dispatcher.UIThread.Post(() => { botView.Content = bot.Content; },
                                    DispatcherPriority.Background);
                                isfirst = false;
                            }
                            else if (token == 10)
                            {
                                token = 0;
                                Dispatcher.UIThread.Post(() =>
                                {
                                    botView.Content = bot.Content;
                                    ViewModel.OnMessageUpdated?.Invoke();
                                }, DispatcherPriority.Background);
                            }
                        }
                    }
                    finally
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            botView.Content = bot.Content;
                            ViewModel.OnMessageUpdated?.Invoke();
                            ViewModel.IsGenerating = false;
                        }, DispatcherPriority.Background);

                        await _chatMessageRepository.InsertAsync(bot);
                    }
                });
            }
            catch (Exception exception)
            {
                _notificationManager?.Show(
                    new Notification(
                        I18nManager.Instance.GetResource(Localization.Controls.WorkspaceConversation
                            .ErrorNotificationTitle), exception.Message, NotificationType.Error));
            }
        }
    }
}