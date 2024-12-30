using Avalonia;
using System;
using ChatBox.Service;

namespace ChatBox.Desktop;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {

            HostApplication.Builder();


            CrossPlatformCustomProtocolHelper.RegisterCustomProtocol();

            var token = CrossPlatformCustomProtocolHelper.ParseCustomProtocolArgs(args);

            if (!string.IsNullOrEmpty(token))
            {
                var settingService = new SettingService();

                settingService.InitSetting(token);

                // 获取已经运行的程序实例
                var runningInstance = System.Diagnostics.Process.GetProcessesByName(
                    System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()
                        ?.Location));

                // 如果已经有实例在运行，则激活它然后退出
                foreach (var process in runningInstance)
                {
                    if (process.Id != Environment.ProcessId)
                    {
                        // 确保窗口是激活的`
                        ShowWindow(process.MainWindowHandle, 9);
                        return;
                    }
                }
            }

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            HostApplication.Error("程序错误终止", ex);
        }
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern void ShowWindow(IntPtr hWnd, int nCmdShow);

    private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        HostApplication.Error("程序错误", (Exception)e.ExceptionObject);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}