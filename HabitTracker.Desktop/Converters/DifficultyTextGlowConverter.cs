using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class DifficultyTextGlowConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is Difficulty.Legendary) {
            return new DropShadowEffect {
                Color = Color.FromArgb(0x40, 0xF5, 0x9E, 0x0B),
                BlurRadius = 20,
                OffsetX = 0,
                OffsetY = 0,
                Opacity = 1
            };
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}