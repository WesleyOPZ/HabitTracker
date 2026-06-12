using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Desktop.Models;

namespace HabitTracker.Desktop.Converters;

public class TabActiveForegroundConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ActiveTab currentTab && parameter is string tabName &&
            Enum.TryParse<ActiveTab>(tabName, out var targetTab))
        {
            return currentTab == targetTab
                ? new SolidColorBrush(Color.Parse("#8b5cf6"))
                : new SolidColorBrush(Color.Parse("#64748b"));
        }

        return new SolidColorBrush(Color.Parse("#64748b"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
