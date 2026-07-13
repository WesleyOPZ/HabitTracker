using Avalonia.Controls;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class HabitHistoryDialog : Window {
    public HabitHistoryDialog() {
        InitializeComponent();
    }

    public HabitHistoryDialog(HabitHistoryViewModel viewModel) : this() {
        DataContext = viewModel;
    }
}