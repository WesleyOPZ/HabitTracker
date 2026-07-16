using Avalonia.Controls;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class TextInputDialog : Window
{
    public TextInputDialog() {
        InitializeComponent();
    }

    public TextInputDialog(TextInputViewModel viewModel) : this() {
        DataContext = viewModel;
        viewModel.Close = Close;
    }
}