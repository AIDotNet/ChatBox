using ChatBox.Models;

namespace ChatBox.Service;

public interface ISettingService
{
    public void UpdateCallback(Action settingModel);

    public void SaveSetting(SettingModel settingModel);

    public void InitSetting(string token);

    public SettingModel LoadSetting();
}