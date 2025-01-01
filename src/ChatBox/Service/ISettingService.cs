using AvaloniaXmlTranslator;
using ChatBox.Models;
using System.Threading;

namespace ChatBox.Service;

public interface ISettingService
{
    public string GetCulture();

    public void SetCulture(string culture);
    public void UpdateCallback(Action settingModel);

    public void SaveSetting(SettingModel settingModel);

    public void InitSetting(string token);

    public SettingModel LoadSetting();
}