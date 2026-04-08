using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Core.Models;
using HabitTracker.Desktop.Models;

namespace HabitTracker.Desktop.ViewModels;

public partial class EditProfileViewModel : ViewModelBase
{
    [ObservableProperty] private string _userName = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private Gender _gender = Gender.Unknown;
    [ObservableProperty] private DateTimeOffset? _dateOfBirth;

    public bool Confirmed { get; private set; }
    public List<Gender> Genders { get; } = Enum.GetValues<Gender>().ToList();
    public ObservableCollection<SelectableAchievement> UnlockedAchievements { get; } = new();

    public List<AchievementType> FeaturedAchievements => UnlockedAchievements
        .Where(a => a.IsSelected)
        .Select(a => a.Achievement.Type)
        .ToList();

    public EditProfileViewModel(UserProfile profile, List<Achievement> allAchievements)
    {
        UserName = profile.UserName;
        Description = profile.Description;
        Gender = profile.Gender;
        DateOfBirth = profile.DateOfBirth.HasValue ? new DateTimeOffset(profile.DateOfBirth.Value) : null;

        var unlocked = allAchievements.Where(a => a.IsUnlocked);
        foreach (var achievement in unlocked)
        {
            var isSelected = profile.FeaturedAchievements.Contains(achievement.Type);
            UnlockedAchievements.Add(new SelectableAchievement(achievement, isSelected, (() =>  CanSelectMore)));
        }
    }

    [RelayCommand]
    private void ToggleAchievement(SelectableAchievement item)
    {
        if (item.IsSelected)
        {
            item.IsSelected = false;
        }
        else
        {
            int selectedCount = UnlockedAchievements.Count(a => a.IsSelected);
            if (selectedCount >= 3) return;
            item.IsSelected = true;
        }

        OnPropertyChanged(nameof(CanSelectMore));

        foreach (var a in UnlockedAchievements)
        {
            a.NotifyCanToggleChanged();
        }
    }

    public bool CanSelectMore => UnlockedAchievements.Count(a => a.IsSelected) < 3;

    [RelayCommand]
    private void Confirm()
    {
        Confirmed = true;
        Close?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        Confirmed = false;
        Close?.Invoke();
    }

    public Action? Close { get; set; }
}