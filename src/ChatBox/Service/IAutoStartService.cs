namespace ChatBox.Service;

public interface IAutoStartService
{
    bool IsAutoStart();
    
    /// <summary>
    /// 设置开机启动 | 关闭开机启动
    /// </summary>
    /// <param name="isAutoStart"></param>
    /// <returns></returns>
    bool SetAutoStart(bool isAutoStart);
}