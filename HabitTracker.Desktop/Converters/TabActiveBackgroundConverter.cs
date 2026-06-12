using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Desktop.Models;

namespace HabitTracker.Desktop.Converters;

public class TabActiveBackgroundConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ActiveTab currentTab && parameter is string tabName &&
            Enum.TryParse<ActiveTab>(tabName, out var targetTab))
        {
            return currentTab == targetTab
                ? new SolidColorBrush(Color.Parse("#18000000"))
                : Brushes.Transparent;
        }

        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
