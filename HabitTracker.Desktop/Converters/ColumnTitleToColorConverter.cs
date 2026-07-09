using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace HabitTracker.Desktop.Converters;

public class ColumnTitleToColorConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var title = value as string;

        var resourceKey = title switch {
            "To-Do" => "StatusToDo",
            "In-Progress" => "StatusInProgress",
            "Done" => "StatusDone",
            _ => "ColorCard"
        };

        if (Avalonia.Application.Current!.TryFindResource(resourceKey, out var brush))
            return brush;

        return Brushes.White;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}