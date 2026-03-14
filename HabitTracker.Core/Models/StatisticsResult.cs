namespace HabitTracker.Core.Models;

public class StatisticsResult
{
    public int TotalHabits { get; set; }
    public int CompletedToday { get; set; }
    public int TotalXp { get; set; }
    public int CurrentLevel { get; set; }
    public string LevelName { get; set; } = string.Empty;
    public int XpForNext { get; set; }
    public int XpProgress { get; set; }
    public int TotalCompletions { get; set; }
    public Habit? BestStreak  { get; set; }
}