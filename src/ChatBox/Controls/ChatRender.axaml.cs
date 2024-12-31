using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using ChatBox.AI;
using ChatBox.Views;
using FluentAvalonia.UI.Controls;
using Markdig.Syntax;
using MarkdownAIRender.CodeRender;
using TextMateSharp.Grammars;

namespace ChatBox.Controls
{
    public partial class ChatRender : UserControl
    {
        private WindowNotificationManager? _notificationManager;
        private readonly ChatCompleteService _chatCompleteService;

        public event EventHandler DeleteClicked;
        public event EventHandler CopyClicked;
        public event EventHandler EditClicked;
        public event EventHandler ReGenerateClicked;

        public ChatRender()
        {
            InitializeComponent();

            // 获取你的 AI 服务
            _chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _notificationManager = new WindowNotificationManager(HostApplication.Services.GetService<MainWindow>())
            {
                Position = NotificationPosition.TopRight,
                MaxItems = 4,
                Margin = new Thickness(0, 0, 15, 40)
            };
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CopyButton_Click(object? sender, RoutedEventArgs e)
        {
            CopyClicked?.Invoke(this, EventArgs.Empty);
        }

        private void EditButton_Click(object? sender, RoutedEventArgs e)
        {
            EditClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 删除并且重新生成
        /// </summary>
        private void ReGenerate_Click(object? sender, RoutedEventArgs e)
        {
            ReGenerateClicked?.Invoke(this, e);
        }

        private void RenderControl_OnCodeToolRenderEvent(
            StackPanel headerPanel,
            StackPanel stackPanel,
            FencedCodeBlock fencedCodeBlock)
        {
            // 1. 如果没有内容，直接返回
            if (fencedCodeBlock.Lines.Count < 1) return;

            // 2. 获取语言、文件路径等信息
            var infoParts = fencedCodeBlock.Info?.Split(":", StringSplitOptions.RemoveEmptyEntries)
                            ?? [];
            var language = infoParts.Length > 0 ? infoParts[0] : (fencedCodeBlock.Info ?? "text");
            var filePath = infoParts.Length > 1 ? string.Join(":", infoParts.Skip(1)) : string.Empty;

            // 3. 如果存在文件路径且是本地文件，则添加“应用代码”按钮，否则仅添加“Copy”按钮
            AddHeaderControls(headerPanel, language, filePath, fencedCodeBlock);

            // 4. 渲染代码
            var currentTheme = Application.Current?.RequestedThemeVariant == ThemeVariant.Light
                ? ThemeName.LightPlus
                : ThemeName.DarkPlus;

            var codeText = fencedCodeBlock.Lines.ToString();
            stackPanel.Children.Add(CodeRender.Render(codeText, language, currentTheme));
        }

        private void AddHeaderControls(
            StackPanel headerPanel,
            string language,
            string filePath,
            FencedCodeBlock fencedCodeBlock)
        {
            // 语言标签
            var languageText = CreateLanguageTextBlock(language);

            // 复制按钮
            var copyButton = CreateCopyButton(fencedCodeBlock.Lines.ToString());

            headerPanel.Children.Add(languageText);
            headerPanel.Children.Add(copyButton);

            // 若是本地文件路径，额外添加应用代码按钮
            if (IsLocalFilePath(filePath))
            {
                var applyButton = CreateApplyCodeButton(filePath, fencedCodeBlock);
                headerPanel.Children.Add(applyButton);
            }
        }

        private TextBlock CreateLanguageTextBlock(string language)
        {
            return new TextBlock
            {
                Text = language,
                Margin = new Thickness(0, 2, 10, 0)
            };
        }

        private Button CreateCopyButton(string codeContent)
        {
            var copyButton = new Button
            {
                Content = "复制",
                FontSize = 12,
                Height = 24,
                Padding = new Thickness(3),
                Margin = new Thickness(0)
            };

            copyButton.Click += (sender, e) =>
            {
                var window = HostApplication.Services.GetService<MainWindow>();
                window.Clipboard?.SetTextAsync(codeContent);
                _notificationManager?.Show(new Notification(
                    "已复制",
                    "代码已复制到剪贴板",
                    NotificationType.Success));
            };

            return copyButton;
        }

        private Button CreateApplyCodeButton(string filePath, FencedCodeBlock fencedCodeBlock)
        {
            var applyButton = new Button
            {
                Content = "应用代码",
                FontSize = 12,
                Height = 24,
                Padding = new Thickness(3),
                Margin = new Thickness(5, 0, 0, 0)
            };

            applyButton.Click += async (sender, e) =>
            {
                // 构建加载提示
                var panel = new WrapPanel();
                panel.Children.Add(new ProgressRing { Margin = new Thickness(1) });
                panel.Children.Add(new TextBlock { Text = "应用中" });
                applyButton.Content = panel;

                try
                {
                    var info = new FileInfo(filePath);
                    var codeText = fencedCodeBlock.Lines.ToString();

                    // 如果目录不存在则创建
                    if (info.Directory?.Exists == false)
                    {
                        info.Directory.Create();
                    }

                    // 如果是新文件则直接写入，否则执行代理代码
                    if (!info.Exists)
                    {
                        await using var stream = info.CreateText();
                        await stream.WriteAsync(codeText);
                    }
                    else
                    {
                        await _chatCompleteService.ApplyCode(info.FullName, codeText);
                    }

                    applyButton.Content = new TextBlock { Text = "应用完成" };
                }
                catch (Exception ex)
                {
                    applyButton.Content = new TextBlock { Text = "应用失败" };
                    _notificationManager?.Show(new Notification(
                        "应用失败",
                        "请检查代码是否正确" + Environment.NewLine + ex,
                        NotificationType.Error));
                }
            };

            return applyButton;
        }

        private static bool IsLocalFilePath(string path)
        {
            if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
            {
                return uri.IsFile;
            }

            return false;
        }
    }
}