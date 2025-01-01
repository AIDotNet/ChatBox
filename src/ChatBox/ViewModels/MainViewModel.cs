using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using ChatBox.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace ChatBox.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private bool isLogin = false;

    public bool IsLogin
    {
        get => isLogin;
        set => SetProperty(ref isLogin, value);
    }

    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<MainViewModel, string>(this, OnNavigation);

        OnNavigation(this, MenuKeys.MenuKeyAIChat);
    }

    public object control;

    public object Control
    {
        get => control;
        set => SetProperty(ref control, value);
    }

    public void OnNavigation(MainViewModel vm, string s)
    {
        Control = s switch
        {
            MenuKeys.MenuKeyAIChat => HostApplication.Services.GetRequiredService<ChatViewModel>(),
            MenuKeys.MenuKeyTool => HostApplication.Services.GetRequiredService<ToolViewModel>(),
            MenuKeys.MenuKeySetting => HostApplication.Services.GetRequiredService<SettingViewModel>(),
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
    }

    private string _selectedNavMenu = MenuKeys.MenuKeyAIChat;

    public string SelectedNavMenu
    {
        get => _selectedNavMenu;
        set => SetProperty(ref _selectedNavMenu, value);
    }
}