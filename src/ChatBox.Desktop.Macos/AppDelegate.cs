using System;
using System.IO;
using System.Linq;
using AppKit;
using Avalonia;
using ChatBox.Constant;
using ChatBox.Service;
using Foundation;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox.Desktop;

[Register("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    public override void DidFinishLaunching(NSNotification notification)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime([]);
    }

    public override void OpenUrls(NSApplication application, NSUrl[] urls)
    {
        var token =  CrossPlatformCustomProtocolHelper.ParseCustomProtocolArgs(urls.Select(u => u.AbsoluteString).Where(x=>!string.IsNullOrWhiteSpace(x)).ToArray()!);
        if (!string.IsNullOrWhiteSpace(token))
        {
            var settingService = HostApplication.Services.GetRequiredService<ISettingService>();
            settingService.InitSetting(token);
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}