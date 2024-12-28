using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChatBox.Logger
{
	public static class LoggerExtensions
	{
		public static ILoggingBuilder AddChatBoxLogger(this ILoggingBuilder builder)
		{
			builder.Services.AddSingleton<ILoggerProvider, ChatBoxLoggerProvider>();
			return builder;
		}
	}
}