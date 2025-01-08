using ChatBox.Service;

namespace ChatBox.Desktop;

public class MacisAutoStartService : IAutoStartService
{
    public bool IsAutoStart()
    {
        return false;
    }

    public bool SetAutoStart(Boolean isAutoStart)
    {
        return false;
    }
}