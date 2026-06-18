using System;
using System.Globalization;
using Avalonia.Data.Converters;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class HabitDoneLabelConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var completed = value is Habit h && h.IsCompletedToday();
        return completed ? "✓ Done" : "○ Complete";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}