using System;
using System.Globalization;
using Avalonia.Data.Converters;
using HabitTracker.Desktop.Models;

namespace HabitTracker.Desktop.Converters;

public class TabVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ActiveTab currentTab && parameter is string tabName)
            return currentTab.ToString() == tabName;
        
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}