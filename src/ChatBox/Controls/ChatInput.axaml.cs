using System.Linq;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ChatBox.AI;
using ChatBox.Models;
using ChatBox.Service;
using ChatBox.Views;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ChatBox.Controls;

public partial class ChatInput : UserControl
{
    private readonly ChatCompleteService chatCompleteService;
    private readonly ChatMessageRepository chatMessageRepository;
    private WindowNotificationManager? _notificationManager;

    public static FilePickerFileType CodeAll { get; } = new("All Code Files")
    {
        Patterns = new[]
        {
            "*.cs", "*.java", "*.js", "*.rs", "*.py", "*.md", "*.ts", "*.tsx",
            "*.cpp", "*.h", "*.hpp", "*.c", "*.h", "*.html", "*.css", "*.xml",
            "*.json", "*.php", "*.rb", "*.go", "*.swift", "*.kt", "*.scala",
            "*.groovy", "*.perl", "*.lua", "*.bash", "*.sh", "*.sql", "*.vue",
            "*.svelte", "*.dart", "*.r", "*.m", "*.mm", "*.gradle", "*.bat",
            "*.ps1", "*.psm1", "*.psd1", "*.ps1xml", "*.psc1", "*.psc2", "*.pslx",
            "*.pslxml", "*.xaml", "*.csproj", "*.sln", "*.cshtml", "*.razor",
            "*.fs", "*.fsi", "*.fsx", "*.fsscript", "*.fslit", "*.fsproj", "*.nuspec",
            "*.asm", "*.s", "*.asmx", "*.asmx.cs", "*.asmx.vb", "*.vb", "*.vbs",
            "*.vbscript", "*.vba", "*.vbscript", "*.vbscript", "*.vbscript",
            "*.vbproj", "*.il", "*.ilproj", "*.xslt", "*.xsl", "*.xslfo", "*.xslm",
            "*.xslmc", "*.xslmci", "*.xslmcx", "*.xslmcv", "*.xslmcvi", "*.xslmcxv",
            "*.xslmcxvi", "*.xslmcxvi", "*.xslmcxvi", "*.xslmcxvi", "*.xslmcxvi",
            "*.xslmcxvi", "*.xslmcxvi", "*.xslmcxvi", "*.xslmcxvi", "*.xslmcxvi",
            "*.axaml"
        }
    };

    public ChatInput()
    {
        InitializeComponent();

        chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();
        chatMessageRepository = HostApplication.Services.GetService<ChatMessageRepository>();
    }

    private ChatViewModel ViewModel => (ChatViewModel)DataContext;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _notificationManager = new WindowNotificationManager(HostApplication.Services.GetService<MainWindow>())
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 4,
            Margin = new Thickness(0, 0, 15, 40)
        };
    }


    public async Task SendChatMessage(bool autoCallTool = false)
    {
        if (string.IsNullOrEmpty(ViewModel.Message))
        {
            return;
        }

        ViewModel.IsGenerating = true;

        try
        {
            var user = new ChatMessage();
            user.Content = ViewModel.Message;
            user.Role = AuthorRole.User.ToString();
            user.CreatedAt = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.SessionId = ViewModel.SessionId;
            user.Meta.Add("avatar", "https://avatars.githubusercontent.com/u/10251060?v=4");
            user.Meta.Add("name", "User");
            bool isfirst = true;

            ViewModel.Message = string.Empty;

            ViewModel.Messages.Add(new ChatMessageListViewModel()
            {
                Id = user.Id,
                Content = user.Content,
                CreatedAt = user.CreatedAt,
                IsEditing = false,
                Meta = user.Meta,
                Plugin = user.Plugin,
                Role = user.Role,
                SessionId = user.SessionId,
                UpdatedAt = user.UpdatedAt,
            });
            await chatMessageRepository.InsertAsync(user);

            var newMessage = ViewModel.Messages.ToList();

            var bot = new ChatMessage
            {
                Role = AuthorRole.Assistant.ToString(),
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                SessionId = ViewModel.SessionId
            };
            bot.Meta.Add("avatar", "https://avatars.githubusercontent.com/u/10251060?v=4");
            bot.Meta.Add("name", "Assistant");
            bot.Content = "";

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

            var model = ViewModel.ModelId.Id;
            var files = ViewModel.Files.ToArray();
            
            await Task.Run(async () =>
            {
                await foreach (var item in chatCompleteService.GetChatComplete(newMessage, model,
                                   autoCallTool, files))
                {
                    if (isfirst)
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            bot.Content = string.Empty;
                            botView.Content = string.Empty;
                            isfirst = false;
                        });
                    }

                    int token = 0;
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
                    if (token == 3)
                    {
                        Dispatcher.UIThread.Invoke(() => { botView.Content = bot.Content; });
                    }
                    else if (token == 6)
                    {
                        token = 0;
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            botView.Content = bot.Content;

                            ViewModel.OnMessageUpdated?.Invoke();
                        });
                    }
                }
            });

            Dispatcher.UIThread.Invoke(() =>
            {
                botView.Content = bot.Content;
                ViewModel.OnMessageUpdated?.Invoke();
            });

            await chatMessageRepository.InsertAsync(bot);
        }
        catch (Exception e)
        {
            _notificationManager?.Show(
                new Notification("错误", e.Message, NotificationType.Error));
        }
        finally
        {
            ViewModel.IsGenerating = false;
        }
    }

    private async void AgentSubmit(object? sender, RoutedEventArgs e)
    {
        await SendChatMessage(true);
    }

    private async void Submit(object? sender, RoutedEventArgs e)
    {
        await SendChatMessage();
    }

    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.Messages.Clear();

        await chatMessageRepository.DeleteSession("default");
    }

    private async void OpenFile(object? sender, RoutedEventArgs e)
    {
        var _target = HostApplication.Services.GetService<MainWindow>();
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "请选择文件",
            AllowMultiple = true,
            FileTypeFilter = new[] { CodeAll, FilePickerFileTypes.TextPlain }
        });

        if (files.Count == 0)
        {
            return;
        }

        foreach (var file in files)
        {
            ViewModel.Files.Add(new FileModel()
            {
                Name = file.Name,
                FullName = file.Path.LocalPath,
                IsFile = true
            });
        }
    }

    private void DeleteFile(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: FileModel file })
        {
            ViewModel.Files.Remove(file);
        }
    }
}