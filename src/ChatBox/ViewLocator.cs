using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ChatBox.ViewModels;

namespace ChatBox;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().Name.Replace("ViewModel", "");
        var type = Type.GetType("ChatBox.Pages." + name);

        if (type != null)
        {
            var control = HostApplication.Services.GetService(type) as UserControl;
            control.DataContext = data;

            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}