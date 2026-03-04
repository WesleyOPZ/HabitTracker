namespace HabitTracker.Models;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int TotalXP { get; set; }
    public DateTime CreatedAt { get; set; }
    public Difficulty Difficulty { get; set; } = Difficulty.Normal;
    public List<DateTime> CompletedDates { get; set; } = new List<DateTime>();
    
    public bool IsCompletedToday()
    {
        return CompletedDates.Any(d => d.Date == DateTime.Today);
    }

    public DateTime? LastCompletedDate()
    {
        return CompletedDates.Any() ? CompletedDates.Max() : null;
    }
}