using HabitTracker.Desktop.Models;

namespace HabitTracker.Core.Models;

public class UserProfile
{
    public string UserName { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Gender Gender { get; set; } = Gender.Unknown;
    public int TotalXp { get; set; }
    public int GlobalLongestStreak { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime DateCreated { get; set; }
}

