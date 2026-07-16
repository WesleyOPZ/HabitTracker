using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.ViewModels;

public partial class KanbanColumnViewModel : ViewModelBase
{
    public int Id { get; }

    [ObservableProperty] private string _title;
    [ObservableProperty] private string? _color;
    
    public bool IsCustom => Id > (int)FolderType.Done;
    
    public ObservableCollection<Habit> Habits { get; } = new();

    public KanbanColumnViewModel(int id, string title, string? color = null)
    {
        Id = id;
        Title = title;
        Color = color;
    }
}