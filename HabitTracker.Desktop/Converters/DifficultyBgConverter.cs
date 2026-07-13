using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class DifficultyBgConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var resourceKey = value is Difficulty difficulty
            ? $"DifficultyBg{difficulty}"
            : "DifficultyBgDefault";

        if (Application.Current!.TryFindResource(resourceKey, out var brush))
            return brush;
        
        return new SolidColorBrush(Color.Parse("#1f1f22"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
