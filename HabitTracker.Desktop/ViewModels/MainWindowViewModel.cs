using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Core.Models;
using HabitTracker.Core.Services;

namespace HabitTracker.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly HabitService _habitService;

    public ObservableCollection<Habit> Habits { get; } = new();

    [ObservableProperty] private int _level;
    [ObservableProperty] private string _levelName = string.Empty;
    [ObservableProperty] private int _currentXp;
    [ObservableProperty] private int _xpInCurrentLevel;
    [ObservableProperty] private int _xpForNextLevel;

    public MainWindowViewModel()
    {
        _habitService = new HabitService();
        LoadHabits();
    }

    private void LoadHabits()
    {
        Habits.Clear();
        foreach (var habit in _habitService.GetHabits())
        {
            Habits.Add(habit);
        }
        
        int totalXp = _habitService.GetTotalXp();
        Level = LevelSystem.CalculateLevel(totalXp);
        LevelName = LevelSystem.GetLevelName(Level);
        CurrentXp = totalXp;
        XpInCurrentLevel = LevelSystem.GetXpProgressInCurrentLevel(totalXp, Level);
        XpForNextLevel = LevelSystem.GetXpForNextLevel(Level);
    }

    [RelayCommand]
    private void CompleteHabit(Habit habit)
    {
        _habitService.CompleteHabit(habit.Id);
        LoadHabits();
    }
}
