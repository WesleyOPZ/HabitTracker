namespace HabitTracker.Core.Models;

public class Achievement
{
    public AchievementType Type { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Description { get; set; } =  string.Empty;
    public string Icon { get; set; } =  string.Empty;
    public bool IsUnlocked { get; set; }
    public DateTime? UnlockedAt { get; set; }
}