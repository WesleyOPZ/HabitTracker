using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Core.Models;
using HabitTracker.Core.Services;
using HabitTracker.Desktop.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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

        int totalXp = _habitService.GetProfile().TotalXp;
        Level = LevelSystem.CalculateLevel(totalXp);
        LevelName = LevelSystem.GetLevelName(Level);
        CurrentXp = totalXp;
        XpInCurrentLevel = LevelSystem.GetXpProgressInCurrentLevel(totalXp, Level);
        XpForNextLevel = LevelSystem.GetXpForNextLevel(Level);
    }

    [RelayCommand]
    private async Task CompleteHabit(Habit habit)
    {
        var result = _habitService.CompleteHabit(habit.Id);
        LoadHabits();

        if (result.AlreadyCompleted)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Already Done!",
                $"'{habit.Name}' already completed today!",
                ButtonEnum.Ok);
            await box.ShowAsync();
            return;
        }

        if (result.LeveledUp)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "🎉 Level Up!",
                $"You reached Level {result.NewLevel} - {LevelSystem.GetLevelName(result.NewLevel)}!",
                ButtonEnum.Ok);
            await box.ShowAsync();
        }

        await ShowAchievementPopups(result.NewAchievements);
    }

    [RelayCommand]
    private async Task OpenCreateHabit()
    {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        if (mainWindow == null) return;

        var viewModel = new CreateHabitViewModel();
        var dialog = new CreateHabitDialog { DataContext = viewModel };

        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed)
        {
            var result = _habitService.CreateHabit(viewModel.Name, viewModel.Description, viewModel.Difficulty,
                viewModel.Category);

            LoadHabits();

            await ShowAchievementPopups(result.NewAchievements);
        }
    }

    [RelayCommand]
    private async Task DeleteHabit(Habit habit)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
            "Delete Habit",
            $"Are you sure you want to delete '{habit.Name}'?",
            ButtonEnum.YesNo);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            _habitService.DeleteHabit(habit.Id);
            LoadHabits();
        }
    }

    [RelayCommand]
    private async Task OpenHabitHistory(Habit habit)
    {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        if (mainWindow == null) return;

        var dialog = new HabitHistoryDialog(habit);
        await dialog.ShowDialog(mainWindow);
    }

    private async Task ShowAchievementPopups(List<Achievement> achievements)
    {
        foreach (var achievement in achievements)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("🏆 Achievement Unlocked!",
                $"{achievement.Icon} {achievement.Name}\n{achievement.Description}",
                ButtonEnum.Ok);
            await box.ShowAsync();
        }
    }
}