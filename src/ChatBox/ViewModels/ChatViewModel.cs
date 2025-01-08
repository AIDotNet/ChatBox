using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading;
using ChatBox.Internal;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class ChatViewModel : ModelListViewModel
{
    private DispatcherTimer _updateTimer;

    private bool _isUpdating;

    public ChatViewModel()
    {
        Messages.CollectionChanged += (sender, args) =>
        {
            if (Messages.Count > 0)
            {
                WelcomeVisible = false;
            }
            else
            {
                WelcomeVisible = true;
            }

            StartUpdateTimer();
        };

        CurrentModel = ModelList.FirstOrDefault();
    }

    /// <summary>
    /// 是否贴右边
    /// </summary>
    /// <returns></returns>
    private bool _isRight = false;

    public bool IsRight
    {
        get => _isRight;
        set => SetProperty(ref _isRight, value);
    }

    private string _message;

    public string Message
    {
        get => _message;
        set
        {
            SetProperty(ref _message, value);
            StartUpdateTimer();
        }
    }

    public event EventHandler<string>? SessionChanged;

    private SessionsViewModel _session;

    public SessionsViewModel Session
    {
        get => _session;
        set
        {
            SetProperty(ref _session, value);
            SessionChanged?.Invoke(this, value.Id);
        }
    }

    private ObservableCollection<SessionsViewModel> _sessions = new();

    public ObservableCollection<SessionsViewModel> Sessions
    {
        get => _sessions;
        set => SetProperty(ref _sessions, value);
    }

    /// <summary>
    /// 加载Session
    /// </summary>
    /// <returns></returns>
    private bool _isSessionLoading;

    public bool IsSessionLoading
    {
        get => _isSessionLoading;
        set
        {
            SetProperty(ref _isSessionLoading, value);
            WelcomeVisible = !value;
        }
    }

    private ObservableCollection<ChatMessageListViewModel> _messages = new();

    public ObservableCollection<ChatMessageListViewModel> Messages
    {
        get => _messages;
        set => SetProperty(ref _messages, value);
    }

    private ObservableCollection<FileModel> _files = new();

    public ObservableCollection<FileModel> Files
    {
        get => _files;
        set => SetProperty(ref _files, value);
    }

    /// <summary>
    /// 消息更新事件
    /// </summary>
    /// <returns></returns>
    public Action OnMessageUpdated { get; set; }

    private bool _isGenerating;

    public bool IsGenerating
    {
        get => _isGenerating;
        set => SetProperty(ref _isGenerating, value);
    }

    private bool _welcomeVisible = true;

    public bool WelcomeVisible
    {
        get => _welcomeVisible;
        set => SetProperty(ref _welcomeVisible, value);
    }


    private int _token;

    public int Token
    {
        get => _token;
        set => SetProperty(ref _token, value);
    }

    private void StartUpdateTimer()
    {
        if (_updateTimer == null)
        {
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2),
            };
            _updateTimer.Tick += (sender, args) => CalculateToken();
        }

        if (!_updateTimer.IsEnabled)
        {
            _updateTimer.Start();
        }
    }

    private void CalculateToken()
    {
        if (_isUpdating)
        {
            return;
        }

        _isUpdating = true;

        _ = Task.Run(async () =>
        {
            var token = Messages.Where(x => !string.IsNullOrEmpty(x.Content)).Select(x => x.Content).ToArray()
                .Sum(s => TokenHelper.GetTotalTokens(s));

            token += TokenHelper.GetTotalTokens(Message);

            await Dispatcher.UIThread.InvokeAsync(() => Token = token);

            _isUpdating = false;

            // Stop the timer if no updates are detected
            if (Messages.All(x => string.IsNullOrEmpty(x.Content)) && string.IsNullOrEmpty(Message))
            {
                _updateTimer.Stop();
            }
        });
    }

    /// <summary>
    /// 当前模型
    /// </summary>
    /// <returns></returns>
    private ModelListDto _currentModel = new();

    public ModelListDto CurrentModel
    {
        get => _currentModel;
        set => SetProperty(ref _currentModel, value);
    }

    public List<ModelListDto> ModelList { get; set; } = new()
    {
        new ModelListDto()
        {
            Key = "Chat",
            Name = "对话模式"
        },
        new ModelListDto()
        {
            Key = "Agent",
            Name = "智能体模式"
        },
        new ModelListDto()
        {
            Key = "Inference",
            Name = "推理模式"
        },
    };
}