using System;
using System.Globalization;
using Avalonia.Data.Converters;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class CategoryDisplayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Category category)
            return category.ToString();
        
        return "All";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}