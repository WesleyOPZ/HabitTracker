namespace HabitTracker.Core.Models;

public class MoveHabitResult {
    public bool LeveledUp { get; set; }
    public int NewLevel { get; set; }
    public List<Achievement> NewAchievements { get; set; } = new();

}