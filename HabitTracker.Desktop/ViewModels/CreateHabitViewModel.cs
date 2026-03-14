using System;
using System.Collections.Generic;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.ViewModels;

public partial class CreateHabitViewModel : ViewModelBase
{
    [ObservableProperty] private string _name = "";
    [ObservableProperty] private string _description = "";
    [ObservableProperty] private Difficulty _difficulty = Difficulty.Normal;
    [ObservableProperty] private Category _category = Category.Personal;

    public IEnumerable<Difficulty> Difficulties => Enum.GetValues<Difficulty>();
    public IEnumerable<Category> Categories => Enum.GetValues<Category>();

    public bool Confirmed { get; set; }
    private Window? _window;
    public void SetWindow(Window window) => _window = window;

    [RelayCommand]
    private void Confirm()
    {
        if (string.IsNullOrWhiteSpace(Name)) return;
        Confirmed = true;
        _window?.Close();
    }

    [RelayCommand]
    private void Cancel() => _window?.Close();
}