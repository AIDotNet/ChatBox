namespace ChatBox.Models;

public class FileModel
{
    public string Name { get; set; }

    public string FullName { get; set; }
    
    /// <summary>
    /// 是否文件
    /// </summary>
    public bool IsFile { get; set; }
    
    public FileModel This => this;
}