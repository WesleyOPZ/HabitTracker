using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;
 
namespace HabitTracker.Desktop.Converters;
 
public class DifficultyToGlowConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is Difficulty.Legendary) {
            var shadowColor = Color.FromArgb(0x80, 0xF5, 0x9E, 0x0B); // 0x80 = ~50% opacidade
 
            var shadow = new BoxShadow {
                OffsetX = 0,
                OffsetY = 0,
                Blur = 20,
                Spread = 0,
                Color = shadowColor
            };
            // Retorna envolvido na struct BoxShadows exigida pelo Avalonia
            return new BoxShadows(shadow);
        }
 
        return new BoxShadows();
    }
 
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}