using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace HabitTracker.Desktop.Converters;

/// <summary>
/// Resolve Habit.ImagePath (relativo, ex: "images/a1b2c3.png") para um
/// Bitmap carregável na UI. Espelha a mesma pasta usada pelo
/// ImageStorageService (ApplicationData/HabitTracker/images) sem
/// depender de instância injetada, já que converters são resolvidos
/// estaticamente pelo XAML.
/// </summary>
public class HabitImagePathToBitmapConverter : IValueConverter {
    private static readonly string ImagesFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitTracker", "images");

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is not string relativePath || string.IsNullOrWhiteSpace(relativePath)) return null;

        string fullPath = Path.Combine(ImagesFolder, Path.GetFileName(relativePath));
        if (!File.Exists(fullPath)) return null;

        try {
            return new Bitmap(fullPath);
        } catch {
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}