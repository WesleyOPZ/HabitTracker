using Avalonia.Controls;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class ColorPickerDialog : Window {
    public ColorPickerDialog() {
        InitializeComponent();
    }

    public ColorPickerDialog(ColorPickerViewModel viewModel) : this() {
        DataContext = viewModel;
        viewModel.Close = Close;
    }
}