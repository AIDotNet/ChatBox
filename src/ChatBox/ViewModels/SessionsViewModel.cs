namespace ChatBox.ViewModels;

public class SessionsViewModel : ViewModelBase
{
    private string _id;

    public string Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string _name;

    /// <summary>
    /// 会话名称
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _description;

    /// <summary>
    /// 会话描述
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _system;

    /// <summary>
    /// 会话角色 Prompt 
    /// </summary>
    public string System
    {
        get => _system;
        set => SetProperty(ref _system, value);
    }

    private string _model;

    /// <summary>
    /// 选择的模型
    /// </summary>
    public string Model
    {
        get => _model;
        set => SetProperty(ref _model, value);
    }

    private long _maxHistory;

    /// <summary>
    /// 最大历史数
    /// </summary>
    /// <returns></returns>
    public long MaxHistory
    {
        get => _maxHistory;
        set => SetProperty(ref _maxHistory, value);
    }

    private string _topicModel;

    /// <summary>
    /// 话题命名模型
    /// </summary>
    /// <returns></returns>
    public string TopicModel
    {
        get => _topicModel;
        set => SetProperty(ref _topicModel, value);
    }

    private string _avatar;

    /// <summary>
    /// 会话的头像
    /// </summary>
    public string Avatar
    {
        get => _avatar;
        set => SetProperty(ref _avatar, value);
    }

    private DateTime _createdAt;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }
}