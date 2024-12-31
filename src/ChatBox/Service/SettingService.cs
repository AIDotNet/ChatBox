using System.IO;
using System.Text.Json;
using ChatBox.Constant;
using ChatBox.Models;

namespace ChatBox.Service;

public class SettingService
{
    private readonly string _settingConfig = ConstantPath.ChatSettingPath;
    private Action? _settingCallback;
    
    /// <summary>
    /// 监听_settingConfig文件的变化
    /// </summary>
    /// <param name="settingModel"></param>
    public void FileChange(Action settingModel)
    {
        _settingCallback = settingModel;
    }

    public void SaveSetting(SettingModel settingModel)
    {
        var path = new FileInfo(_settingConfig);

        if (path.Directory?.Exists == false)
        {
            path.Directory.Create();
        }

        var content = JsonSerializer.Serialize(settingModel, AppJsonSerialize.SerializerOptions);

        File.WriteAllText(_settingConfig, content);
        
        _settingCallback?.Invoke();
    }

    public void InitSetting(string token)
    {
        var setting = LoadSetting();
        setting.ApiKey = token;
        SaveSetting(setting);
    }

    public SettingModel LoadSetting()
    {
        if (!File.Exists(_settingConfig))
        {
            return new SettingModel();
        }

        var content = File.ReadAllText(_settingConfig);

        var result = JsonSerializer.Deserialize<SettingModel>(content, AppJsonSerialize.SerializerOptions);

        return result;
    }
}