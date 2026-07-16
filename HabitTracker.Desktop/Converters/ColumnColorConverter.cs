using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Converters {
    public class ColumnColorConverter : IMultiValueConverter {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) {
            string? colorHex = null;
            int columId = 0;

            if (values.Count > 0 && values[0] is string hex) {
                colorHex = hex;
            }

            if (values.Count > 1 && values[1] is int id) {
                columId = id;
            }

            if (!string.IsNullOrEmpty(colorHex) && Color.TryParse(colorHex, out var customColor)) {
                return new SolidColorBrush(customColor);
            }

            var resourceKey = columId switch {
                (int)FolderType.ToDo => "StatusToDo",
                (int)FolderType.InProgress => "StatusInProgress",
                (int)FolderType.Done => "StatusDone",
                _ => "ColorTextPrimary"
            };

            if (Application.Current!.TryFindResource(resourceKey, out var brush) && brush is IBrush foundBrush) {
                return foundBrush;
            }

            return Brushes.White;
        }
    }
}