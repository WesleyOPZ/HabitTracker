using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class DifficultyColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Difficulty difficulty)
            return new SolidColorBrush(Color.Parse("#94a3b8"));

        return difficulty switch
        {
            Difficulty.Easy => new SolidColorBrush(Color.Parse("#3bb478")),
            Difficulty.Normal => new SolidColorBrush(Color.Parse("#3b82f6")),
            Difficulty.Hard => new SolidColorBrush(Color.Parse("#ef4444")),
            Difficulty.Legendary => new SolidColorBrush(Color.Parse("#f59e0b")),
            _ => new SolidColorBrush(Color.Parse("#94a3b8"))
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
