using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
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

        HostApplication.Services.GetService<SettingService>()!.SaveSetting(setting);

        _notificationManager?.Show(
            new Notification("保存成功", "设置保存成功", NotificationType.Success));
    }

    private void InitializeSetting()
    {
        ViewModel.ModelProvider.Clear();
        ViewModel.Setting = HostApplication.Services.GetService<SettingService>()!.LoadSetting();
        var models = _modelProviderService.LoadModelProviders();
        foreach (var modelProvider in models)
        {
            ViewModel.ModelProvider.Add(modelProvider);
        }

        var item = models.FirstOrDefault(x => x.Id == ViewModel.Setting.Type);

        ViewModel.SelectedModelProvider = item ?? models.FirstOrDefault();
    }
}