using System;
using System.IO;

namespace ChatBox.Desktop;

public class CrossPlatformCustomProtocolHelper
{
    private const string CustomProtocol = "chatbox";

    /// <summary>
    /// 在程序启动时，检查命令行参数有没有带自定义协议的回调。
    /// 如果有，则解析出 token 或其他信息。
    /// </summary>
    public static string? ParseCustomProtocolArgs(string[] args)
    {
        // args[0] 可能形如 "myapp://callback?token=abc123&other=xxx"
        // 要先判断是否带有 "myapp://" 前缀
        if (args.Length > 0 && args[0].StartsWith(CustomProtocol + "://", StringComparison.OrdinalIgnoreCase))
        {
            Uri uri = new Uri(args[0]);
            // 这里就能解析出 callback Path、Querystring
            // uri.AbsolutePath -> "/callback"
            // uri.Query -> "?token=abc123&other=xxx"

            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
            string token = queryParams["token"];
            return token; // 返回拿到的登录凭据
        }

        return null; // 没有匹配到自定义协议URL
    }

    /// <summary>
    /// 打开自定义协议的 URL到浏览器
    /// </summary>
    public static void OpenCustomProtocolUrl()
    {
        var url = CustomProtocol + "://callback";
        // 打开https://api.token-ai.cn/login?redirect_uri=chatbox://callback
        // 会调用默认浏览器打开 URL
        var process = new System.Diagnostics.Process();
        // 使用默认浏览器打开
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = "https://api.token-ai.cn/login?redirect_uri=" + url;
        process.Start();
    }

    /// <summary>
    /// 将当前应用程序的自定义协议注册到系统中
    /// </summary>
    public static void RegisterCustomProtocol()
    {
        // 判断当前系统
        if (OperatingSystem.IsWindows())
        {
            // 判断是否已经注册过
            if (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(CustomProtocol) != null)
            {
                return;
            }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatBox.Desktop.exe");

            // 将自定义协议注册到系统中
            // HKEY_CLASSES_ROOT\chatbox\shell\open\command
            // (默认) = "ChatBox.Desktop.exe %1"
            var key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(CustomProtocol);
            
            key.SetValue("", "URL:ChatBox Protocol");
            key.SetValue("URL Protocol", "");
            key.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command").SetValue("", $"\"{path}\" \"%1\"");

            // HKEY_CLASSES_ROOT\chatbox\DefaultIcon
            // (默认) = "ChatBox.Desktop.exe,1"
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(CustomProtocol).CreateSubKey("DefaultIcon");
            key.SetValue("", $"\"{path}\",1");

        }
        else if (OperatingSystem.IsLinux())
        {
            // Linux
            // 通过 xdg-open 命令打开 URL
            // xdg-open chatbox://callback?token=abc123
            // 会调用默认浏览器打开 URL
            // 但是需要先注册协议处理程序
            // 可以参考 https://specifications.freedesktop.org/desktop-entry-spec/latest/ar01s05.html
            // 或者直接修改 ~/.local/share/applications/mimeapps.list
            // 添加 chatbox=chatbox.desktop
            // 然后在 ~/.local/share/applications/ 目录下创建 chatbox.desktop 文件
            // 文件内容如下：
            // [Desktop Entry]
            // Name=ChatBox
            // Exec=/usr/bin/xdg-open %u
            // Terminal=false
            // Type=Application
            // NoDisplay=true
            // MimeType=x-scheme-handler/chatbox
            // 判断是否已经注册过
            if (System.IO.File.Exists(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "chatbox.desktop")))
            {
                return;
            }

            // 或者直接调用 xdg-mime 命令
            // 注册
            var mime = "x-scheme-handler/" + CustomProtocol;
            var desktopFile = "chatbox.desktop";
            var desktopFilePath =
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    desktopFile);

            System.IO.File.WriteAllText(desktopFilePath, "[Desktop Entry]\n" +
                                                         $"Name={CustomProtocol}\n" +
                                                         $"Exec=/usr/bin/xdg-open %u\n" +
                                                         "Terminal=false\n" +
                                                         "Type=Application\n" +
                                                         "NoDisplay=true\n" +
                                                         $"MimeType={mime}\n");

            System.Diagnostics.Process.Start("xdg-mime", $"default {desktopFile} {mime}");
        }
        else if (OperatingSystem.IsMacOS())
        {
            // 判断是否已经注册过
            if (System.IO.File.Exists(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Info.plist")))
            {
                return;
            }

            // macOS
            // 通过 open 命令打开 URL
            // open chatbox://callback?token=abc123
            // 会调用默认浏览器打开 URL
            // 但是需要先注册协议处理程序
            // 可以参考 https://developer.apple.com/documentation/bundleresources/information_property_list/lsurltypes
            // 或者直
            // 直接修改 Info.plist 文件
            var plistPath =
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName),
                    "Info.plist");
            var plist = new System.Xml.XmlDocument();
            plist.Load(plistPath);
            var dict = plist.SelectSingleNode("/plist/dict");

            var urlTypes = plist.CreateElement("key");
            urlTypes.InnerText = "CFBundleURLTypes";
            dict.AppendChild(urlTypes);

            var array = plist.CreateElement("array");
            dict.AppendChild(array);

            var urlType = plist.CreateElement("dict");

            var urlName = plist.CreateElement("key");

            urlName.InnerText = "CFBundleURLName";

            urlType.AppendChild(urlName);
        }
    }
}