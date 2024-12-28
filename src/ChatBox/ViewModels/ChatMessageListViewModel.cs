using System.Collections.Generic;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class ChatMessageListViewModel : ViewModelBase
{
    private string _content;

    public string Content
    {
        get => _content;
        set
        {
            SetProperty(ref _content, value);
            ContentChanged?.Invoke();
        }
    }

    public Action? ContentChanged { get; set; }

    private string _role;

    public string Role
    {
        get => _role;
        set => SetProperty(ref _role, value);
    }

    private Dictionary<string, string> _meta = new();

    public Dictionary<string, string> Meta
    {
        get => _meta;
        set => SetProperty(ref _meta, value);
    }

    private DateTime _createdAt;

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    private DateTime? _updatedAt;

    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => SetProperty(ref _updatedAt, value);
    }

    private string _sessionId;

    public string SessionId
    {
        get => _sessionId;
        set => SetProperty(ref _sessionId, value);
    }

    private ChatMessagePlugin? _plugin;

    public ChatMessagePlugin? Plugin
    {
        get => _plugin;
        set => SetProperty(ref _plugin, value);
    }

    private string _id;

    public string Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    /// <summary>
    /// 编辑状态
    /// </summary>
    /// <returns></returns>
    private bool _isEditing = false;

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public ChatMessageListViewModel This => this;
}