using System.Globalization;
using Avalonia.Data.Converters;
using ChatBox.Models;

namespace ChatBox.Convertors;

public class IsSelectedConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SessionsViewModel session)
        {
            var chatViewModel = HostApplication.Services.GetService<ChatViewModel>();
            return chatViewModel.Session == session;
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}