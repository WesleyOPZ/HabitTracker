using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class DifficultyColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var resourceKey = value is Difficulty difficulty
            ? $"Difficulty{difficulty}"
            : "ColorTextSecondary";

        if (Application.Current!.TryFindResource(resourceKey, out var brush))
            return brush!;

        return new SolidColorBrush(Color.Parse("#94a3b8"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
