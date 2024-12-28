using System.Globalization;
using Avalonia.Data.Converters;

namespace ChatBox.Convertors;

public class NullToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not null)
        {
            if (value is string str)
            {
                return string.IsNullOrEmpty(str);
            }
        }
        else
        {
            if (value is null)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}