using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
    // ===== Campos privados =====
    private readonly HabitService _habitService;
    private List<Habit> _allHabits = new();

    // ===== Propriedades públicas (não observáveis) =====
    public ObservableCollection<KanbanColumnViewModel> KanbanColumns { get; } = new();
    public ObservableCollection<Achievement> Achievements { get; } = new();
    public string AppVersion { get; } = GetAppVersion();

    public List<Category?> Categories { get; } = new() {
        null,
        Category.Health,
        Category.Study,
        Category.Work,
        Category.Personal
    };

    public record MoveHabitArgs(Habit Habit, int OldFolderId, int TargetFolderId);

    // ===== Estado de navegação / filtro =====
    [ObservableProperty] private ActiveTab _activeTab = ActiveTab.Habits;
    [ObservableProperty] private Category? _selectedCategory;

    // ===== Nível / XP (topo do sidebar) =====
    [ObservableProperty] private int _level;
    [ObservableProperty] private string _levelName = string.Empty;
    [ObservableProperty] private int _currentXp;
    [ObservableProperty] private int _xpInCurrentLevel;
    [ObservableProperty] private int _xpForNextLevel;

    // ===== Perfil =====
    [ObservableProperty] private string _profileUserName = string.Empty;
    [ObservableProperty] private string _profileDescription = string.Empty;
    [ObservableProperty] private string _profileGender = string.Empty;
    [ObservableProperty] private string _profileDateOfBirth = string.Empty;
    [ObservableProperty] private string _profileMemberSince = string.Empty;
    [ObservableProperty] private int _globalLongestStreak;

    // ===== Estatísticas =====
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

    // ===== Construtor =====
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

    // ===== Commands: CRUD de hábitos =====
    [RelayCommand]
    private async Task OpenCreateHabit() {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        if (mainWindow == null) return;

        var viewModel = new CreateHabitViewModel();
        var dialog = new CreateHabitDialog(viewModel);

        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed) {
            var result = _habitService.CreateHabit(viewModel.Name, viewModel.Description, viewModel.Difficulty,
                viewModel.Category);

            LoadHabits();

            await ShowAchievementPopups(result.NewAchievements);
        }
    }

    [RelayCommand]
    private async Task OpenCreateColumn() {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (mainWindow == null) return;

        var viewModel = new TextInputViewModel("New Column", "Column Name");
        var dialog = new TextInputDialog(viewModel);
        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed) {
            _habitService.CreateFolder(viewModel.Text.Trim());
            LoadHabits();
        }
    }

    [RelayCommand]
    private async Task RenameColumn(KanbanColumnViewModel column) {
        var mainWindow = (Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (mainWindow == null) return;

        var viewModel = new TextInputViewModel("Rename Column", "New Name", column.Title);
        var dialog = new TextInputDialog(viewModel);
        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed) {
            _habitService.RenameFolder(column.Id, viewModel.Text.Trim());
            column.Title = viewModel.Text.Trim();
        }
    }

    [RelayCommand]
    private async Task DeleteColumn(KanbanColumnViewModel column) {
        string message = column.Habits.Count > 0
            ? $"Column '{column.Title}' has '{column.Habits.Count}' Habits. They will be moved to To-Do. Continue?"
            : $"Delete column '{column.Title}'?";

        var confirmBox = MessageBoxManager.GetMessageBoxStandard("Delete Column", message, ButtonEnum.YesNo);
        if (await confirmBox.ShowAsync() != ButtonResult.Yes) return;

        var result = _habitService.DeleteFolder(column.Id);
        if (!result.Success) {
            var box = MessageBoxManager.GetMessageBoxStandard("Could not delete", result.Message, ButtonEnum.Ok);
            await box.ShowAsync();
            return;
        }

        LoadHabits();
    }

    [RelayCommand]
    private async Task ChangeColumnColor(KanbanColumnViewModel column) {
        var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow;
        if (mainWindow == null) return;

        var viewModel = new ColorPickerViewModel(column.Color);
        var dialog = new ColorPickerDialog(viewModel);
        await dialog.ShowDialog(mainWindow);

        if (viewModel.Confirmed) {
            string hex = viewModel.Color.ToString();
            _habitService.SetFolderColor(column.Id, hex);
            column.Color = hex;
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

        var viewModel = new HabitHistoryViewModel(habit);
        var dialog = new HabitHistoryDialog(viewModel);
        await dialog.ShowDialog(mainWindow);
    }

    // ===== Commands: Kanban (mover hábito entre colunas) =====
    [RelayCommand]
    private void MoveHabitInUi(MoveHabitArgs args) {
        // Atualiza a UI (Remover da coluna antiga, adicionar na nova)
        var oldColumn = KanbanColumns.FirstOrDefault(c => c.Id == args.OldFolderId); // <- usa o valor congelado
        var newColumn = KanbanColumns.FirstOrDefault(c => c.Id == args.TargetFolderId);

        if (oldColumn == null || newColumn == null) return;

        oldColumn.Habits.Remove(args.Habit);
        newColumn.Habits.Add(args.Habit);
    }

    [RelayCommand]
    public async Task MoveHabitForward(Habit habit) {
        int currentIndex = KanbanColumns.ToList().FindIndex(c => c.Id == habit.FolderId);
        if (currentIndex < 0 || currentIndex >= KanbanColumns.Count - 1) return;

        int targetFolderId = KanbanColumns[currentIndex + 1].Id;
        int oldFolderId = habit.FolderId;
        var result = _habitService.MoveHabitToFolder(habit.Id, targetFolderId);
        MoveHabitInUi(new MoveHabitArgs(habit, oldFolderId, targetFolderId));
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
        int currentIndex = KanbanColumns.ToList().FindIndex(c => c.Id == habit.FolderId);
        if (currentIndex <= 0) return;

        int targetFolderId = KanbanColumns[currentIndex - 1].Id;
        int oldFolderId = habit.FolderId;
        var result = _habitService.MoveHabitToFolder(habit.Id, targetFolderId);
        MoveHabitInUi(new MoveHabitArgs(habit, oldFolderId, targetFolderId));
        RefreshXpStats();

        await ShowAchievementPopups(result.NewAchievements);
    }

    // ===== Commands: Perfil / Navegação =====
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
    private void ShowTab(ActiveTab tab) => ActiveTab = tab;

    public void MoveColumnTo(KanbanColumnViewModel column, int newIndex) {
        int oldIndex = KanbanColumns.IndexOf(column);
        if (oldIndex < 0 || oldIndex == newIndex) return;

        KanbanColumns.Move(oldIndex, newIndex);
        _habitService.ReorderFolders(KanbanColumns.Select(c => c.Id).ToList());
    }

    // ===== Partial change handlers =====
    partial void OnSelectedCategoryChanged(Category? value) {
        ApplyFilter();
    }

    // ===== Helpers privados: carregamento/atualização de estado =====
    private void LoadHabits() {
        _allHabits = _habitService.GetHabits();

        SyncKanbanColumns();

        ApplyFilter();

        RefreshXpStats();

        Achievements.Clear();
        foreach (var achievement in _habitService.Achievements.GetAllAchievements())
            Achievements.Add(achievement);

        LoadProfile();
    }

    private void SyncKanbanColumns() {
        var folders = _habitService.GetFolders();

        for (int i = KanbanColumns.Count - 1; i >= 0; i--) {
            if (folders.All(f => f.Id != KanbanColumns[i].Id))
                KanbanColumns.RemoveAt(i);
        }

        for (int i = 0; i < folders.Count; i++) {
            var folder = folders[i];
            var existing = KanbanColumns.FirstOrDefault(c => c.Id == folder.Id);

            if (existing == null) {
                KanbanColumns.Insert(
                    Math.Min(i, KanbanColumns.Count), new KanbanColumnViewModel(folder.Id, folder.Name, folder.Color));
            } else {
                existing.Title = folder.Name;
                existing.Color = folder.Color;
                int currentIndex = KanbanColumns.IndexOf(existing);
                if (currentIndex != 1) KanbanColumns.Move(currentIndex, i);
            }
        }
    }

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
        StatBestStreakDays = stats.BestStreak?.CurrentStreak ?? 0;
    }

    private void RefreshXpStats() {
        int totalXp = _habitService.GetProfile().TotalXp;
        Level = LevelSystem.CalculateLevel(totalXp);
        LevelName = LevelSystem.GetLevelName(Level);
        CurrentXp = totalXp;
        XpInCurrentLevel = LevelSystem.GetXpProgressInCurrentLevel(totalXp, Level);
        XpForNextLevel = LevelSystem.GetXpForNextLevel(Level);

        LoadStatistics();
    }

    private async Task ShowAchievementPopups(List<Achievement> achievements) {
        foreach (var achievement in achievements) {
            var box = MessageBoxManager.GetMessageBoxStandard("🏆 Achievement Unlocked!",
                $"{achievement.Icon} {achievement.Name}\n{achievement.Description}",
                ButtonEnum.Ok);
            await box.ShowAsync();
        }
    }

    // ===== Utilitários estáticos =====
    private static string GetAppVersion() {
        var version = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;
        if (string.IsNullOrEmpty(version)) return "v?.?.?";

        // Remove o sufixo +hash se existir, mantém só X.Y.Z
        var cleanVersion = version.Split('+')[0];
        return $"v{cleanVersion}";
    }
}