using System;
using Foundation;
using UIKit;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.iOS;
using ChatBox.Desktop;
using ChatBox.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public partial class AppDelegate : AvaloniaAppDelegate<App>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    public AppDelegate()
    {
        // 订阅 Activated 事件
        ((IAvaloniaAppDelegate)this).Activated += OnActivated;
    }

    private void OnActivated(object? sender, ActivatedEventArgs e)
    {
        // 检查是否是 URL 协议激活
        if (e is ProtocolActivatedEventArgs protocolArgs)
        {
            var uri = protocolArgs.Uri.ToString();
            var token =  CrossPlatformCustomProtocolHelper.ParseCustomProtocolArgs([uri]);
            if (!string.IsNullOrWhiteSpace(token))
            {
                var settingService = HostApplication.Services.GetRequiredService<SettingService>();
                settingService.InitSetting(token);
            }
        }
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .LogToTrace();
    }
}