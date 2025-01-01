using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using AvaloniaXmlTranslator;
using ChatBox.Desktop;
using ChatBox.Pages;
using ChatBox.Service;
using ChatBox.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox.Views;

public partial class MainView : UserControl
{
    private readonly ISettingService _settingService;
    private WindowNotificationManager? _notificationManager;

    public MainView()
    {
        InitializeComponent();

        DataContext = HostApplication.Services.GetRequiredService<MainViewModel>();
        _settingService = HostApplication.Services.GetRequiredService<ISettingService>();
        _settingService.SetCulture(_settingService.GetCulture());

        HostApplication.Logout += () => { Dispatcher.UIThread.Invoke(() => { ViewModel.IsLogin = false; }); };
    }

    private MainViewModel ViewModel => (MainViewModel)DataContext;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _notificationManager = HostApplication.Services.GetService<WindowNotificationManager>();

        var setting = _settingService.LoadSetting();

        ViewModel.IsLogin = !string.IsNullOrEmpty(setting.ApiKey);

        NavView.SelectedItem = NavView.MenuItems[0];
        NavView.FindControl<NavigationView>("");
    }

    private void NvSample_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem item)
        {
            if (item.Name?.Equals("SettingsItem", StringComparison.OrdinalIgnoreCase) == true)
            {
                ViewModel.OnNavigation(ViewModel, MenuKeys.MenuKeySetting);
                return;
            }

            ViewModel.OnNavigation(ViewModel, item.Tag.ToString());
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        CrossPlatformCustomProtocolHelper.OpenCustomProtocolUrl();

        _settingService.UpdateCallback(() =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                // 登录成功
                _notificationManager?.Show(
                    new Notification(
                        I18nManager.Instance.GetResource(Localization.Views.MainView.loginSuccessNotificationTitle),
                        I18nManager.Instance.GetResource(Localization.Views.MainView.loginSuccessNotificationMessage), NotificationType.Success));

                ViewModel.IsLogin = true;
            });
        });
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
}