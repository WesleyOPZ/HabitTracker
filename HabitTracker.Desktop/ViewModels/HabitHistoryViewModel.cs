using System;
using System.Collections.Generic;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.ViewModels;

public class HabitHistoryViewModel : ViewModelBase {
    public string Name { get; }
    public List<DateTime> CompletedDates { get; }

    public HabitHistoryViewModel(Habit habit) {
        Name = habit.Name;
        CompletedDates = habit.CompletedDates;
    }
}