namespace ChatBox.Models;

public class Session
{
    public string Id { get; set; }

    /// <summary>
    /// 会话名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 会话描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 会话角色 Prompt 
    /// </summary>
    public string System { get; set; }

    /// <summary>
    /// 选择的模型
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// 最大历史数
    /// </summary>
    /// <returns></returns>
    public long MaxHistory { get; set; }

    /// <summary>
    /// 话题命名模型
    /// </summary>
    /// <returns></returns>
    public string TopicModel { get; set; }

    /// <summary>
    /// 会话的头像
    /// </summary>
    public string Avatar { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}