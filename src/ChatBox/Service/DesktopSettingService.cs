using System.IO;
using System.Text.Json;
using ChatBox.Constant;
using ChatBox.Models;

namespace ChatBox.Service;

public class DesktopSettingService : ISettingService
{
    private readonly string _settingConfig = ConstantPath.ChatSettingPath;

    /// <summary>
    /// 监听_settingConfig文件的变化
    /// </summary>
    /// <param name="settingModel"></param>
    public void UpdateCallback(Action settingModel)
    {
        var path = new FileInfo(_settingConfig);

        if (path.Directory?.Exists == false)
        {
            path.Directory.Create();
        }

        // 监听文件变化
        var watcher = new FileSystemWatcher(path.DirectoryName ?? throw new InvalidOperationException(), path.Name)
        {
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.LastWrite,
            Filter = path.Name
        };

        watcher.Changed += (sender, args) =>
        {
            settingModel.Invoke();

            watcher.EnableRaisingEvents = false;

            watcher.Dispose();
        };
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