using Avalonia.Controls;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class CreateHabitDialog : Window {
    public CreateHabitDialog() {
        InitializeComponent();
    }

    public CreateHabitDialog(CreateHabitViewModel viewModel) : this() {
        DataContext = viewModel;
        viewModel.Close = Close;
    }
        
}