using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;
 
namespace HabitTracker.Desktop.Converters;
 
public class DifficultyToGlowConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is Difficulty.Legendary &&
            Application.Current!.TryFindResource("LegendaryGlow", out var glow) &&
            glow is BoxShadows shadows) {
            return shadows;
        }

        return new BoxShadows();
    }
 
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}