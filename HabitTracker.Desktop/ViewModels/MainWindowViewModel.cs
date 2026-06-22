using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Core.Models;
using HabitTracker.Core.Services;
using HabitTracker.Desktop.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using HabitTracker.Desktop.Models;

namespace HabitTracker.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    private readonly HabitService _habitService;

    public ObservableCollection<KanbanColumnViewModel> KanbanColumns { get; } = new();


    [ObservableProperty] private int _level;
    [ObservableProperty] private string _levelName = string.Empty;
    [ObservableProperty] private int _currentXp;
    [ObservableProperty] private int _xpInCurrentLevel;
    [ObservableProperty] private int _xpForNextLevel;
    [ObservableProperty] private Category? _selectedCategory;
    [ObservableProperty] private ActiveTab _activeTab = ActiveTab.Habits;
    [ObservableProperty] private string _profileUserName = string.Empty;
    [ObservableProperty] private string _profileDescription = string.Empty;
    [ObservableProperty] private string _profileGender = string.Empty;
    [ObservableProperty] private string _profileDateOfBirth = string.Empty;
    [ObservableProperty] private string _profileMemberSince = string.Empty;
    [ObservableProperty] private int _globalLongestStreak;
    [ObservableProperty] private int _statTotalHabits;
    [ObservableProperty] private int _statCompletedToday;
    [ObservableProperty] private int _statTotalXp;
    [ObservableProperty] private int _statCurrentLevel;
    [ObservableProperty] private string _statLevelName = string.Empty;
    [ObservableProperty] private int _statXpForNext;
    [ObservableProperty] private int _statXpProgress;
    [ObservableProperty] private int _statTotalCompletions;
    [ObservableProperty] private string _statBestStreakName = "No habits yet";
    [ObservableProperty] private int _statBestStreakDays;
    public ObservableCollection<Achievement> Achievements { get; } = new();

    private List<Habit> _allHabits = new();

    public record MoveHabitArgs(Habit Habit, int TargetFolderId);

    public MainWindowViewModel() {
        if (Design.IsDesignMode) {
            _habitService = null!;
            return;
        }

        _habitService = new HabitService();

        // Tenta rodar o reset. Se rodar (virou o dia), recarrega tudo.
        if (_habitService.ProcessDailyReset()) {
            // Notifica o usuário com um popup se desejar
        }

        LoadHabits();
    }

    private void LoadHabits() {
        _allHabits = _habitService.GetHabits();

        if (KanbanColumns.Count == 0) {
            KanbanColumns.Add(new KanbanColumnViewModel((int)FolderType.ToDo, "To-Do"));
            KanbanColumns.Add(new KanbanColumnViewModel((int)FolderType.InProgress, "In-Progress"));
            KanbanColumns.Add(new KanbanColumnViewModel((int)FolderType.Done, "Done"));
        }

        ApplyFilter();

        int totalXp = _habitService.GetProfile().TotalXp;
        Level = LevelSystem.CalculateLevel(totalXp);
        LevelName = LevelSystem.GetLevelName(Level);
        CurrentXp = totalXp;
        XpInCurrentLevel = LevelSystem.GetXpProgressInCurrentLevel(totalXp, Level);
        XpForNextLevel = LevelSystem.GetXpForNextLevel(Level);
        Achievements.Clear();
        foreach (var achievement in _habitService.Achievements.GetAllAchievements())
            Achievements.Add(achievement);

        LoadProfile();
        LoadStatistics();
    }

    [RelayCommand]
    private async Task OpenCreateHabit() {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        if (mainWindow == null) return;

        var viewModel = new CreateHabitViewModel();
        var dialog = new CreateHabitDialog { DataContext = viewModel };

        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed) {
            var result = _habitService.CreateHabit(viewModel.Name, viewModel.Description, viewModel.Difficulty,
                viewModel.Category);

            LoadHabits();

            await ShowAchievementPopups(result.NewAchievements);
        }
    }

    [RelayCommand]
    private async Task DeleteHabit(Habit habit) {
        var box = MessageBoxManager.GetMessageBoxStandard(
            "Delete Habit",
            $"Are you sure you want to delete '{habit.Name}'?",
            ButtonEnum.YesNo);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes) {
            _habitService.DeleteHabit(habit.Id);
            LoadHabits();
        }
    }

    [RelayCommand]
    private async Task OpenHabitHistory(Habit habit) {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        if (mainWindow == null) return;

        var dialog = new HabitHistoryDialog(habit);
        await dialog.ShowDialog(mainWindow);
    }

    [RelayCommand]
    private void ShowTab(ActiveTab tab) => ActiveTab = tab;

    private async Task ShowAchievementPopups(List<Achievement> achievements) {
        foreach (var achievement in achievements) {
            var box = MessageBoxManager.GetMessageBoxStandard("🏆 Achievement Unlocked!",
                $"{achievement.Icon} {achievement.Name}\n{achievement.Description}",
                ButtonEnum.Ok);
            await box.ShowAsync();
        }
    }

    [RelayCommand]
    private async Task OpenEditProfile() {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        if (mainWindow == null) return;

        var profile = _habitService.GetProfile();
        var viewModel = new EditProfileViewModel(profile, _habitService.Achievements.GetAllAchievements());
        var dialog = new EditProfileDialog(viewModel);

        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed) {
            profile.UserName = viewModel.UserName;
            profile.Description = viewModel.Description;
            profile.Gender = viewModel.Gender;
            profile.DateOfBirth = viewModel.DateOfBirth?.DateTime;
            profile.FeaturedAchievements = viewModel.FeaturedAchievements;

            _habitService.UpdateProfile(profile);
            LoadProfile();
        }
    }

    [RelayCommand]
    public void MoveHabitInUi(MoveHabitArgs args) {
        // Atualiza a UI (Remover da coluna antiga, adicionar na nova)
        var oldColumn = KanbanColumns.FirstOrDefault(c => c.Id == args.Habit.FolderId);
        var newColumn = KanbanColumns.FirstOrDefault(c => c.Id == args.TargetFolderId);

        if (oldColumn == null || newColumn == null) return;

        oldColumn.Habits.Remove(args.Habit);
        newColumn.Habits.Add(args.Habit);
    }

    [RelayCommand]
    public async Task MoveHabitForward(Habit habit) {
        int next = habit.FolderId + 1;
        if (next > (int)FolderType.Done) return;

        MoveHabitInUi(new MoveHabitArgs(habit, next));
        var result = _habitService.MoveHabitToFolder(habit.Id, next);
        RefreshXpStats();

        if (result.LeveledUp) {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "🎉 Level Up!",
                $"You reached Level {result.NewLevel} - {LevelSystem.GetLevelName(result.NewLevel)}!",
                ButtonEnum.Ok);
            await box.ShowAsync();
        }

        await ShowAchievementPopups(result.NewAchievements);
    }


    [RelayCommand]
    public async Task MoveHabitBack(Habit habit) {
        int prev = habit.FolderId - 1;
        if (prev < (int)FolderType.ToDo) return;
        
        MoveHabitInUi(new MoveHabitArgs(habit, prev));
        var result = _habitService.MoveHabitToFolder(habit.Id, prev);
        RefreshXpStats();

        await ShowAchievementPopups(result.NewAchievements);
    }

    public List<Category?> Categories { get; } = new() {
        null,
        Category.Health,
        Category.Study,
        Category.Work,
        Category.Personal
    };

    private void ApplyFilter() {
        foreach (var column in KanbanColumns) {
            column.Habits.Clear();
        }

        var filteredHabits = SelectedCategory == null
            ? _allHabits
            : _allHabits.Where(h => h.Category == SelectedCategory);

        foreach (var habit in filteredHabits) {
            int targetFolder = habit.FolderId > 0 ? habit.FolderId : (int)FolderType.ToDo;

            var targetColumn = KanbanColumns.FirstOrDefault(c => c.Id == targetFolder);
            if (targetColumn != null) {
                targetColumn.Habits.Add(habit);
            }
        }
    }

    partial void OnSelectedCategoryChanged(Category? value) {
        ApplyFilter();
    }

    private void LoadProfile() {
        var profile = _habitService.GetProfile();
        ProfileUserName = string.IsNullOrEmpty(profile.UserName) ? "No name set" : profile.UserName;
        ProfileDescription = string.IsNullOrEmpty(profile.Description) ? "No description set" : profile.Description;
        ProfileGender = profile.Gender == Gender.Unknown ? "Not specified" : profile.Gender.ToString();
        ProfileDateOfBirth = profile.DateOfBirth.HasValue
            ? profile.DateOfBirth.Value.ToString("dd/MM/yyyy")
            : "Not specified";
        ProfileMemberSince = profile.DateCreated.ToString("dd/MM/yyyy");
        GlobalLongestStreak = profile.GlobalLongestStreak;
    }

    private void LoadStatistics() {
        var stats = _habitService.Statistics.GetStatistics();

        StatTotalHabits = stats.TotalHabits;
        StatCompletedToday = stats.CompletedToday;
        StatTotalXp = stats.TotalXp;
        StatCurrentLevel = stats.CurrentLevel;
        StatLevelName = stats.LevelName;
        StatXpForNext = stats.XpForNext;
        StatXpProgress = stats.XpProgress;
        StatTotalCompletions = stats.TotalCompletions;

        StatBestStreakName = stats.BestStreak?.Name ?? "No habits yet";
        StatBestStreakDays = stats.BestStreak?.LongestStreak ?? 0;
    }

    private void RefreshXpStats() {
        int totalXp = _habitService.GetProfile().TotalXp;
        Level = LevelSystem.CalculateLevel(totalXp);
        LevelName = LevelSystem.GetLevelName(Level);
        CurrentXp = totalXp;
        XpInCurrentLevel = LevelSystem.GetXpProgressInCurrentLevel(totalXp, Level);
        XpForNextLevel = LevelSystem.GetXpForNextLevel(Level);
    }
}