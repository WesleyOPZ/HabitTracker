using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HabitTracker.Desktop.ViewModels;
public partial class ColorPickerViewModel  : DialogViewModelBase {
    [ObservableProperty] private Color _color;
    
    public bool Confirmed { get; private set; }

    public ColorPickerViewModel(string? initialColorHex) {
        Color = initialColorHex != null && Color.TryParse(initialColorHex, out var parsed)
            ? parsed
            : Colors.White;
    }

    [RelayCommand]
    private void Confirm() {
        Confirmed = true;
        Close?.Invoke();
    }

    [RelayCommand]
    private void Cancel() {
        Confirmed = false;
        Close?.Invoke();
    }
}