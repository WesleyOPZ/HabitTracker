using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class DifficultyBgConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Difficulty difficulty)
            return new SolidColorBrush(Color.Parse("#1f1f22"));

        return difficulty switch
        {
            Difficulty.Easy => new SolidColorBrush(Color.Parse("#152a1e")),
            Difficulty.Normal => new SolidColorBrush(Color.Parse("#1a2640")),
            Difficulty.Hard => new SolidColorBrush(Color.Parse("#3a1212")),
            Difficulty.Legendary => new SolidColorBrush(Color.Parse("#3a2a0a")),
            _ => new SolidColorBrush(Color.Parse("#1f1f22"))
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
