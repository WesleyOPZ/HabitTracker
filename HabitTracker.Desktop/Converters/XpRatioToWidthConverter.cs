using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HabitTracker.Desktop.Converters;


public class XpRatioToWidthConverter : IMultiValueConverter {
    private const double TrackWidth = 40;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) {
        if (values.Count < 2 || values[0] is not int current || values[1] is not int max || max <= 0)
            return 0.0;

        var ratio = Math.Clamp((double)current / max, 0.0, 1.0);
        return ratio * TrackWidth;
    }
}