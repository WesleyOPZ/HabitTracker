namespace HabitTracker.Core.Models;

public class CreateHabitResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<Achievement> NewAchievements { get; set; } = new();
}