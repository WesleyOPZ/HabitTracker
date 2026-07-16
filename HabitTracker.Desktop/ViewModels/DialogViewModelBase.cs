using System;

namespace HabitTracker.Desktop.ViewModels;
public abstract class DialogViewModelBase : ViewModelBase
{
    public Action? Close { get; set; }
}