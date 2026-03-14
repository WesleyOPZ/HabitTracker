using HabitTracker.Core.Models;

namespace HabitTracker.Core.Services;

public class StatisticsService
{
    private readonly List<Habit> _habits;

    public StatisticsService(List<Habit> habits)
    {
        _habits = habits;
    }

    public StatisticsResult GetStatistics()
    {
        int completedToday = _habits.Count(h => h.IsCompletedToday());
        int totalXp = _habits.Sum(h => h.TotalXp);
        int currentLevel = LevelSystem.CalculateLevel(totalXp);
        string levelName = LevelSystem.GetLevelName(currentLevel);
        int xpForNext = LevelSystem.GetXpForNextLevel(currentLevel);
        int xpProgress = LevelSystem.GetXpProgressInCurrentLevel(totalXp, currentLevel);
        int totalCompletions = _habits.Sum(h => h.CompletedDates.Count);
        var bestStreak = _habits.MaxBy(h => h.CurrentStreak);

        return new StatisticsResult
        {
            TotalHabits = _habits.Count,
            CompletedToday = completedToday,
            TotalXp = totalXp,
            CurrentLevel = currentLevel,
            LevelName = levelName,
            XpForNext = xpForNext,
            XpProgress = xpProgress,
            TotalCompletions = totalCompletions,
            BestStreak = bestStreak
        };
    }
}