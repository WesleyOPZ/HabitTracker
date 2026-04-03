using Avalonia.Controls;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class EditProfileDialog : Window
{
    public EditProfileDialog()
    {
        InitializeComponent();
    }

    public EditProfileDialog(EditProfileViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.Close = Close;
    }
    
}