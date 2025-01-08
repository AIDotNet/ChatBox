using System;
using System.Diagnostics;
using System.Reflection;
using ChatBox.Service;

namespace ChatBox.Desktop.Services;

public class WindowAutoStartService : IAutoStartService
{
    public bool IsAutoStart()
    {
        // 判断当前window系统否存在开机启动
        using var userRunKey =
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


        string exePath = Process.GetCurrentProcess().MainModule.FileName;
        string exeName = Assembly.GetExecutingAssembly().GetName().Name;


        object runValue = userRunKey.GetValue(exeName);
        if (runValue != null &&
            string.Equals(runValue.ToString(), $"\"{exePath}\"", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        
        return false;
    }

    public bool SetAutoStart(bool isAutoStart)
    {
        string exePath = Process.GetCurrentProcess().MainModule.FileName;
        string exeName = Assembly.GetExecutingAssembly().GetName().Name;
        using var key =
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (isAutoStart)
        {
            key.SetValue(exeName, $"\"{exePath}\"");
        }
        else
        {
            key.DeleteValue(exeName, false);
        }

        return true;
    }
}