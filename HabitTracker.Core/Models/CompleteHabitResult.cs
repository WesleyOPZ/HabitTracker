namespace HabitTracker.Core.Models;

public class CompleteHabitResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int XpEarned { get; set; }
    public bool LeveledUp { get; set; }
    public int NewLevel { get; set; }
    public bool AlreadyCompleted { get; set; }
    public List<Achievement> NewAchievements { get; set; } = new();

}