namespace ChatBox.Models;

public class ModelDto
{
    public string Id { get; set; }

    /// <summary>
    /// 部署名称
    /// </summary>
    public string DeploymentName { get; set; }
    
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    public string DisplayName { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// 是否支持FunctionCall
    /// </summary>
    public bool FunctionCall { get; set; }
    
    /// <summary>
    /// 是否支持vision
    /// </summary>
    public bool Vision { get; set; }
    
    /// <summary>
    /// tokens最大长度
    /// </summary>
    public int MaxTokens { get; set; }
    
    /// <summary>
    /// maxOutput
    /// </summary>
    public int MaxOutput { get; set; }

    public string Icon { get; set; }
}