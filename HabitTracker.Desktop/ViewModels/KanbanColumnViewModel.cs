using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.ViewModels;

public partial class KanbanColumnViewModel : ViewModelBase
{
    public int Id { get; }
    public string Title { get; }

    public ObservableCollection<Habit> Habits { get; } = new();

    public KanbanColumnViewModel(int id, string title)
    {
        Id = id;
        Title = title;
    }
}