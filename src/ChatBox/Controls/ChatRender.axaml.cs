using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using ChatBox.AI;
using ChatBox.Models;
using ChatBox.Views;
using FluentAvalonia.UI.Controls;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdownAIRender.CodeRender;
using TextMateSharp.Grammars;

namespace ChatBox.Controls
{
    public partial class ChatRender : UserControl
    {
        private WindowNotificationManager? _notificationManager;
        private readonly ChatCompleteService _chatCompleteService;

        // 1. 用于存储当前 Markdown 内容的私有字段
        private string _value = string.Empty;

        // 2. Public 属性，供外部绑定或设置 Markdown 文本
        public string Value
        {
            get => _value;
            set
            {
                // 若新值与旧值相同，跳过渲染
                if (_value == value)
                    return;

                _value = value;

                // 异步触发渲染流程，避免阻塞UI线程
                RenderMarkdownAsync(_value);
            }
        }

        public event EventHandler DeleteClicked;
        public event EventHandler CopyClicked;
        public event EventHandler EditClicked;
        public event EventHandler ReGenerateClicked;

        public ChatRender()
        {
            InitializeComponent();

            // 获取你的 AI 服务
            _chatCompleteService = HostApplication.Services.GetService<ChatCompleteService>();

            // 订阅主题更改事件（若你需要监听 Avalonia 框架的主题切换）
            Application.Current.ActualThemeVariantChanged += OnThemeVariantChanged;
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

        /// <summary>
        /// 主题变更时触发
        /// </summary>
        private void OnThemeVariantChanged(object sender, EventArgs e)
        {
            // 若需要在主题切换时重新渲染，可以再次调用
            // 不想完全重新解析 Markdown，可自行优化只更新代码高亮部分
            RenderMarkdownAsync(_value);
        }

        /// <summary>
        /// 异步渲染 Markdown
        /// </summary>
        private async void RenderMarkdownAsync(string markdownText)
        {
            // 在后台解析 Markdown
            var controls = await Task.Run(() =>
            {
                // 解析 Markdown
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var document = Markdig.Markdown.Parse(markdownText ?? string.Empty, pipeline);

                // 根据解析结果，生成控件列表
                // 你可把这部分逻辑拆到单独方法里，这里为演示直接内联
                var panel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 6
                };

                // 遍历节点并渲染
                foreach (var block in document)
                {
                    // 简化演示，只渲染代码块
                    if (block is FencedCodeBlock fencedCodeBlock)
                    {
                        var container = new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Margin = new Thickness(0, 6)
                        };

                        var headerPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal
                        };

                        var contentPanel = new StackPanel
                        {
                            Orientation = Orientation.Vertical
                        };

                        // 使用你原本的代码块渲染逻辑
                        RenderControl_OnCodeToolRenderEvent(headerPanel, contentPanel, fencedCodeBlock);

                        container.Children.Add(headerPanel);
                        container.Children.Add(contentPanel);

                        panel.Children.Add(container);
                    }
                    else
                    {
                        // 其他类型的节点可自行处理
                        // 例如添加到某个 TextBlock 中等
                    }
                }

                return panel;
            });

            // 回到UI线程，将生成好的控件赋给本控件的 Content 或者合并到你已有的 Panel 中
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                // 假设此控件本身是一个 ContentControl 或者你有一个内部 Panel
                // 为方便演示，这里直接把结果赋给 Content
                // 如果你在 XAML 里有 <ContentPresenter /> 或 <Panel x:Name="RootPanel" />，
                // 你可自行替换对应写法
                Content = controls;
            });
        }

        private void RenderControl_OnCodeToolRenderEvent(
            StackPanel headerPanel,
            StackPanel stackPanel,
            FencedCodeBlock fencedCodeBlock)
        {
            // 1. 如果没有内容，直接返回
            if (fencedCodeBlock.Lines.Count <= 1) return;

            // 2. 获取语言、文件路径等信息
            var infoParts = fencedCodeBlock.Info?.Split(":", StringSplitOptions.RemoveEmptyEntries) 
                           ?? Array.Empty<string>();
            var language = infoParts.Length > 0 ? infoParts[0] : (fencedCodeBlock.Info ?? "text");
            var filePath = infoParts.Length > 1 ? string.Join(":", infoParts.Skip(1)) : string.Empty;

            // 3. 如果存在文件路径且是本地文件，则添加“应用代码”按钮，否则仅添加“Copy”按钮
            AddHeaderControls(headerPanel, language, filePath, fencedCodeBlock);

            // 4. 渲染代码
            var currentTheme = Application.Current.RequestedThemeVariant == ThemeVariant.Light
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
                        using var stream = info.CreateText();
                        stream.Write(codeText);
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