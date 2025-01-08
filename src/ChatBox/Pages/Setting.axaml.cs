using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaXmlTranslator;
using Avalonia.VisualTree;
using ChatBox.Service;
using ChatBox.ViewModels;
using ChatBox.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox.Pages;

public partial class Setting : UserControl
{
    private WindowNotificationManager? _notificationManager;
    private readonly ModelProviderService _modelProviderService;

    public Setting()
    {
        InitializeComponent();

        _modelProviderService = HostApplication.Services.GetService<ModelProviderService>();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        InitializeSetting();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _notificationManager = HostApplication.Services.GetService<WindowNotificationManager>();
    }

    private SettingViewModel ViewModel => (SettingViewModel)DataContext;

    private void SaveSetting(object? sender, RoutedEventArgs routedEventArgs)
    {
        var setting = ViewModel.Setting;

        setting.Type = ViewModel.SelectedModelProvider.Id;
        if (setting.Language != ViewModel.SelectedLanguage?.CultureName)
        {
            setting.Language = ViewModel.SelectedLanguage?.CultureName;
            I18nManager.Instance.Culture = new CultureInfo(setting.Language);
        }

        HostApplication.Services.GetService<ISettingService>()!.SaveSetting(setting);

        _notificationManager?.Show(
            new Notification(I18nManager.Instance.GetResource(Localization.Pages.Setting.SaveSuccessNotificationTitle),
                I18nManager.Instance.GetResource(Localization.Pages.Setting.SaveSuccessNotificationMessage),
                NotificationType.Success));
    }

    private void InitializeSetting()
    {
        ViewModel.ModelProvider.Clear();
        ViewModel.Setting = HostApplication.Services.GetService<ISettingService>()!.LoadSetting();
        var models = _modelProviderService.LoadModelProviders();
        foreach (var modelProvider in models)
        {
            ViewModel.ModelProvider.Add(modelProvider);
        }

        var item = models.FirstOrDefault(x => x.Id == ViewModel.Setting.Type);

        ViewModel.SelectedModelProvider = item ?? models.FirstOrDefault();

        // 获取当前应用程序是否存在系统开机
        if (HostApplication.Services.GetService<IAutoStartService>()!.IsAutoStart())
        {
            ViewModel.IsAutoStart = true;
        }
        else
        {
            ViewModel.IsAutoStart = false;
        }
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 获取当前窗口并调用 BeginMoveDrag
        var window = this.GetVisualRoot() as Window;
        if (window != null)
        {
            window.BeginMoveDrag(e);
        }
    }

    private void Logout(object? sender, RoutedEventArgs e)
    {
        // 如果返回401，清空ApiKey
        var settings = HostApplication.Services.GetRequiredService<ISettingService>().LoadSetting();

        settings.ApiKey = string.Empty;
        HostApplication.Services.GetRequiredService<ISettingService>().SaveSetting(settings);

        HostApplication.Logout?.Invoke();
    }

    private void ToggleAutoStart(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.IsAutoStart)
        {
            HostApplication.Services.GetService<IAutoStartService>()!.SetAutoStart(false);
            ViewModel.IsAutoStart = false;
        }
        else
        {
            HostApplication.Services.GetService<IAutoStartService>()!.SetAutoStart(true);
            ViewModel.IsAutoStart = true;
        }
        
        _notificationManager?.Show(
            new Notification("设置成功",
                "设置成功",
                NotificationType.Success));
    }
}