using System.IO;

namespace ChatBox.Constant;

public class ConstantPath
{
    /// <summary>
    /// 数据存储目录
    /// </summary>
    public static string ChatDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ChatData");
    
    public static string ChatSettingPath => Path.Combine(ChatDataPath, "setting.json");
    
    /// <summary>
    /// 数据库路径
    /// </summary>
    public static string ChatDbPath => Path.Combine(ChatDataPath, "Chat.db");
    
}