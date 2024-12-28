using Microsoft.Extensions.Logging;

namespace ChatBox.Logger
{
	public class ChatBoxLogger : ILogger
	{
		private readonly string _categoryName;

		public ChatBoxLogger(string categoryName)
		{
			_categoryName = categoryName;
		}

		public IDisposable BeginScope<TState>(TState state) => null;

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {_categoryName}: {formatter(state, exception)}";
			if (exception != null)
			{
				message += $"\n{exception}";
			}

			File.AppendAllText(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "logs"), $"{DateTime.Now:yyyy-MM-dd}.log"), message + Environment.NewLine);
		}
	}

	public class ChatBoxLoggerProvider : ILoggerProvider
	{
		public ILogger CreateLogger(string categoryName)
		{
			return new ChatBoxLogger(categoryName);
		}

		public void Dispose()
		{
		}
	}
}
