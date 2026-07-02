using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace HabitTracker.Desktop.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = -1;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e) {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void MinimizeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        WindowState = WindowState.Minimized;
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        Close();
    }
}