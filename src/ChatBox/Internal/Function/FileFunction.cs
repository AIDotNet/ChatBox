using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.Json;
using FluentAvalonia.Interop;
using Microsoft.SemanticKernel;

namespace ChatBox.Internal.Function
{
	/// <summary>
	/// 文件函数
	/// </summary>
	public class FileFunction
	{
		/// <summary>
		/// 获取当前系统硬盘数量
		/// </summary>
		/// <returns>硬盘数量</returns>
		[KernelFunction, Description("获取当前电脑所有硬盘信息")]
		public string GetDrives()
		{
			var list = new List<object>();

			foreach (var drive in DriveInfo.GetDrives())
			{
				try
				{
					list.Add(new
					{
						drive.Name,
						drive.DriveType,
						drive.DriveFormat,
						drive.TotalSize,
						drive.AvailableFreeSpace,
						drive.VolumeLabel,
						drive.IsReady,
					});
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}

			return JsonSerializer.Serialize(list);
		}

		/// <summary>
		/// 创建文件
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <param name="content">文件内容</param>
		[KernelFunction, Description("创建一个新文件并写入内容")]
		public async Task CreateFile(
			[Description("文件路径")] string filePath,
			[Description("文件内容")] string content)
		{
			await File.WriteAllTextAsync(filePath, content);
		}

		/// <summary>
		/// 创建目录
		/// </summary>
		/// <param name="directoryPath"></param>
		[KernelFunction, Description("创建一个新目录")]
		public void CreateDirectory(
			[Description("目录路径")] string directoryPath)
		{
			Directory.CreateDirectory(directoryPath);
		}

		/// <summary>
		/// 删除目录
		/// </summary>
		/// <param name="directoryPath"></param>
		[KernelFunction, Description("删除指定目录")]
		public void DeleteDirectory(
			[Description("目录路径")] string directoryPath)
		{
			Directory.Delete(directoryPath, true);
		}

		/// <summary>
		/// 移动目录
		/// </summary>
		/// <param name="sourceDirectoryPath"></param>
		/// <param name="targetDirectoryPath"></param>
		[KernelFunction, Description("移动目录，将源目录移动到目标目录，一般用于重命名目录")]
		public void MoveDirectory(
			[Description("源目录路径")] string sourceDirectoryPath,
			[Description("目标目录路径")] string targetDirectoryPath)
		{
			Directory.Move(sourceDirectoryPath, targetDirectoryPath);
		}

		/// <summary>
		/// 压缩文件或目录
		/// </summary>
		/// <returns></returns>
		[KernelFunction, Description("压缩文件或目录")]
		public void CompressFileOrDirectory(
			[Description("要压缩的文件或目录路径")] string sourcePath,
			[Description("压缩后的文件路径")] string targetPath)
		{
			ZipFile.CreateFromDirectory(sourcePath, targetPath);
		}

		/// <summary>
		/// 写入文件内容
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <param name="content">要写入的内容</param>
		[KernelFunction, Description("向指定文件写入内容")]
		public async Task WriteToFile(
			[Description("文件路径")] string filePath,
			[Description("要写入的内容")] string content)
		{
			await File.WriteAllTextAsync(filePath, content);
		}

		/// <summary>
		/// 获取指定硬盘下的文件和目录
		/// </summary>
		/// <param name="driveName">硬盘名称</param>
		/// <param name="path">
		/// 指定的路径
		/// </param>
		/// <returns>文件和目录列表</returns>
		[KernelFunction, Description("获取指定目录下的文件和目录")]
		public string[] GetFilesAndDirectories(
			[Description("具体路径，不能包含换行")] string? path)
		{
			if (Directory.Exists(path))
			{
				return Directory.GetFileSystemEntries(path);
			}

			throw new ArgumentException("指定的路径不存在");
		}

		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="filePath">要删除的文件路径</param>
		[KernelFunction, Description("删除指定文件")]
		public void DeleteFile(
			[Description("要删除的文件路径")] string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			else
			{
				throw new FileNotFoundException("指定的文件不存在");
			}
		}

		/// <summary>
		/// 读取文件内容
		/// </summary>
		/// <param name="filePath">要读取的文件路径</param>
		/// <returns>文件内容</returns>
		[KernelFunction, Description("读取指定文件的内容")]
		public string ReadFileContent(
			[Description("要读取的文件路径")] string filePath)
		{
			if (File.Exists(filePath))
			{
				return File.ReadAllText(filePath);
			}

			throw new FileNotFoundException("指定的文件不存在");
		}

		/// <summary>
		/// 搜索文件
		/// </summary>
		/// <param name="path">搜索路径</param>
		/// <param name="searchPattern">搜索模式</param>
		/// <returns>搜索结果</returns>
		/// <exception cref="ArgumentException">指定的路径不存在</exception>
		/// <exception cref="ArgumentNullException">搜索模式为空</exception>
		/// <exception cref="DirectoryNotFoundException">指定的路径不存在</exception>
		[KernelFunction, Description("搜索指定目录下的文件")]
		public string[] SearchFiles(
			[Description("搜索路径")] string path,
			[Description("搜索模式,支持通配符")] string searchPattern)
		{
			if (string.IsNullOrWhiteSpace(searchPattern))
			{
				throw new ArgumentNullException(nameof(searchPattern));
			}

			if (Directory.Exists(path))
			{
				return Directory.GetFiles(path, searchPattern);
			}

			throw new DirectoryNotFoundException("指定的路径不存在");
		}

		/// <summary>
		/// 获取指定文件的信息
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <returns>文件信息</returns>
		[KernelFunction, Description("获取指定文件的信息")]
		public string GetFileInfo(
			[Description("文件路径")] string filePath)
		{
			if (File.Exists(filePath))
			{
				var info = new FileInfo(filePath);
				return JsonSerializer.Serialize(new
				{
					info.Name,
					info.FullName,
					info.Extension,
					info.DirectoryName,
					info.CreationTime,
					info.LastAccessTime,
					info.LastWriteTime,
					info.Length,
					info.Attributes,
				});
			}

			throw new FileNotFoundException("指定的文件不存在");
		}

		/// <summary>
		/// 获取管理员权限
		/// </summary>
		[KernelFunction, Description("获取管理员权限")]
		public bool AdminPermission()
		{
			if (OSVersionHelper.IsWindows())
			{
				var identity = WindowsIdentity.GetCurrent();
				var principal = new WindowsPrincipal(identity);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				var process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = "sudo",
						Arguments = "echo",
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						RedirectStandardInput = true,
					}
				};
				process.Start();
				process.WaitForExit();
				return process.ExitCode == 0;
			}
			else
			{
				return false;
			}
		}

	}
}