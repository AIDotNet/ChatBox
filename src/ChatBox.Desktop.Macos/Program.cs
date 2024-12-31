using Avalonia;
using System;
using System.IO;
using ChatBox.Service;
using AppKit;

namespace ChatBox.Desktop;

static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            HostApplication.Builder();
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
        catch (Exception ex)
        {
            HostApplication.Error("程序错误终止", ex);
        }
    }

    private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        HostApplication.Error("程序错误", (Exception)e.ExceptionObject);
    }
}