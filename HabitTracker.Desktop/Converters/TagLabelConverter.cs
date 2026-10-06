using System;
using System.Globalization;
using Avalonia.Data.Converters;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class TagLabelConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string tagValue ? TagLocalization.GetLabel(tagValue) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}