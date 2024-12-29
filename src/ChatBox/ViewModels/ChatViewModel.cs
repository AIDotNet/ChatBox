using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChatBox.Internal;
using ChatBox.Models;

namespace ChatBox.ViewModels;

public class ChatViewModel : ModelListViewModel
{
	public ChatViewModel()
	{
		Messages.CollectionChanged += (sender, args) =>
		{
			if (Messages.Count > 0)
			{
				WelcomeVisible = false;
			}
			else
			{
				WelcomeVisible = true;
			}

			CalculateToken();
		};
	}

	private string _message;

	public string Message
	{
		get => _message;
		set
		{
			SetProperty(ref _message, value);
			CalculateToken();
		}
	}


	private string _sessionId = "default";

	public string SessionId
	{
		get => _sessionId;
		set => SetProperty(ref _sessionId, value);
	}

	private ObservableCollection<ChatMessageListViewModel> _messages = new();

	public ObservableCollection<ChatMessageListViewModel> Messages
	{
		get => _messages;
		set => SetProperty(ref _messages, value);
	}

	private ObservableCollection<FileModel> _files = new();

	public ObservableCollection<FileModel> Files
	{
		get => _files;
		set => SetProperty(ref _files, value);
	}

	/// <summary>
	/// 消息更新事件
	/// </summary>
	/// <returns></returns>
	public Action OnMessageUpdated { get; set; }

	private bool _isGenerating;

	public bool IsGenerating
	{
		get => _isGenerating;
		set => SetProperty(ref _isGenerating, value);
	}

	private bool _welcomeVisible = true;

	public bool WelcomeVisible
	{
		get => _welcomeVisible;
		set => SetProperty(ref _welcomeVisible, value);
	}


	private int _token;

	public int Token
	{
		get => _token;
		set => SetProperty(ref _token, value);
	}


	/// <summary>
	/// 计算token
	/// </summary>
	public void CalculateToken()
	{
		var token = 0;
		foreach (var s in Messages.Where(x => !string.IsNullOrEmpty(x.Content)).Select(x => x.Content).ToArray())
		{
			token += TokenHelper.GetTotalTokens(s);
		}

		Token = token + TokenHelper.GetTotalTokens(Message);
	}

	/// <summary>
	/// 当前模型
	/// </summary>
	/// <returns></returns>
	private string _currentModel = "Chat";
	
	public string CurrentModel
	{
		get => _currentModel;
		set => SetProperty(ref _currentModel, value);
	}
}