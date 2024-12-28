using System.Globalization;
using Avalonia.Data.Converters;

namespace ChatBox.Convertors;

public class WorkspaceEditConvertor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is bool isRender && value is ChatMessageListViewModel chatMessage)
        {
            if (isRender)
            {
                if (chatMessage.IsEditing)
                {
                    return false;
                }

                return true;
            }

            if (chatMessage.IsEditing)
            {
                return true;
            }
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}