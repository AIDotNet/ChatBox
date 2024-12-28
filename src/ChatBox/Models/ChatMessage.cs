using System.Collections.Generic;

namespace ChatBox.Models;

public class ChatMessage
{
    public string Content { get; set; }


    public string Role { get; set; }


    public Dictionary<string, string> Meta { get; set; } = new();


    public DateTime CreatedAt { get; set; }


    public DateTime? UpdatedAt { get; set; }


    public string SessionId { get; set; }


    public ChatMessagePlugin? Plugin { get; set; }


    public string Id { get; set; }
}

public class ChatMessagePlugin
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 参数
    /// </summary>
	public string Arguments { get; set; }
}