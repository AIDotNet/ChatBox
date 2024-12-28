using System.ComponentModel;
using System.Diagnostics;
using Microsoft.SemanticKernel;

namespace ChatBox.Internal.Function;

/// <summary>
/// 命令函数
/// </summary>
public class CommandFunction
{
    /// <summary>
    /// 执行命令并且返回结果
    /// </summary>
    /// <param name="arguments">需要执行的命令</param>
    /// <returns>命令执行的结果</returns>
    [KernelFunction, Description("执行指令命令，并且返回执行结果")]
    public async Task<string> ExecuteCommand(
        [Description("需要执行的命令")] string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "bash",
            Arguments = "/c " + arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false // 控制台窗口不隐藏
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        return string.IsNullOrEmpty(error) ? output : error;
    }

    /// <summary>
    /// 执行命令并且返回结果
    /// </summary>
    /// <param name="file"></param>
    /// <returns>命令执行的结果</returns>
    [KernelFunction, Description("执行指定Bat文件，并且返回执行结果")]
    public async Task<string> ExecuteFileCommand(
        [Description("需要执行Bat文件")] string file)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = file,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false // 控制台窗口不隐藏
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        return string.IsNullOrEmpty(error) ? output : error;
    }
}