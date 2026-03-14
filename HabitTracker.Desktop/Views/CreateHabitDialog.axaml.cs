using Avalonia.Controls;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class CreateHabitDialog : Window
{
    public CreateHabitDialog()
    {
        InitializeComponent();
        DataContextChanged += (_, _) =>
        {
            if (DataContext is ViewModels.CreateHabitViewModel vm)
            {
                vm.SetWindow(this);
            }
        };
    }
}