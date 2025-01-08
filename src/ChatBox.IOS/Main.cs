using ChatBox.Service;
using Microsoft.Extensions.DependencyInjection;
using UIKit;

namespace ChatBox.iOS;

public class Application
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        HostApplication.Builder((collection =>
        {
            collection.AddSingleton<IAutoStartService, NullAutoStartService>();
        }));
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}