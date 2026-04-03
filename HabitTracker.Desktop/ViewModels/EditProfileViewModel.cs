using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Core.Models;
using HabitTracker.Desktop.Models;

namespace HabitTracker.Desktop.ViewModels;

public partial class EditProfileViewModel : ViewModelBase
{
    [ObservableProperty] private string _userName = string.Empty;
    [ObservableProperty] private string _description  = string.Empty;
    [ObservableProperty] private Gender  _gender = Gender.Unknown;
    [ObservableProperty] private DateTimeOffset? _dateOfBirth;
    
    public bool Confirmed {  get; private set; }

    public List<Gender> Genders { get; } = Enum.GetValues<Gender>().ToList();

    public EditProfileViewModel(UserProfile profile)
    {
        UserName = profile.UserName;
        Description = profile.Description;
        Gender =  profile.Gender;
        DateOfBirth = profile.DateOfBirth.HasValue ? new DateTimeOffset(profile.DateOfBirth.Value) : null;
    }

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