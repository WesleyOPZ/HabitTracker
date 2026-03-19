using Avalonia.Controls;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Views;

public partial class HabitHistoryDialog : Window
{
    public HabitHistoryDialog()
    {
        InitializeComponent();
    }

    public HabitHistoryDialog(Habit habit) : this()
    {
        DataContext = habit;
    }
}