using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters;

public class ColumnIdToCardBackgroundConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var resourceKey = value is int id
            ? id switch {
                (int)FolderType.ToDo => "StatusToDo",
                (int)FolderType.InProgress => "StatusInProgress",
                (int)FolderType.Done => "StatusDone",
                _ => "ColorCard"
            }
            : "ColorCard";

        if (Application.Current!.TryFindResource(resourceKey, out var brush) && brush is IBrush foundBrush)
            return foundBrush;
        
        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}