using Avalonia;
using Avalonia.Controls.Notifications;
using ChatBox.AI;
using ChatBox.Logger;
using ChatBox.Pages;
using ChatBox.Service;
using ChatBox.Views;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChatBox;

public class HostApplication
{
    private static IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns></returns>
    public static Action Logout { get; set; } = null!;

    public static IServiceProvider Services => _serviceProvider;

    public static void Infonrmation(string message)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogInformation(message);
    }

    public static void Error(string message)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogError(message);
    }

    public static void Warning(string message)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogWarning(message);
    }

    public static void Debug(string message)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogDebug(message);
    }

    public static void Infonrmation(string message, Exception exception)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogInformation(exception, message);
    }

    public static void Error(string message, Exception exception)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogError(exception, message);
    }

    public static void Warning(string message, Exception exception)
    {
        var logger = _serviceProvider.GetService<ILogger<HostApplication>>();
        logger.LogWarning(exception, message);
    }

    public static IHost Builder()
    {
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<ChatViewModel>();

				services.AddSingleton<MainWindow>();
				services.AddSingleton<Setting>();
				services.AddSingleton<SettingViewModel>();
                if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() ||
                    OperatingSystem.IsMacCatalyst() || OperatingSystem.IsLinux())
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<WindowNotificationManager>(s => new WindowNotificationManager(s.GetRequiredService<MainWindow>())
                    {
                        Position = NotificationPosition.TopRight,
                        MaxItems = 4,
                        Margin = new Thickness(0, 0, 15, 40)
                    });
                }
                else
                {
                    services.AddSingleton<MainView>();
                    services.AddSingleton<WindowNotificationManager>(s => new WindowNotificationManager(TopLevel.GetTopLevel(s.GetRequiredService<MainView>()))
                    {
                        Position = NotificationPosition.TopRight,
                        MaxItems = 4,
                        Margin = new Thickness(0, 0, 15, 40)
                    });
                }

                services.AddSingleton<Setting>();

				services.AddSingleton<Tool>();
				services.AddSingleton<ToolViewModel>();

				services.AddSingleton<Chat>();

				services.AddSingleton<Lazy<DbContext>>((provider =>
				{
					return new Lazy<DbContext>(() => new DbContext());
				}));

				services.AddSingleton<ChatCompleteService>();

				services.AddSingleton<SettingService>();


				services.AddSingleton<TokenService>();
				services.AddSingleton<ModelProviderService>();

				services.AddSingleton<ChatMessageRepository>();
			}).ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddChatBoxLogger();
			})

			.Build();

		_serviceProvider = host.Services;

		return host;
	}
}