using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using ChatBox.Views;

namespace ChatBox;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = HostApplication.Services.GetRequiredService<MainWindow>();

            // 创建托盘
            var trayIcon = new TrayIcon();

            desktop.MainWindow.Closing += (sender, e) =>
            {
                e.Cancel = true;
                desktop.MainWindow.Hide();
            };

            trayIcon.Menu = InitializeMenu();


            trayIcon.Clicked += (sender, e) => { desktop.MainWindow.Show(); };

            // 设置托盘图标
            var favicon = AssetLoader.OpenAndGetAssembly(new Uri("avares://ChatBox/Assets/favicon.ico"));
            trayIcon.Icon = new WindowIcon(favicon.stream);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = HostApplication.Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private NativeMenu InitializeMenu()
    {
        var menu = new NativeMenu();

        var exit = new NativeMenuItem
        {
            Header = "退出程序"
        };
        exit.Click += (sender, args) => { Environment.Exit(0); };

        var chat = new NativeMenuItem
        {
            Header = "显示主界面"
        };

        chat.Click += (sender, args) =>
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow?.Show();
            }
        };

        menu.Items.Add(chat);
        menu.Items.Add(new NativeMenuItemSeparator());
        menu.Items.Add(exit);

        return menu;
    }
}