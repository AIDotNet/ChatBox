using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ChatBox.Convertors;

public class WorkspacePanelConvertor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string role)
        {
            if (role == value!.ToString())
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