using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class HabitStatusAccentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var completed = value is Habit h && h.IsCompletedToday();
        return completed
            ? new SolidColorBrush(Color.Parse("#22c55e"))
            : new SolidColorBrush(Color.Parse("#ef4444"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}