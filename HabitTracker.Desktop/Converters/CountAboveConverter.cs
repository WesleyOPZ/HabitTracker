using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HabitTracker.Desktop.Converters;

public class CountAboveConverter  : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is int count && parameter is string thresholdStr && int.TryParse(thresholdStr, out var threshold))
            return count > threshold;
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}