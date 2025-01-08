namespace ChatBox.Service;

public class NullAutoStartService : IAutoStartService
{
    public bool IsAutoStart()
    {
        return false;
    }

    public bool SetAutoStart(bool isAutoStart)
    {
        return false;
    }
}